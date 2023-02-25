using R2API;
using System;

namespace JhinMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Jhin
            string prefix = JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_";

            string desc = "Jhin is a high single-target burst survivor, with an emphasis on heavy hitting shots instead of rapid damage.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Jhin can only gain attackspeed from leveling. Any other sources instead increase the effectiveness of his other abilities." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a canvas upon which to paint his next masterpiece.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Jhin");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Virtuoso");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Every Moment Matters");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Jhin's attack speed scales differently from other survivors. He cannot gain attackspeed outside of leveling, and instead gains 0.25% bonus damage per 1% of bonus attackspeed. Addtionally, critical hits grant him 10% (+ 0.4% per 1% bonus attack speed) movespeed.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_NAME", "Whisper");
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_DESCRIPTION", Helpers.agilePrefix + $"Fire a bullet for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>. The fourth shot crits. Can fire up to 4 shots before needing to reload.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Jhin: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Jhin, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Jhin: Mastery");
            #endregion
            #endregion
        }
    }
}