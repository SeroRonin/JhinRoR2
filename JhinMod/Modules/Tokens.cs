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
            desc = desc + "< ! > Jhin's crits grant him bonus movement speed! Use this to escape tricky situations." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Items like Crowbar synergize well with Jhin's high burst damage potential." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Dancing Grenade and Curtain Call can be used to deal with crowds, which Jhin struggles with during the early game." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a canvas upon which to paint his next masterpiece.";
            string outroFailure = "..and so he vanished, forever seeking the perfect canvas.";

            LanguageAPI.Add(prefix + "NAME", "Jhin");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Virtuoso");
            LanguageAPI.Add(prefix + "LORE", "Jhin is a meticulous criminal psychopath who believes murder is art. Once an Ionian prisoner, but freed by shadowy elements within Ionia's ruling council, the serial killer now works as their cabal's assassin. Using his gun as his paintbrush, Jhin creates works of artistic brutality, horrifying victims and onlookers. He gains a cruel pleasure from putting on his gruesome theater, making him the ideal choice to send the most powerful of messages: terror.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Jhin");
            LanguageAPI.Add(prefix + "PROJECT_SKIN_NAME", "Project: Jhin");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Every Moment Matters");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Attackspeed only grows with level, other sources instead give <style=cIsDamage>0.25% bonus damage</style> per <style=cIsDamage>1% of bonus attackspeed</style>. Addtionally, <style=cDeath>critical hits</style> grant <style=cIsUtility>10%</style> + (<style=cIsUtility>0.4%</style> per <style=cIsDamage>1% bonus attack speed</style>) <style=cIsUtility>movespeed</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_NAME", "Whisper");
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_DESCRIPTION", Helpers.agilePrefix + $"Fire a bullet for <style=cIsDamage>{100f * StaticValues.whisperDamageCoefficient}% damage</style>. The fourth shot crits. Can fire up to 4 shots before needing to reload.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Dancing Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a grenade for <style=cIsDamage>{100f * StaticValues.dancingGrenadeDamageCoefficient}% damage</style>. The grenade bounces to nearby enemies up to 3 additional times. Each bounce gains an additional <style=cIsDamage>35% total damage</style> if it kills the enemy it hits.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Deadly Flourish");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", Helpers.stunningPrefix + $"Fire a <style=cIsDamage>piercing</style> beam for <style=cIsDamage>{100f * StaticValues.deadlyFlourishDamageCoefficient}% damage</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Curtain Call");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", Helpers.executingPrefix + $"Reload Whisper and empower your next 4 primary shots, firing explosive rounds for <style=cIsDamage>{100f * StaticValues.curtainCallDamageCoefficient}% damage</style>. Deals bonus damage based on enemy's missing health. The fourth shot crits.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Jhin: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Jhin, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Jhin: Mastery");


            LanguageAPI.Add(prefix + "DANCINGGRENADE_ACHIEVEMENT_NAME", "Jhin: Crescendo");
            LanguageAPI.Add(prefix + "DANCINGGRENADE_ACHIEVEMENT_DESC", "As Jhin, kill an enemy with each bounce of 'Dancing Grenade'");
            #endregion
            #endregion
        }
    }
}