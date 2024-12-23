﻿using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Utils;
using UnityEngine.AddressableAssets;
using JhinMod.Content.Components;

namespace JhinMod.Modules.CustomProjectiles
{
    public class ProjectileDancingGrenade : LightningOrb, IOrbFixedUpdateBehavior
    {
        public float damageCoefficientOnBounceKill;
        public float initialDistance;
        public float spawnTime;

        public GameObject ghostPrefab;
        public BounceVisualizer bounceVis;

        public Vector3 targetLastPosition;
        public bool lostTarget;
        public GameObject fakeTarget;

        public override void Begin()
        {
            base.duration = base.distanceToTarget / this.speed;
            base.canBounceOnSameTarget = false;

            ghostPrefab = Helpers.GetVFXDynamic("Grenade", this.attacker);

            bounceVis = ghostPrefab.GetComponent<BounceVisualizer>();
            bounceVis.projectileSpeed = this.speed;
            bounceVis.initialDistance = base.distanceToTarget;
            bounceVis.isActive = (this.bouncesRemaining < 3);

            EffectData effectData = new EffectData
            {
                origin = this.origin,
                genericFloat = base.duration
            };

            effectData.SetHurtBoxReference(this.target);
            EffectManager.SpawnEffect(ghostPrefab, effectData, true);

            if (this.bouncesRemaining < 3)
            {
                Helpers.PlaySound("QTravelBounce", this.attacker, this.target.gameObject, fallback: "QTravel");
            }
            else
                Helpers.PlaySound("QTravel", this.attacker, this.target.gameObject);
        }

        public override void OnArrival()
        {
            if (this.target)
            {
                HealthComponent healthComponent = this.target.healthComponent;
                lastTarget = this.target.healthComponent;

                if (healthComponent)
                {
                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.damage = this.damageValue;
                    damageInfo.attacker = this.attacker;
                    damageInfo.inflictor = this.inflictor;
                    damageInfo.force = Vector3.zero;
                    damageInfo.crit = this.isCrit;
                    damageInfo.procChainMask = this.procChainMask;
                    damageInfo.procCoefficient = this.procCoefficient;
                    damageInfo.position = this.target.transform.position;
                    damageInfo.damageColorIndex = this.damageColorIndex;
                    damageInfo.damageType = this.damageType;

                    var skillLocator = this.attacker.GetComponent<SkillLocator>();
                    if (skillLocator && skillLocator.utility.cooldownRemaining < 4)
                    {
                        R2API.DamageAPI.AddModdedDamageType(damageInfo, Modules.Buffs.JhinMarkDamage);
                    }

                    healthComponent.TakeDamage(damageInfo);

                    GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
                }
                //Did we kill the target we hit?
                this.failedToKill |= (!healthComponent || healthComponent.alive);

                var vfxString = this.failedToKill ? "GrenadeImpact" : "GrenadeImpactKill"; 
                Helpers.PlayVFXDynamic(vfxString, this.attacker.gameObject, parent: healthComponent.gameObject);
            }

            if (this.bouncesRemaining > 0)
            {
                for (int i = 0; i < this.targetsToFindPerBounce; i++)
                {
                    if ( this.bouncedObjects != null && this.target )
                    {
                        this.bouncedObjects.Add(this.target.healthComponent);
                    }

                    HurtBox hurtBox = this.PickNextTargetWithNewPriority(this.targetLastPosition);
                    if (hurtBox)
                    {
                        ProjectileDancingGrenade lightningOrb = new ProjectileDancingGrenade();
                        lightningOrb.search = this.search;
                        lightningOrb.origin = this.targetLastPosition;
                        lightningOrb.target = hurtBox;
                        lightningOrb.attacker = this.attacker;
                        lightningOrb.inflictor = this.inflictor;
                        lightningOrb.teamIndex = this.teamIndex;
                        lightningOrb.damageValue = this.damageValue;

                        var speedModif = 3f;
                        var speedSet = Math.Min(lightningOrb.distanceToTarget * speedModif, JhinMod.SkillStates.DancingGrenade.projectileTravelSpeed);

                        lightningOrb.bouncesRemaining = this.bouncesRemaining - 1;
                        lightningOrb.isCrit = this.isCrit;
                        lightningOrb.bouncedObjects = this.bouncedObjects;
                        lightningOrb.deadObjects = this.deadObjects;
                        lightningOrb.lightningType = this.lightningType;
                        lightningOrb.procChainMask = this.procChainMask;
                        lightningOrb.procCoefficient = this.procCoefficient;
                        lightningOrb.damageColorIndex = this.damageColorIndex;
                        lightningOrb.damageCoefficientOnBounceKill = this.damageCoefficientOnBounceKill;
                        lightningOrb.speed = speedSet;
                        lightningOrb.range = this.range;
                        lightningOrb.damageType = this.damageType;
                        lightningOrb.duration = speedSet;
                        lightningOrb.initialDistance = distanceToTarget;

                        //If we killed, add a percentage of current damage on top
                        if (!this.failedToKill)
                        {
                            lightningOrb.damageValue += this.damageValue * this.damageCoefficientOnBounceKill;
                        }

                        OrbManager.instance.AddOrb(lightningOrb);
                        Helpers.PlaySound("QHit", this.attacker, this.target ? this.target.gameObject : this.fakeTarget);
                    }
                    else
                    {
                        Helpers.PlaySound("QHitLast", this.attacker, this.target ? this.target.gameObject : this.fakeTarget, fallback: "QHit");
                    }
                }
            }
            else if (this.bouncesRemaining == 0)
            {
                Helpers.PlaySound("QHitLast", this.attacker, this.target ? this.target.gameObject : this.fakeTarget, fallback: "QHit");
            }

            if ( this.fakeTarget )
            {
                GameObject.Destroy( this.fakeTarget, 1f );
            }
        }

