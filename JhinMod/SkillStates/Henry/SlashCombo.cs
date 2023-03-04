using JhinMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace JhinMod.SkillStates.Henry
{
    public class SlashCombo : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            hitboxName = "Sword";

            damageType = DamageType.Generic;
            damageCoefficient = Modules.StaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;
            attackStartTime = 0.2f;
            attackEndTime = 0.4f;
            baseEarlyExitTime = 0.4f;
            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "JhinSwordSwing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            swingEffectPrefab = Modules.Assets.swordSwingEffect;
            hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            impactSound = Modules.Assets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState()
        {
            int index = swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            outer.SetNextState(new SlashCombo
            {
                swingIndex = index
            });
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}