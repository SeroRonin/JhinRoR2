using EntityStates;
using RoR2;
using UnityEngine;

namespace JhinMod.SkillStates
{
    public class WhisperReload : BaseState
    {
        public static float baseDuration = 2.5f;

        private float duration;
        private bool hasReloaded;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = WhisperReload.baseDuration;

            base.PlayAnimation("UpperBody, Override", (base.skillLocator.primary.stock == 0) ? "Reload" : "ReloadCrit", "1f", this.duration);
            
            //Util.PlayAttackSpeedSound(Reload.enterSoundString, base.gameObject, Reload.enterSoundPitch);
        }

        public override void OnExit()
        {
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration )
            {
                this.PerformReload();
            }
            if (!base.isAuthority || base.fixedAge < this.duration)
            {
                return;
            }

            //Util.PlayAttackSpeedSound(Reload.exitSoundString, base.gameObject, Reload.exitSoundPitch);
            this.outer.SetNextStateToMain();
        }
        private void PerformReload()
        {
            if (this.hasReloaded)
            {
                return;
            }
            base.skillLocator.primary.stock = base.skillLocator.primary.maxStock ;
            this.hasReloaded = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}