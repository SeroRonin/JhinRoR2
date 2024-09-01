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
    public class CurtainCallCancel : BaseSkillState
    {
        public float duration = 1f;
        private JhinStateController jhinStateController;
        private EntityStateMachine ultStateMachine;
        private bool wantsCancel;

        public override void OnEnter()
        {
            jhinStateController = base.GetComponent<JhinStateController>();
            jhinStateController.isUlting = false;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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
