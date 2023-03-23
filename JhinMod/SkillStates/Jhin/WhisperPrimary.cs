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
using JhinMod.Content.Components;
using static RoR2.DotController;
using JhinMod.Modules;

namespace JhinMod.SkillStates
{
    public class WhisperPrimary : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.whisperDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 2.57f;
        //public static float baseFireDelayPercent = 0.15625f; //League value, is actually 4ish frames late
        public static float baseFireDelayPercent = 0.1f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 512f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private JhinStateController jhinStateController;
        private float duration;
        private float earlyExitTime;
        private float fireTime;
        private bool hasFired;
        private bool isCrit;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            jhinStateController = base.GetComponent<JhinStateController>();
            jhinStateController.timeSinceFire = 0f;
            jhinStateController.ResetReload();

            base.characterBody.SetAimTimer(3f);
            this.muzzleString = "Muzzle";
            this.isCrit = RollCrit();
            this.earlyExitTime = 1 / this.characterBody.attackSpeed;

            this.duration = WhisperPrimary.baseDuration * (this.characterBody.baseAttackSpeed / this.characterBody.attackSpeed);
            this.fireTime = WhisperPrimary.baseFireDelayPercent * this.duration;
            this.PlayFireAnimation();

            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1);

            Helpers.PlaySoundDynamic(shotIndex == 4 ? "PassiveCritCast" : $"AttackCast{shotIndex}", base.gameObject);
            Helpers.StopSoundDynamic("PassiveCritSpin", base.gameObject);
            Helpers.StopSoundDynamic("PassiveCritMusic", base.gameObject);
            //Util.PlaySound(shotIndex == 4 ? "Play_Seroronin_Jhin_PassiveCritCast" : $"Play_Seroronin_Jhin_AttackCast{shotIndex}", base.gameObject);
            //Util.PlaySound("Stop_Seroronin_Jhin_PassiveCritSpin", base.gameObject);
            //Util.PlaySound("Stop_Seroronin_Jhin_PassiveCritMusic", base.gameObject);

        }
        public void PlayFireAnimation()
        {
            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1 );
            ChatMessage.Send($"shot index {shotIndex}");
            if (shotIndex == 4)
            {
                var animatorComponent = this.GetModelAnimator();
                var layerIndex = animatorComponent.GetLayerIndex("UpperBody, Override");
                animatorComponent.SetLayerWeight(layerIndex, 0f);
                base.PlayAnimation("FullBody Passive Crit, Override", "AttackPassiveCrit", "ShootGun.playbackRate", duration);
            }
            else if (this.isCrit)
            {
                    
                base.PlayAnimation("UpperBody, Override", "AttackCrit", "ShootGun.playbackRate", duration);
            }
            else
            {
                base.PlayAnimation("UpperBody, Override", $"Attack{shotIndex}", "ShootGun.playbackRate", duration);
            }
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

                //If we only have the last shot, crit regardless
                if (jhinStateController.ammoCount == 1)
                {
                    isCrit = true;
                    var animatorComponent = this.GetModelAnimator();
                    var layerIndex = animatorComponent.GetLayerIndex("UpperBody, Override");
                    animatorComponent.SetLayerWeight(layerIndex, 1f);
                }

                base.characterBody.SetAimTimer(this.duration * 2f);
                base.characterBody.AddSpreadBloom(1.5f);
                this.DoFireEffects();
                base.AddRecoil(-1f * WhisperPrimary.recoil, -2f * WhisperPrimary.recoil, -0.5f * WhisperPrimary.recoil, 0.5f * WhisperPrimary.recoil);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    BulletAttack bulletAttack = this.GenerateBulletAttack(aimRay);
                    //this.ModifyBullet(bulletAttack);
                    bulletAttack.Fire();
                    this.OnFireBulletAuthority(aimRay);
                }

                jhinStateController.TakeAmmo(1);
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
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
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
                hitCallback = BulletHitCallback,
                modifyOutgoingDamageCallback = ModifyDamage
            };
        }
        protected virtual void DoFireEffects()
        {
            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1);
            if (shotIndex ==  4)
            {
                Helpers.PlaySoundDynamic("PassiveCritFire", base.gameObject);
                //Util.PlaySound("Play_Seroronin_Jhin_PassiveCritFire", base.gameObject);
            }
            else if (this.isCrit)
            {
                Helpers.PlaySoundDynamic("AttackCritFire", base.gameObject);
                //Util.PlaySound("Play_Seroronin_Jhin_AttackCritFire", base.gameObject);
            }
            else
            {
                Helpers.PlaySoundDynamic($"AttackFire{shotIndex}", base.gameObject);
                //Util.PlaySound($"Play_Seroronin_Jhin_AttackFire{shotIndex}", base.gameObject);
            }
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
        }

        protected virtual void OnFireBulletAuthority(Ray aimRay)
        {
        }
        
        /*
        protected virtual void ModifyBullet(BulletAttack bulletAttack)
        {
            //Make 4th shot deal % missing health damage
        }
        */

        private bool BulletHitCallback(BulletAttack bulletAttack, ref BulletHit hitInfo)
        {
            
            var result = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            HealthComponent healthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;
            
            /*
            if (jhinStateController.ammoCount == 1 && healthComponent && hitInfo.hitHurtBox.teamIndex != base.teamComponent.teamIndex)
            {
                
            }
            */

            return result;
        }
        
        private void ModifyDamage(BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
        {
            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1);
            var targetHealthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;

            if (targetHealthComponent && hitInfo.hitHurtBox.teamIndex != base.teamComponent.teamIndex)
            {
                if (shotIndex == 4)
                {
                    var executeDamage = (targetHealthComponent.fullHealth - targetHealthComponent.health) * StaticValues.executePrimaryMissingHealthDamagePercent;
                    var maxDamage = (damageInfo.damage * StaticValues.executePrimaryDamagePercentCap);

                    damageInfo.damage += Math.Min(executeDamage, maxDamage);
                    ChatMessage.Send($"Execute: {executeDamage}");
                    ChatMessage.Send($"Max Execute: {maxDamage}");
                    ChatMessage.Send($"Total Execute: {Math.Min(executeDamage, maxDamage)}");
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ( base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }
            if ( hasFired && base.fixedAge >= this.earlyExitTime)
            {
                if (this.inputBank.skill1.down)
                {
                    this.outer.SetNextState(new WhisperPrimary());
                }
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