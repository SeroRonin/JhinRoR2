using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using JhinMod.Modules;

namespace JhinMod.SkillStates.BaseStates
{
    internal class AnimatedDeathState : GenericCharacterDeath
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Helpers.PlaySound("DeathSFX", base.gameObject);

            Animator modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {
                modelAnimator.CrossFadeInFixedTime("Death", 0.1f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
