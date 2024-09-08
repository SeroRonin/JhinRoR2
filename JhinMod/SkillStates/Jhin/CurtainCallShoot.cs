using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using JhinMod.Modules;
using JhinMod.Content.Components;
using UnityEngine.AddressableAssets;
using R2API.Utils;
using R2API;

namespace JhinMod.SkillStates
{
    public class CurtainCallShoot : BaseSkillState
    {
        public static float damageCoefficient = Config.specialDamageCoefficient.Value;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static float projectileSpeed = 200f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public JhinStateController jhinStateController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.jhinStateController = this.GetComponent<JhinStateController>();
            this.duration = CurtainCallShoot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            Helpers.PlaySound("UltLoadShot", base.gameObject);
            //Util.PlaySound("Play_Seroronin_Jhin_UltLoadShot", base.gameObject);

            if (jhinStateController.ultFX != null)
            {
                Animator ultAnimator = jhinStateController.ultFX.GetComponent<Animator>();
                if (ultAnimator)
                {
                    EntityState.PlayAnimationOnAnimator( ultAnimator, "Base Layer", "Fire");
                }
            }

            base.PlayAnimation("UpperBody, Override", "CurtainCallAttack");
        }

        public virtual bool CheckCrit()
        {
            return RollCrit();
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

                var tempPrefab = Modules.Projectiles.ultMissilePrefab;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Helpers.PlaySound("UltFire", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * CurtainCallShoot.recoil, -2f * CurtainCallShoot.recoil, -0.5f * CurtainCallShoot.recoil, 0.5f * CurtainCallShoot.recoil);

                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = tempPrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = this.gameObject,
                        damage = this.characterBody.damage * CurtainCallShoot.damageCoefficient,
                        force = CurtainCallShoot.force,
                        crit = CheckCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        target = null,
                        speedOverride = CurtainCallShoot.projectileSpeed,
                        fuseOverride = -1f,
                        damageTypeOverride = DamageType.Generic
                    };

                    ProjectileManager.instance.FireProjectile( fireProjectileInfo );
                }

                jhinStateController.TakeAmmo(1);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}