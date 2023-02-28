using EntityStates;
using RoR2;
using JhinMod.Content.Controllers;
using UnityEngine;
using R2API.Utils;

namespace JhinMod.SkillStates
{
    public class WhisperReload : BaseState
    {
        private AmmoComponent ammoComponent;
        private float duration;
        private bool hasReloaded;

        public override void OnEnter()
        {
            base.OnEnter();
            this.ammoComponent = base.GetComponent<AmmoComponent>();
            this.duration = ammoComponent.reloadTime;

            base.PlayCrossfade("UpperBody, Override", (ammoComponent.timeSinceEmpty < 0.5f) ? "Reload_FromFireEmpty" : "Reload", "", this.duration, 0.2f);

            //Util.PlayAttackSpeedSound(Reload.enterSoundString, base.gameObject, Reload.enterSoundPitch);
        }

        public override void OnExit()
        {
            base.OnExit(); 
            ammoComponent.StopReload();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (ammoComponent.startedReload && base.fixedAge >= this.duration )
            {
                this.PerformReload(); 
                this.outer.SetNextStateToMain();
            }
            if (!base.isAuthority || base.fixedAge < this.duration)
            {
                return;
            }

            //Util.PlayAttackSpeedSound(Reload.exitSoundString, base.gameObject, Reload.exitSoundPitch);
            
        }

        private void PerformReload()
        {
            if (this.hasReloaded)
            {
                return;
            }
            
            ammoComponent.Reload( true );
            base.skillLocator.primary.stock = ammoComponent.ammoCount;

            this.hasReloaded = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}