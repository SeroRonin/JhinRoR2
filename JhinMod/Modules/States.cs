using JhinMod.SkillStates;
using JhinMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace JhinMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(AnimatedDeathState));

            Modules.Content.AddEntityState(typeof(WhisperPrimary));
            Modules.Content.AddEntityState(typeof(WhisperReload));

            Modules.Content.AddEntityState(typeof(DancingGrenade));

            Modules.Content.AddEntityState(typeof(DeadlyFlourish));

            Modules.Content.AddEntityState(typeof(CurtainCall));
            Modules.Content.AddEntityState(typeof(CurtainCallShot));
        }
    }
}