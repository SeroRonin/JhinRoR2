using System;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using UnityEngine;
using EntityStates.Railgunner.Scope;
using JhinMod.Content.Controllers;
using JhinMod.Modules;
using EntityStates;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using EntityStates.LemurianMonster;
using JhinMod.SkillStates.BaseStates;

namespace JhinMod.SkillStates
{
    public class CurtainCall : BaseSkillState
    {
        private float duration = 10f;
        private JhinStateController jhinStateController;
        private EntityStateMachine ultStateMachine;
        private int shotsRemaining;
        public override void OnEnter()
        {
            base.OnEnter();
            jhinStateController = base.GetComponent<JhinStateController>();
            var allSMs = this.gameObject.GetComponents<EntityStateMachine>();

            //Stop our current reload state if active, and fill our ammo manually
            jhinStateController.StopReload( true );
            jhinStateController.Reload(true);
            jhinStateController.isUlting= true;

            foreach ( EntityStateMachine entSM in allSMs )
            {
                if (entSM.customName == "WeaponMode")
                {
                    ultStateMachine = entSM;
                    break;
                }
            }

            Util.PlaySound("JhinUltCast", base.gameObject);
            Util.PlaySound("JhinUltMusic", base.gameObject);

            this.ultStateMachine.SetNextState( new JhinUltActiveState() );
        }

        public override void OnExit()
        {
            jhinStateController.isUlting = false;
            this.ultStateMachine.SetNextStateToMain();

            Util.PlaySound("JhinStopUltMusic", base.gameObject);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && this.fixedAge > this.duration)
            {
                this.outer.SetNextState(this.GetNextState());
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