        //These values are specifically used for this custom PickNextTarget method
        public List<HealthComponent> deadObjects;
        public HealthComponent lastTarget;

        public HurtBox PickNextTargetWithNewPriority(Vector3 position)
        {
            if (this.search == null)
            {
                this.search = new BullseyeSearch();
            }
            this.search.searchOrigin = position;
            this.search.searchDirection = Vector3.zero;
            this.search.teamMaskFilter = TeamMask.allButNeutral;
            this.search.teamMaskFilter.RemoveTeam(this.teamIndex);
            this.search.filterByLoS = false;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.range;
            this.search.RefreshCandidates();
            HurtBox hurtBox = (from v in this.search.GetResults()
                               where this.bouncedObjects.Contains(v.healthComponent) && !this.deadObjects.Contains(v.healthComponent) && this.lastTarget != v.healthComponent
                               select v).FirstOrDefault<HurtBox>();
            HurtBox hurtBoxNew = (from v in this.search.GetResults()
                               where !this.bouncedObjects.Contains(v.healthComponent) && !this.deadObjects.Contains(v.healthComponent)
                                  select v).FirstOrDefault<HurtBox>();

            HurtBox outputHurtbox = null;

            //Do we have any new targets? If so, prioritize
            if (hurtBoxNew)
            {
                outputHurtbox = hurtBoxNew;
            }
            else if (hurtBox)
            {
                outputHurtbox = hurtBox;
            }

            
            //Recursively check for non-dead entities to bounce to
            if (outputHurtbox)
            {
                if (!outputHurtbox.healthComponent.alive)
                {
                    this.deadObjects.Add(outputHurtbox.healthComponent);

                    var notDead = PickNextTargetWithNewPriority(position);
                    outputHurtbox = notDead;
                }
            }

            return outputHurtbox;
        }

        public void FixedUpdate()
        {
            if ( this.target )
            {
                this.targetLastPosition = this.target.gameObject.transform.position;
            }
            if ( !this.lostTarget && !this.target )
            {
                this.fakeTarget = new GameObject();
                fakeTarget.transform.position = this.targetLastPosition;
                this.lostTarget = true;
            }
        }
    }
}
