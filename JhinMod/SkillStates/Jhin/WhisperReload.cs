using EntityStates;
using RoR2;
using JhinMod.Content.Components;
using UnityEngine;
using R2API.Utils;
using JhinMod.Modules;

namespace JhinMod.SkillStates
{
    public class WhisperReload : BaseState
    {
        private JhinStateController jhinStateController;
        private float duration;
        private bool hasReloaded;

        public override void OnEnter()
        {
            base.OnEnter();
            this.jhinStateController = base.GetComponent<JhinStateController>();
            this.duration = jhinStateController.reloadTime;
            var recentlyEmpty = this.jhinStateController.ammoCount == 0 && this.jhinStateController.timeSinceFire < 0.5f;
            base.PlayCrossfade("UpperBody, Override", recentlyEmpty ? "Reload_FromFireEmpty" : "Reload", "", this.duration, 0.2f);

            Helpers.PlaySoundDynamic(recentlyEmpty ? "ReloadEmpty" : "Reload", base.gameObject);
            Helpers.StopSoundDynamic("PassiveCritSpin", base.gameObject);
            Helpers.StopSoundDynamic("PassiveCritMusic", base.gameObject);
            //Util.PlaySound(recentlyEmpty ? "Play_Seroronin_Jhin_ReloadEmpty" : "Play_Seroronin_Jhin_Reload", base.gameObject);
            //Util.PlaySound("Stop_Seroronin_Jhin_PassiveCritSpin", base.gameObject);
            //Util.PlaySound("Stop_Seroronin_Jhin_PassiveCritMusic", base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.jhinStateController.startedReload && base.fixedAge >= this.duration )
            {
                this.PerformReload(); 
                this.outer.SetNextStateToMain();
            }
            if (!base.isAuthority || base.fixedAge < this.duration)
            {
                return;
            }
        }

        private void PerformReload()
        {
            if (this.hasReloaded)
            {
                return;
            }

            this.jhinStateController.Reload( true );
            base.skillLocator.primary.stock = this.jhinStateController.ammoCount;

            this.hasReloaded = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}