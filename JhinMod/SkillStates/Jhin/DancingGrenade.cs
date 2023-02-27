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

namespace JhinMod.SkillStates
{
    // Token: 0x02000325 RID: 805
    public class DancingGrenade : BaseState
    {
        public static float baseDuration = 0.2f;

        public static float baseDelay = 0.05f;

        public static GameObject muzzleFlashPrefab;

        public static float damageCoefficientOnSuccessfulKill = 0.35f;

        public static float damageCoefficientPerBounce = 1.1f;

        public static float projectileProcCoefficient = 1f;

        public static int maxBounceCount = 3;

        public static float projectileTravelSpeed = 25f;

        public static float projectileBounceRange = 25f;

        public static string attackSoundString;

        public float damageCoefficient;

        private float duration;

        private float stopwatch;

        private Animator animator;

        private GameObject chargeEffect;

        private Transform modelTransform;

        private HuntressTracker huntressTracker;

        private ChildLocator childLocator;

        private bool hasTriedToFire;

        private bool hasFired;

        private HurtBox initialOrbTarget;
        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.duration = DancingGrenade.baseDuration;
            this.damageCoefficient = StaticValues.dancingGrenadeDamageCoefficient;
            this.modelTransform = base.GetModelTransform();
            this.animator = base.GetModelAnimator();
            this.huntressTracker = base.GetComponent<HuntressTracker>();
            this.childLocator = this.modelTransform.GetComponent<ChildLocator>();

            //Util.PlayAttackSpeedSound(ThrowDancingGrenade.attackSoundString, base.gameObject, this.attackSpeedStat);

            if (this.huntressTracker && base.isAuthority)
            {
                this.initialOrbTarget = this.huntressTracker.GetTrackingTarget();
            }
            if ( this.initialOrbTarget )
            {
                base.PlayAnimation("UpperBody, Override", "DancingGrenade");
            }
            /*
            if (this.modelTransform)
            {
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
                if (this.childLocator)
                {
                    Transform transform = this.childLocator.FindChild("HandR");
                    if (transform && ThrowGlaive.chargePrefab)
                    {
                        this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(ThrowGlaive.chargePrefab, transform.position, transform.rotation);
                        this.chargeEffect.transform.parent = transform;
                    }
                }
            }
            */
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
            if (!this.hasTriedToThrowGlaive)
            {
                this.FireOrbGlaive();
            }*/
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
                this.FireOrbGlaive();
            }

            //CharacterMotor characterMotor = base.characterMotor;
            //characterMotor.velocity.y = characterMotor.velocity.y + ThrowDancingGrenade.antigravityStrength * Time.fixedDeltaTime * (1f - this.stopwatch / this.duration);

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06000E6F RID: 3695 RVA: 0x0003E6F8 File Offset: 0x0003C8F8
        private void FireOrbGlaive()
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
            dancingGrenade.damageCoefficientOnSuccessfulKill = DancingGrenade.damageCoefficientOnSuccessfulKill;
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
        }

        // Token: 0x06000E70 RID: 3696 RVA: 0x0000EE13 File Offset: 0x0000D013
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        // Token: 0x06000E71 RID: 3697 RVA: 0x0003E81B File Offset: 0x0003CA1B
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }

        // Token: 0x06000E72 RID: 3698 RVA: 0x0003E830 File Offset: 0x0003CA30
        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}
