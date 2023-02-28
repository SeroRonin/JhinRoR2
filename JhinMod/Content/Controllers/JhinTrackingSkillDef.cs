using System;
using JetBrains.Annotations;
using UnityEngine;
using RoR2.Skills;
using RoR2;

namespace JhinMod.Content.Controllers
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/JhinTrackingSkillDef")]
    public class JhinTrackingSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new JhinTrackingSkillDef.InstanceData
            {
                jhinTracker = skillSlot.GetComponent<JhinTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            JhinTracker jhinTracker = ((JhinTrackingSkillDef.InstanceData)skillSlot.skillInstanceData).jhinTracker;
            return (jhinTracker != null) ? jhinTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return JhinTrackingSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && JhinTrackingSkillDef.HasTarget(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public JhinTracker jhinTracker;
        }
    }
}
