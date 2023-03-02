using System;
using JetBrains.Annotations;
using UnityEngine;
using RoR2.Skills;
using RoR2;

namespace JhinMod.Content.Controllers
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/JhinAmmoSkillDefSkillDef")]
    public class JhinAmmoSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new JhinAmmoSkillDef.InstanceData
            {
                jhinAmmoComponent = skillSlot.GetComponent<JhinStateController>()
            };
        }

        private static bool HasAmmo([NotNull] GenericSkill skillSlot)
        {
            JhinStateController jhinAmmoComponent = ((JhinAmmoSkillDef.InstanceData)skillSlot.skillInstanceData).jhinAmmoComponent;
            return (jhinAmmoComponent.ammoCount != 0);
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return JhinAmmoSkillDef.HasAmmo(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && JhinAmmoSkillDef.HasAmmo(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public JhinStateController jhinAmmoComponent;
        }
    }
}
