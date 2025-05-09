﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RoR2;

namespace JhinMod.Content.Components
{
    /// <summary>
    /// Tracker Component used by JhinTrackingSkillDef and DancingGrenade. Almost 1:1 with HuntressTracker, but we can't access the base Unity methods so we need a custom MonoBehavior.
    /// </summary>
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(TeamComponent))]
    public class JhinTracker : MonoBehaviour
    {
        public float maxTrackingDistance = 40f;
        public float maxTrackingAngle = 25f;
        public float trackerUpdateFrequency = 10f;

        private HurtBox trackingTarget;

        private CharacterBody characterBody;
        private TeamComponent teamComponent;
        private InputBankTest inputBank;
        private SkillLocator skillLocator;
        private float trackerUpdateStopwatch;

        private Indicator indicator;
        private string indicatorPrefab = "Prefabs/HuntressTrackingIndicator"; //Replace with custom prefab later

        private readonly BullseyeSearch search = new BullseyeSearch();

        private void Awake()
        {
            this.indicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>(indicatorPrefab));
        }

        private void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
            this.skillLocator = base.GetComponent<SkillLocator>();
        }

        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }

        private void OnEnable()
        {
            this.indicator.active = true;
        }

        private void OnDisable()
        {
            this.indicator.active = false;
        }

        private void FixedUpdate()
        {
            if (this.skillLocator.secondary.skillOverrides.Length > 0)
                this.indicator.active = false;
            else 
                this.indicator.active = true;

            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                HurtBox hurtBox = this.trackingTarget;
                Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
                this.SearchForTarget(aimRay);
                this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
            }
        }

        private void SearchForTarget(Ray aimRay)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.maxTrackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
        }
    }
}
