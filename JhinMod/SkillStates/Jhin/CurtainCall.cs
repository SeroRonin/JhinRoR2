using System;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using UnityEngine;
using EntityStates.Railgunner.Scope;
using JhinMod.Content.Components;
using JhinMod.Modules;
using EntityStates;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using EntityStates.LemurianMonster;
using JhinMod.SkillStates.BaseStates;
using R2API.Utils;

namespace JhinMod.SkillStates
{
    public class CurtainCall : BaseSkillState
    {
        public float duration = 10f;
        private JhinStateController jhinStateController;
        private EntityStateMachine ultStateMachine;

        public override void OnEnter()
        {
            base.OnEnter();
            jhinStateController = base.GetComponent<JhinStateController>();
            ultStateMachine = Helpers.GetEntityStateMachine(this.gameObject, "WeaponMode");

            //Stop our current reload state if active, and fill our ammo manually
            jhinStateController.StopReload( true );
            jhinStateController.Reload(true);
            jhinStateController.isUlting= true;

            Helpers.PlaySoundDynamic("UltCast",base.gameObject);
            Helpers.PlaySoundDynamic("UltMusic", base.gameObject);

            this.ultStateMachine.SetNextState( new JhinWeaponUltActiveState() );
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && ( !jhinStateController.isUlting || this.fixedAge > this.duration ))
            {
                //this.ultStateMachine.SetNextStateToMain();
                //this.outer.SetNextStateToMain();
                //jhinStateController.ResetUlt();
            }
        }

        protected virtual BaseWindDown GetNextState()
        {
            return new BaseWindDown();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
