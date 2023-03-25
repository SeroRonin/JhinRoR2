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
            desc = desc + "< ! > Items like Crowbar and Elemental Bands synergize well with Jhin's high burst damage potential." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Dancing Grenade and Curtain Call can be used to deal with crowds, which Jhin struggles with during the early game." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a canvas upon which to paint his next masterpiece.";
            string outroFailure = "..and so he vanished, forever seeking the perfect canvas.";

            LanguageAPI.Add(prefix + "NAME", "Jhin");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Virtuoso");
            LanguageAPI.Add(prefix + "LORE", "Jhin is a meticulous criminal psychopath who believes murder is art. Once an Ionian prisoner, but freed by shadowy elements within Ionia's ruling council, the serial killer now works as their cabal's assassin. Using his gun as his paintbrush, Jhin creates works of artistic brutality, horrifying victims and onlookers. He gains a cruel pleasure from putting on his gruesome theater, making him the ideal choice to send the most powerful of messages: terror.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            //LanguageAPI.Add("KEYWORD_EXECUTING", Helpers.KeywordText("Executing", $"Deals <style=cIsDamage>bonus damage</style> based on <style=cIsHealth> target's missing health</style>."));
            LanguageAPI.Add("KEYWORD_EXECUTING_WHISPER", Helpers.KeywordText("Executing: Primary", $"Deals <style=cIsDamage>bonus damage</style> equal to <style=cIsHealth>{100f * Config.primaryExecuteMissingHealthPercentage.Value}% of the target's missing health</style>. This bonus caps at <style=cIsDamage>{100f * Config.primaryExecuteDamageCap.Value}% of the original damage</style>."));
            LanguageAPI.Add("KEYWORD_EXECUTING_SPECIAL", Helpers.KeywordText("Executing: Special", $"Deals <style=cIsDamage>{100f * Config.specialExecutePercentage.Value}% bonus damage</style> per <style=cIsHealth>1% of target's missing health</style>."));
            LanguageAPI.Add("KEYWORD_RELOAD", Helpers.KeywordText("Reload", $"Enter a reload state after firing 4 <style=cIsUtility>Primary</style> shots, or after 10 seconds. The timer is reset after using any skill."));

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Jhin");
            LanguageAPI.Add(prefix + "HIGHNOON_SKIN_NAME", "High Noon Jhin");
            LanguageAPI.Add(prefix + "BLOODMOON_SKIN_NAME", "Blood Moon Jhin");
            LanguageAPI.Add(prefix + "PROJECT_SKIN_NAME", "PROJECT: Jhin");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Every Moment Matters");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Attack speed only grows with level, other sources instead give <style=cIsDamage>0.25% bonus damage</style> per <style=cIsDamage>1% of bonus attack speed</style>. Addtionally, <style=cDeath>critical hits</style> grant <style=cIsUtility>10%</style> + (<style=cIsUtility>0.4%</style> per <style=cIsDamage>1% bonus attack speed</style>) <style=cIsUtility>bonus movement speed</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_NAME", "Whisper");
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_DESCRIPTION", Helpers.agilePrefix + $"Fire a bullet for <style=cIsDamage>{100f * Modules.Config.primaryDamageCoefficient.Value}% damage</style>. The fourth shot <style=cDeath>critically strikes</style> and is {Helpers.executingPrefix}Can fire up to 4 shots before needing to <style=cIsUtility>reload</style>.");
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_CRIT_DESCRIPTION", Helpers.agilePrefix + Helpers.executingPrefix + $"Fire a bullet for <style=cIsDamage>{100f * Modules.Config.primaryDamageCoefficient.Value}% damage</style>. <style=cDeath>This shot critically strikes</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GRENADE_NAME", "Dancing Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_GRENADE_DESCRIPTION", Helpers.agilePrefix + $"Fire a grenade for <style=cIsDamage>{100f * Config.secondaryDamageCoefficient.Value}% damage</style>. The grenade bounces to nearby enemies up to <style=cIsDamage>3</style> additional times. Each bounce gains an additional <style=cIsDamage>{100f * Config.secondaryDamageBounceCoefficient.Value}% TOTAL damage</style> if it kills the enemy it hits.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_FLOURISH_NAME", "Deadly Flourish");
            LanguageAPI.Add(prefix + "UTILITY_FLOURISH_DESCRIPTION", Helpers.stunningPrefix + $"Fire a <style=cIsDamage>piercing</style> beam for <style=cIsDamage>{100f * Config.utilityDamageCoefficient.Value}% damage</style>. Triggers <style=cIsUtility>Every Moment Matters</style> as if Jhin had crit.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_ULT_NAME", "Curtain Call");
            LanguageAPI.Add(prefix + "SPECIAL_ULT_DESCRIPTION", Helpers.executingPrefix + $"Instantly <style=cIsUtility>reload</style> and empower your next 4 primary shots, firing explosive rounds for <style=cIsDamage>{100f * Config.specialDamageCoefficient.Value}% damage</style>. <style=cDeath>The fourth shot critically strikes</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_ULT_SHOT_DESCRIPTION", Helpers.executingPrefix + $"Fire an explosive round for <style=cIsDamage>{100f * Config.specialDamageCoefficient.Value}% damage</style>.");
            LanguageAPI.Add(prefix + "SPECIAL_ULT_SHOT_CRIT_DESCRIPTION", Helpers.executingPrefix + $"Fire an explosive round for <style=cIsDamage>{100f * Config.specialDamageCoefficient.Value}% damage</style>. <style=cDeath>This shot critically strikes</style>.");

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