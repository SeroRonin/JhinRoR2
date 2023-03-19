using System;
using System.Collections.Generic;
using RoR2;
using RoR2.Orbs;
using EntityStates;
using EntityStates.Huntress.HuntressWeapon;
using UnityEngine;
using UnityEngine.Networking;
using JhinMod.Modules.CustomProjectiles;
using JhinMod.Modules;
using R2API.Utils;
using UnityEngine.UIElements;
using R2API.Networking.Interfaces;
using JhinMod.Content.Components;

namespace JhinMod.SkillStates
{
    public class DancingGrenade : BaseState
    {
        public static float baseDuration = 1f;

        public static float baseDelay = 0.25f;

        public static GameObject muzzleFlashPrefab;

        public static float projectileProcCoefficient = 1f;

        public static int maxBounceCount = 3;

        public static float projectileTravelSpeed = 40f;

        public static float projectileBounceRange = 25f;

        public static string attackSoundString;

        public float damageCoefficient;

        public float damageCoefficientOnBounceKill;

        private float duration;

        private float stopwatch;

        private Animator animator;

        private GameObject chargeEffect;

        private Transform modelTransform;

        private JhinTracker tracker;

        private ChildLocator childLocator;

        private bool hasTriedToFire;

        private bool hasFired;

        private HurtBox initialOrbTarget;
        private JhinStateController jhinStateController;
        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.duration = DancingGrenade.baseDuration;
            this.damageCoefficient = StaticValues.dancingGrenadeDamageCoefficient;
            this.damageCoefficientOnBounceKill = StaticValues.dancingGrenadeBounceKillDamageCoefficient;
            this.modelTransform = base.GetModelTransform();
            this.animator = base.GetModelAnimator();
            this.tracker = base.GetComponent<JhinTracker>();
            this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
            this.jhinStateController = GetComponent<JhinStateController>();

            //Util.PlayAttackSpeedSound(ThrowDancingGrenade.attackSoundString, base.gameObject, this.attackSpeedStat);

            if (this.tracker && base.isAuthority)
            {
                this.initialOrbTarget = this.tracker.GetTrackingTarget();
            }

            this.jhinStateController.StopReload();
            base.PlayAnimation("UpperBody, Override", "DancingGrenade");
            Helpers.PlaySoundDynamic("QCast", base.gameObject);
            //Util.PlaySound("Play_Seroronin_Jhin_QCast", base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            /*
            if (this.chargeEffect)
            {
                EntityState.Destroy(this.chargeEffect);
            }
            int layerIndex = this.animator.GetLayerIndex("Impact");
            if (layerIndex >= 0)
            {
                this.animator.SetLayerWeight(layerIndex, 1.5f);
                this.animator.PlayInFixedTime("LightImpact", layerIndex, 0f);
            }
            */
            if (!this.hasFired && NetworkServer.active)
            {
                base.skillLocator.secondary.AddOneStock();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
            if (!this.hasTriedToFire && !hasFired && this.stopwatch > DancingGrenade.baseDelay )
            {
                this.FireOrbDancingGrenade();
            }

            //CharacterMotor characterMotor = base.characterMotor;
            //characterMotor.velocity.y = characterMotor.velocity.y + ThrowDancingGrenade.antigravityStrength * Time.fixedDeltaTime * (1f - this.stopwatch / this.duration);

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void FireOrbDancingGrenade()
        {
            if (!NetworkServer.active || this.hasTriedToFire)
            {
                return;
            }
            this.hasTriedToFire = true;
            ProjectileDancingGrenade dancingGrenade = new ProjectileDancingGrenade();
            dancingGrenade.targetsToFindPerBounce = 1;
            dancingGrenade.damageValue = base.characterBody.damage * this.damageCoefficient;
            dancingGrenade.isCrit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            dancingGrenade.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            dancingGrenade.attacker = base.gameObject;
            dancingGrenade.procCoefficient = DancingGrenade.projectileProcCoefficient;
            dancingGrenade.bouncesRemaining = DancingGrenade.maxBounceCount;
            dancingGrenade.speed = DancingGrenade.projectileTravelSpeed;
            dancingGrenade.bouncedObjects = new List<HealthComponent>();
            dancingGrenade.range = DancingGrenade.projectileBounceRange;
            dancingGrenade.damageCoefficientOnBounceKill = this.damageCoefficientOnBounceKill;
            HurtBox hurtBox = this.initialOrbTarget;
            if (hurtBox)
            {
                this.hasFired = true;
                Transform transform = this.childLocator.FindChild("ShoulderR");
                //EffectManager.SimpleMuzzleFlash(DancingGrenade.muzzleFlashPrefab, base.gameObject, "HandR", true);
                dancingGrenade.origin = transform.position;
                dancingGrenade.target = hurtBox;
                OrbManager.instance.AddOrb(dancingGrenade);
            }
            Helpers.PlaySoundDynamic("QFire", base.gameObject);
            //Util.PlaySound("Play_Seroronin_Jhin_QFire", base.gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}
