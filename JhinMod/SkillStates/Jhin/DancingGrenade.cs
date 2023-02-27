using EntityStates;
using JhinMod.Modules;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace JhinMod.SkillStates
{
    public class DancingGrenade : GenericProjectileBaseState
    {

        public static float BaseDuration = 0.2f;
        public static float BaseDelayDuration = 0.05f;

        public static float DamageCoefficient = StaticValues.dancingGrenadeDamageCoefficient;

        public override void OnEnter()
        {
            base.projectilePrefab = Modules.Projectiles.bombPrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            base.attackSoundString = "HenryBombThrow";

            base.baseDuration = BaseDuration;
            base.baseDelayBeforeFiringProjectile = BaseDelayDuration;

            base.damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            base.force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            base.recoilAmplitude = 0.1f;
            base.bloom = 10;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration)
        {

            if (base.GetModelAnimator())
            {
                base.PlayAnimation("UpperBody, Override", "DancingGrenade");
            }
        }
    }
}