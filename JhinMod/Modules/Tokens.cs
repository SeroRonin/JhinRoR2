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

            string desc = "Jhin is a high damage, single-target burst survivor, with an emphasis on heavy hitting shots instead of rapid damage.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Jhin can only gain attack speed from leveling. Any other sources instead increase the damage effectiveness of his other abilities." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Jhin's crits grant him bonus movement speed! Use the guaranteed crit from his fourth shot to escape tricky situations." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Deadly Flourish can be used to interrupt attacks you otherwise aren't able to avoid. Useful for tripping up those pesky golems!" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Dancing Grenade and Curtain Call can be used to deal with crowds, which Jhin can struggle with during the early game." + Environment.NewLine + Environment.NewLine;

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
            LanguageAPI.Add("KEYWORD_EXECUTING_SPECIAL", Helpers.KeywordText("Executing: Special", $"Deals up to <style=cIsDamage>{100f * Config.specialExecutePercentage.Value}% bonus damage</style> based on the <style=cIsHealth>target's missing health</style>."));
            LanguageAPI.Add("KEYWORD_RELOAD", Helpers.KeywordText("Reload", $"Enter a reload state after firing 4 <color=#ff5078>Whisper</color> shots, or after <style=cIsUtility>{Config.primaryAutoReloadTime.Value}</style> seconds. <i>The timer is reset after using any skill.</i>"));
            LanguageAPI.Add("KEYWORD_CAPTIVATING", Helpers.KeywordText("Captivating", $"Jhin's other skills <style=cDeath>mark</style> enemies for <style=cIsUtility>{Config.utilityMarkDuration.Value}</style> seconds. Hitting a marked enemy with <color=#ff5078>Deadly Flourish</color> consumes the mark, <style=cIsDamage>rooting</style> them for <style=cIsUtility>{Config.utilityRootDuration.Value}</style> seconds."));

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Jhin");
            LanguageAPI.Add(prefix + "HIGHNOON_SKIN_NAME", "High Noon Jhin");
            LanguageAPI.Add(prefix + "BLOODMOON_SKIN_NAME", "Blood Moon Jhin");
            LanguageAPI.Add(prefix + "SKTT1_SKIN_NAME", "SKT T1 Jhin");
            LanguageAPI.Add(prefix + "PROJECT_SKIN_NAME", "PROJECT: Jhin");
            LanguageAPI.Add(prefix + "SHANHAI_SKIN_NAME", "Shan Hai Scrolls Jhin");
            LanguageAPI.Add(prefix + "DWG_SKIN_NAME", "DWG Jhin");
            LanguageAPI.Add(prefix + "EMPYREAN_SKIN_NAME", "Empyrean Jhin");
            LanguageAPI.Add(prefix + "SOULFIGHTER_SKIN_NAME", "Soul Fighter Jhin");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Every Moment Matters");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"Jhin can only gain <style=cIsDamage>Attack Speed</style> from level growth. <style=cIsDamage>{100 * Modules.Config.passiveDamageConversion.Value}% of additional Attack Speed</style> is instead converted into <style=cDeath>Percent Bonus Damage</style>. Addtionally, <style=cDeath>critical hits</style> grant <style=cIsUtility>10%</style> + (<style=cIsUtility>{Modules.Config.passiveMovespeedConversion.Value}%</style> per <style=cIsDamage>1% bonus attack speed</style>) <style=cIsUtility>bonus movement speed</style> for <style=cIsUtility>{Config.passiveBuffDuration.Value}</style> seconds.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_NAME", "Whisper");
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_DESCRIPTION", Helpers.agilePrefix + $"Fire a bullet for <style=cIsDamage>{100f * Modules.Config.primaryDamageCoefficient.Value}% damage</style>. The fourth shot <style=cDeath>critically strikes</style> and is <color=#ff5078>exectuing</color>. Can fire up to 4 shots before needing to <style=cIsUtility>reload</style>.");
            LanguageAPI.Add(prefix + "PRIMARY_WHISPER_CRIT_DESCRIPTION", Helpers.agilePrefix + Helpers.executingPrefix + $"Fire a bullet for <style=cIsDamage>{100f * Modules.Config.primaryDamageCoefficient.Value}% damage</style>. <style=cDeath>This shot critically strikes</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GRENADE_NAME", "Dancing Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_GRENADE_DESCRIPTION", Helpers.agilePrefix + $"Fire a targeted grenade for <style=cIsDamage>{100f * Config.secondaryDamageCoefficient.Value}% damage</style>. The grenade bounces to a nearby enemy up to <style=cIsDamage>3</style> additional times. Each bounce gains an additional <style=cIsDamage>{100f * Config.secondaryDamageBounceCoefficient.Value}% TOTAL damage</style> if the enemy <style=cDeath>dies</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_FLOURISH_NAME", "Deadly Flourish");
            LanguageAPI.Add(prefix + "UTILITY_FLOURISH_DESCRIPTION", Helpers.captivatingPrefix + Helpers.stunningPrefix + $"Fire a <style=cIsDamage>piercing</style> beam for <style=cIsDamage>{100f * Config.utilityDamageCoefficient.Value}% damage</style>. Damaging an enemy triggers <color=#ff5078>Every Moment Matters</color> as if Jhin had crit, which lasts <style=cIsUtility>{Modules.Config.passiveBuffDuration.Value * Modules.Config.utilityBuffMultiplier.Value}</style> seconds.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_ULT_NAME", "Curtain Call");
            LanguageAPI.Add(prefix + "SPECIAL_ULT_DESCRIPTION", Helpers.executingPrefix + $"Instantly <style=cIsUtility>reload</style> and empower your primary skill. For the next <style=cIsUtility>10 seconds</style>, using your primary skill fires <style=cIsDamage>explosive</style> rounds for <style=cIsDamage>{100f * Config.specialDamageCoefficient.Value}% damage</style>. This lasts for up to <style=cIsDamage>4</style> shots. <style=cDeath>The fourth shot critically strikes</style>.");
            
            LanguageAPI.Add(prefix + "SPECIAL_ULT_CANCEL_DESCRIPTION", Helpers.executingPrefix + $"End Curtain Call early.");
            LanguageAPI.Add(prefix + "SPECIAL_ULT_SHOT_DESCRIPTION", Helpers.executingPrefix + $"Fire an <style=cIsDamage>explosive</style> round for <style=cIsDamage>{100f * Config.specialDamageCoefficient.Value}% damage</style>.");
            LanguageAPI.Add(prefix + "SPECIAL_ULT_SHOT_CRIT_DESCRIPTION", Helpers.executingPrefix + $"Fire an <style=cIsDamage>explosive</style> round for <style=cIsDamage>{100f * Config.specialDamageCoefficient.Value}% damage</style>. <style=cDeath>This shot critically strikes</style>.");

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