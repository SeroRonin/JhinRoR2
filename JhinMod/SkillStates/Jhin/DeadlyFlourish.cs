﻿using EntityStates;
using JhinMod.Content.Components;
using JhinMod.Modules;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using static RoR2.BulletAttack;
using static UnityEngine.SpookyHash;

namespace JhinMod.SkillStates
{
    public class DeadlyFlourish : BaseSkillState
    {
        public static float damageCoefficient = Config.utilityDamageCoefficient.Value;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.15f;
        public static float baseFireDelay = 0.75f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private JhinStateController jhinStateController;
        public override void OnEnter()
        {
            base.OnEnter();

            this.jhinStateController = GetComponent<JhinStateController>();
            this.jhinStateController.isAttacking = false;

            if (jhinStateController.ammoCount != 0)
            {
                this.jhinStateController.StopReload(true, 2f);
            }

            this.duration = DeadlyFlourish.baseDuration;
            this.fireTime = DeadlyFlourish.baseFireDelay;
            base.StartAimMode(duration, true);
            this.muzzleString = "Muzzle";

            if (base.characterDirection)
            {
                base.characterDirection.moveVector = base.characterDirection.forward;
            }

            Helpers.PlaySound("WCast", base.gameObject);

            base.PlayAnimation("FullBody, Override", "DeadlyFlourish");
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    base.AddRecoil(-1f * DeadlyFlourish.recoil, -2f * DeadlyFlourish.recoil, -0.5f * DeadlyFlourish.recoil, 0.5f * DeadlyFlourish.recoil);

                    Helpers.PlayVFXDynamic("DeadlyFlourishBeam", base.gameObject, muzzleString, true, aimRay);
                    Helpers.PlayVFXDynamic("DeadlyFlourishMuzzle", base.gameObject, muzzleString, true, aimRay);

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = DeadlyFlourish.damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Stun1s,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = DeadlyFlourish.range,
                        force = DeadlyFlourish.force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 1f,
                        sniper = false,
                        stopperMask = 0,
                        weapon = null,
                        tracerEffectPrefab = DeadlyFlourish.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitCallback = BulletHitCallback,
                        modifyOutgoingDamageCallback = ModifyDamage,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
            }
        }
        private bool BulletHitCallback(BulletAttack bulletAttack, ref BulletHit hitInfo)
        {

            var result = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            HealthComponent healthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;

            
            if (healthComponent && hitInfo.hitHurtBox.teamIndex != base.teamComponent.teamIndex)
            {
                base.characterBody.AddTimedBuff(Modules.Buffs.jhinCritMovespeedBuff, Modules.Config.passiveBuffDuration.Value * Modules.Config.utilityBuffMultiplier.Value );
            }

            return result;
        }

        private void ModifyDamage(BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
        {
            R2API.DamageAPI.AddModdedDamageType(damageInfo, Modules.Buffs.JhinConsumeMarkDamage);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!hasFired)
            {
                var aimAnimator = GetAimAnimator();
                if (aimAnimator)
                {
                    aimAnimator.AimImmediate();
                }

                if (base.characterMotor && base.inputBank)
                {
                    base.characterDirection.moveVector = base.inputBank.aimDirection;
                }
            }

            HandleMovements();
            

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public virtual void HandleMovements()
        {
            if (!base.characterMotor.isGrounded)
            {
                base.characterMotor.moveDirection = this.inputBank.moveVector;
            }
            else
            {
                base.characterMotor.moveDirection = Vector3.zero;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}