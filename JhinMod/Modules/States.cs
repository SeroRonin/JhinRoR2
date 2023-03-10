using JhinMod.SkillStates;
using JhinMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using JhinMod.SkillStates.Henry;

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

            //Jhin EntityStates

            Modules.Content.AddEntityState(typeof(AnimatedDeathState));

            Modules.Content.AddEntityState(typeof(WhisperPrimary));
            Modules.Content.AddEntityState(typeof(WhisperReload));

            Modules.Content.AddEntityState(typeof(DancingGrenade));

            Modules.Content.AddEntityState(typeof(DeadlyFlourish));

            Modules.Content.AddEntityState(typeof(CurtainCall));
            Modules.Content.AddEntityState(typeof(CurtainCallShoot));
            Modules.Content.AddEntityState(typeof(CurtainCallShootCrit));

            Modules.Content.AddEntityState(typeof(JhinWeaponMainState));
            Modules.Content.AddEntityState(typeof(JhinWeaponPassiveCritReadyState));
            Modules.Content.AddEntityState(typeof(JhinWeaponUltActiveState));
        }
    }
}