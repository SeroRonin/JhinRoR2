﻿using System;
using JetBrains.Annotations;
using UnityEngine;
using RoR2.Skills;
using RoR2;
using JhinMod.Content.Components;
using JhinMod.SkillStates.BaseStates;
using EntityStates;

namespace JhinMod.Modules.SkillDefs
{
    /// <summary>
    /// This SkillDef is used to disable the special skill while in use via JhinStateController
    /// This prevents casting the special multiple times without any benefit
    /// </summary>
    [CreateAssetMenu(menuName = "RoR2/SkillDef/JhinUltSkillDefSkillDef")]
    public class JhinUltSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                jhinAmmoComponent = skillSlot.GetComponent<JhinStateController>(),
                jhinUltState = Helpers.GetEntityStateMachine(skillSlot.gameObject, "WeaponMode").state
            };
        }

        private static bool IsUlting([NotNull] GenericSkill skillSlot)
        {
            JhinStateController jhinAmmoComponent = ((InstanceData)skillSlot.skillInstanceData).jhinAmmoComponent;
            EntityState jhinUltState = ((InstanceData)skillSlot.skillInstanceData).jhinUltState;
            var isUlting = jhinAmmoComponent.isUlting;
            var isExitingUlt = false;
            if ( jhinUltState is JhinWeaponUltActiveState )
            {
                isExitingUlt = (jhinUltState as JhinWeaponUltActiveState).startedExitEffects;
            }
            return (isUlting && !isExitingUlt);
        }
        private static bool IsAttacking([NotNull] GenericSkill skillSlot)
        {
            JhinStateController jhinAmmoComponent = ((InstanceData)skillSlot.skillInstanceData).jhinAmmoComponent;
            return jhinAmmoComponent.isAttacking;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return base.CanExecute(skillSlot) && !IsUlting(skillSlot) && !IsAttacking(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && !IsUlting(skillSlot) && !IsAttacking(skillSlot);
        }

        protected class InstanceData : BaseSkillInstanceData
        {
            public JhinStateController jhinAmmoComponent;
            public EntityState jhinUltState;
        }
    }
}
