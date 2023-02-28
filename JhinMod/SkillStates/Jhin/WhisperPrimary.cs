using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.BulletAttack;
using R2API.Utils;
using JhinMod.Content.Controllers;

namespace JhinMod.SkillStates
{
    public class WhisperPrimary : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.whisperDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.6f;
        public static float baseFireDelay = 0.15625f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 512f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private AmmoComponent ammoComponent;
        private float duration;
        private float fireTime = 1f;
        private bool hasFired;
        private bool isCrit;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            ammoComponent = base.GetComponent<AmmoComponent>();
            base.characterBody.SetAimTimer(3f);
            this.muzzleString = "Muzzle";

            if ( CanFire() )
            {
                this.duration = this.attackSpeedStat / this.characterBody.baseAttackSpeed;
                this.fireTime = WhisperPrimary.baseFireDelay * this.duration;
                base.PlayAnimation("UpperBody, Override", "Attack", "ShootGun.playbackRate", duration);
            }
            else
            {
                duration = 0.1f;
            }
                
        }
        
        public bool CanFire()
        {
            return ammoComponent.CanTakeAmmo(1);
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

                //Roll crit, if we only have the last shot, crit regardless
                this.isCrit = RollCrit();
                if (ammoComponent.ammoCount == 1 ) isCrit = true;

                base.characterBody.SetAimTimer(this.duration * 2f);
                base.characterBody.AddSpreadBloom(1.5f);
                this.DoFireEffects();
                base.AddRecoil(-1f * WhisperPrimary.recoil, -2f * WhisperPrimary.recoil, -0.5f * WhisperPrimary.recoil, 0.5f * WhisperPrimary.recoil);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    BulletAttack bulletAttack = this.GenerateBulletAttack(aimRay);
                    this.ModifyBullet(bulletAttack);
                    bulletAttack.Fire();
                    this.OnFireBulletAuthority(aimRay);
                }

                ammoComponent.TakeAmmo(1);
            }
        }
        protected BulletAttack GenerateBulletAttack(Ray aimRay)
        {
            return new BulletAttack
            {
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                owner = base.gameObject,
                weapon = null,
                bulletCount = 1,
                damage = this.damageStat * WhisperPrimary.damageCoefficient,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.Buckshot,
                force = WhisperPrimary.force,
                HitEffectNormal = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = WhisperPrimary.procCoefficient,
                maxDistance = WhisperPrimary.range,
                radius = 0.75f,
                isCrit = isCrit,
                muzzleName = muzzleString,
                minSpread = 0f,
                maxSpread = 0f,
                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                smartCollision = false,
                sniper = false,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                tracerEffectPrefab = WhisperPrimary.tracerEffectPrefab,
                hitCallback = BulletHitCallback
            };
        }

        protected virtual void DoFireEffects()
        {
            Util.PlaySound("HenryShootPistol", base.gameObject);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
        }
        protected virtual void OnFireBulletAuthority(Ray aimRay)
        {
        }

        protected virtual void ModifyBullet(BulletAttack bulletAttack)
        {
            //Make 4th shot deal % missing health damage
        }

        private bool BulletHitCallback(BulletAttack bulletAttack, ref BulletHit hitInfo)
        {
            
            var result = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            HealthComponent healthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;
            /*
            if (ammoComponent.ammoCount == 1 && healthComponent && hitInfo.hitHurtBox.teamIndex != base.teamComponent.teamIndex)
            {
                
            }*/
            return result;
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ( CanFire() && base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}