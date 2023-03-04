using System;
using JetBrains.Annotations;
using UnityEngine;
using RoR2.Skills;
using RoR2;
using JhinMod.Content.Controllers;

namespace JhinMod.Modules.SkillDefs
{
    /// <summary>
    /// This SkillDef is used to disable the primary skill when we run out of ammo via JhinStateController
    /// </summary>
    [CreateAssetMenu(menuName = "RoR2/SkillDef/JhinAmmoSkillDefSkillDef")]
    public class JhinAmmoSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                jhinAmmoComponent = skillSlot.GetComponent<JhinStateController>()
            };
        }

        private static bool HasAmmo([NotNull] GenericSkill skillSlot)
        {
            JhinStateController jhinAmmoComponent = ((InstanceData)skillSlot.skillInstanceData).jhinAmmoComponent;
            return jhinAmmoComponent.ammoCount != 0;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HasAmmo(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasAmmo(skillSlot);
        }

        protected class InstanceData : BaseSkillInstanceData
        {
            public JhinStateController jhinAmmoComponent;
        }
    }
}
