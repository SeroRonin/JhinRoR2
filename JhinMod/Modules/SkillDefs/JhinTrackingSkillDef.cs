using System;
using JetBrains.Annotations;
using UnityEngine;
using RoR2.Skills;
using RoR2;
using JhinMod.Content.Controllers;

namespace JhinMod.Modules.SkillDefs
{
    /// <summary>
    /// This SkillDef used by Dancing Grenade to retrieve tracking data, almost 1:1 with Huntress' tracking SkillDef
    /// </summary>
    [CreateAssetMenu(menuName = "RoR2/SkillDef/JhinTrackingSkillDef")]
    public class JhinTrackingSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                jhinTracker = skillSlot.GetComponent<JhinTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            JhinTracker jhinTracker = ((InstanceData)skillSlot.skillInstanceData).jhinTracker;
            return jhinTracker != null ? jhinTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }

        protected class InstanceData : BaseSkillInstanceData
        {
            public JhinTracker jhinTracker;
        }
    }
}
