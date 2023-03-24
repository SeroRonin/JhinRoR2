using BepInEx.Configuration;
using System;
using UnityEngine;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;

namespace JhinMod.Modules
{
    public static class Config
    {

        public static ConfigEntry<float> healthBase;
        public static ConfigEntry<float> healthGrowth;

        public static ConfigEntry<float> regenBase;
        public static ConfigEntry<float> regenGrowth;

        public static ConfigEntry<float> armorBase;
        public static ConfigEntry<float> armorGrowth;

        public static ConfigEntry<float> damageBase;
        public static ConfigEntry<float> damageGrowth;

        public static ConfigEntry<float> attackSpeedBase;
        public static ConfigEntry<float> attackSpeedGrowth;

        public static ConfigEntry<float> critBase;
        public static ConfigEntry<float> critGrowth;

        public static ConfigEntry<float> movementSpeedBase;
        public static ConfigEntry<float> movementSpeedGrowth;

        public static ConfigEntry<float> passiveDuration;

        public static ConfigEntry<float> primaryDamageCoefficient;
        public static ConfigEntry<float> primaryExecuteMissingHealthPercentage;
        public static ConfigEntry<float> primaryExecuteDamageCap;

        public static ConfigEntry<float> secondaryCD;
        public static ConfigEntry<float> secondaryDamageCoefficient;
        public static ConfigEntry<float> secondaryDamageBounceCoefficient;

        public static ConfigEntry<float> utilityCD;
        public static ConfigEntry<float> utilityDamageCoefficient;

        public static ConfigEntry<float> specialCD;
        public static ConfigEntry<float> specialDamageCoefficient;
        public static ConfigEntry<float> specialExecutePercentage;

        public static ConfigEntry<SFXChoice> sfxChoice;

        //Unused atm, may use Lemonlust-based emote and VO code
        public static ConfigEntry<KeyCode> tauntKeybind;
        public static ConfigEntry<KeyCode> jokeKeybind;
        public static ConfigEntry<KeyCode> laughKeybind;
        public static ConfigEntry<KeyCode> danceKeybind;

        public static ConfigEntry<bool> voiceLines;


        public static void ReadConfig()
        {
            #region Base Stats
            healthBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Health: Base"), 110f, new ConfigDescription(CreateOptionDesc("", 110f)));
            healthGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Health: Growth"), 33f, new ConfigDescription(CreateOptionDesc("", 33f)));

            regenBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Health Regen: Base"), 1.5f, new ConfigDescription(CreateOptionDesc("", 1.5f)));
            regenGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Health Regen: Growth"), 0.2f, new ConfigDescription(CreateOptionDesc("", 0.2f)));

            armorBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Armor: Base"), 0f, new ConfigDescription(CreateOptionDesc("", 0f)));
            armorGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Armor: Growth"), 0f, new ConfigDescription(CreateOptionDesc("", 0f)));

            damageBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Damage: Base"), 12f, new ConfigDescription(CreateOptionDesc("", 12f)));
            damageGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Damage: Growth"), 0.24f, new ConfigDescription(CreateOptionDesc("", 0.24f)));

            attackSpeedBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Attack Speed: Base"), 0.625f, new ConfigDescription(CreateOptionDesc("", 0.625f)));
            attackSpeedGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Attack Speed: Growth"), 0.019f, new ConfigDescription(CreateOptionDesc("", 0.019f)));

            movementSpeedBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Movement Speed: Base"), 7f, new ConfigDescription(CreateOptionDesc("", 7f)));
            movementSpeedGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Movement Speed: Growth"), 0f, new ConfigDescription(CreateOptionDesc("", 0f)));

            critBase = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Crit: Base"), 1f, new ConfigDescription(CreateOptionDesc("", 1f)));
            critGrowth = JhinPlugin.instance.Config.Bind<float>(new ConfigDefinition("Character Stats", "Crit: Growth"), 0f, new ConfigDescription(CreateOptionDesc("", 0f)));
            #endregion

            #region Skills
            //Every Moment Matters
            passiveDuration = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Every Moment Matters: Buff Duration"),
                2f,
                new ConfigDescription(CreateOptionDesc("How long the movement speed buff gained from critical strikes lasts", 2f)));

            //Whisper
            primaryDamageCoefficient = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Whisper: Damage Coefficient"), 
                8f, 
                new ConfigDescription(CreateOptionDesc("", 8f)));

            primaryExecuteMissingHealthPercentage = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Whisper: Execute Damage Coefficient"), 
                0.3f, 
                new ConfigDescription(CreateOptionDesc("The percentage of missing health used by Whisper's Execute mechanic", 0.3f)));

            primaryExecuteDamageCap = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Whisper: Execute Damage Cap"), 
                1f, 
                new ConfigDescription(CreateOptionDesc("Percent of bonus damage, based on Whisper's damage, allowed to be dealt by Whisper's Execute mechanic", 1f)));

            //Dancing Grenade
            secondaryCD = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Dancing Grenade: Cooldown"), 
                6f, 
                new ConfigDescription(CreateOptionDesc("", 6f)));

            secondaryDamageCoefficient = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Dancing Grenade: Damage Coefficient"), 
                6f, 
                new ConfigDescription(CreateOptionDesc("", 6f)));

            secondaryDamageBounceCoefficient = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Dancing Grenade: Bounce Damage Coefficient"), 
                0.35f, 
                new ConfigDescription(CreateOptionDesc("Percent of Dancing Grenade's current damage to add when a bounce kills an enemy", 0.35f)));

            //Deadly Flourish
            utilityCD = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Deadly Flourish: Cooldown"), 
                4f, 
                new ConfigDescription(CreateOptionDesc("", 8f)));

            utilityDamageCoefficient = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Deadly Flourish: Damage Coefficient"), 
                5f, 
                new ConfigDescription(CreateOptionDesc("", 9f)));

            //CurtainCall
            specialCD = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Curtain Call: Cooldown"), 
                10f, 
                new ConfigDescription(CreateOptionDesc("", 10f)));

            specialDamageCoefficient = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Curtain Call: Damage Coefficient"), 
                16f, 
                new ConfigDescription(CreateOptionDesc("", 16f)));

            specialExecutePercentage = JhinPlugin.instance.Config.Bind<float>(
                new ConfigDefinition("Skills", "Curtain Call: Execute Damage Coefficient"), 
                3f, 
                new ConfigDescription(CreateOptionDesc("The percentage of damage to add for each percent of missing health used by Curtain Call's Execute mechanic", 3f)));
            #endregion

            //Other
            sfxChoice = JhinPlugin.instance.Config.Bind<SFXChoice>(
                new ConfigDefinition("Audio", "SFX Choice"),
                SFXChoice.SkinDependent,
                new ConfigDescription(CreateOptionDesc("If set, forces the mod to use SFX from a specific skin. Otherwise uses SFX from the player's respestive skin", SFXChoice.SkinDependent)));

            //Not Implemented
            /*
            tauntKeybind = JhinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("Not Implemented", "Taunt"), KeyCode.Alpha1, new ConfigDescription("Keybind used to perform the Taunt emote"));
            jokeKeybind = JhinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("Not Implemented", "Joke"), KeyCode.Alpha2, new ConfigDescription("Keybind used to perform the Joke emote"));
            laughKeybind = JhinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("Not Implemented", "Laugh"), KeyCode.Alpha3, new ConfigDescription("Keybind used to perform the Laugh emote"));
            danceKeybind = JhinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("Not Implemented", "Dance"), KeyCode.Alpha4, new ConfigDescription("Keybind used to perform the Dance emote"));

            voiceLines = JhinPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Not Implemented", "Voice Lines"), true, new ConfigDescription("Enable Voice Lines"));
            */
        }

        internal static String CreateOptionDesc(string desc, float defaultVal)
        {
            if (String.IsNullOrEmpty(desc))
                return $"Default: {defaultVal}";
            else
                return desc + Environment.NewLine + Environment.NewLine + $"Default: {defaultVal}";
        }
        internal static String CreateOptionDesc(string desc, SFXChoice defaultVal)
        {
            return desc + Environment.NewLine + Environment.NewLine + $"Default: {defaultVal}";
        }

        internal static void CreateRiskofOptionsCompat()
        {
            ModSettingsManager.SetModDescription("Jhin Survivor Mod Configuration. ANY CHANGES WILL REQUIRE A RESTART.");
            ModSettingsManager.SetModIcon(Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkinIcon"));

            CreateOptionEntry(healthBase);
            CreateOptionEntry(healthGrowth);
            CreateOptionEntry(regenBase);
            CreateOptionEntry(regenGrowth);
            CreateOptionEntry(armorBase);
            CreateOptionEntry(armorGrowth);
            CreateOptionEntry(damageBase);
            CreateOptionEntry(damageGrowth);
            CreateOptionEntry(attackSpeedBase);
            CreateOptionEntry(attackSpeedGrowth);

            CreateOptionEntry(passiveDuration);

            CreateOptionEntry(primaryDamageCoefficient);
            CreateOptionEntry(primaryExecuteMissingHealthPercentage);
            CreateOptionEntry(primaryExecuteDamageCap);

            CreateOptionEntry(secondaryCD);
            CreateOptionEntry(secondaryDamageCoefficient);
            CreateOptionEntry(secondaryDamageBounceCoefficient);

            CreateOptionEntry(utilityCD);
            CreateOptionEntry(utilityDamageCoefficient);

            CreateOptionEntry(specialCD);
            CreateOptionEntry(specialDamageCoefficient);
            CreateOptionEntry(specialExecutePercentage);


            CreateOptionEntry(sfxChoice);
        }
        internal static void CreateOptionEntry(ConfigEntryBase configEntry, bool autoScale = true, float max = 100, float increment = 10)
        {
            var maxOut = max;
            var incrementOut = increment;

            if (configEntry.DefaultValue.GetType() == typeof(SFXChoice))
            {
                ModSettingsManager.AddOption(new ChoiceOption(configEntry));
                return;
            }

            if (configEntry.DefaultValue.GetType() == typeof(bool))
            {
                ModSettingsManager.AddOption(new CheckBoxOption((ConfigEntry<bool>)configEntry));
                return;
            }

            if (configEntry.DefaultValue.GetType() == typeof(int))
            {
                if (autoScale && ((ConfigEntry<int>)configEntry).Value != 0)
                {
                    maxOut = ((ConfigEntry<int>)configEntry).Value * 10;
                }
                else
                {
                    maxOut = (int)Math.Floor(maxOut);
                }
                ModSettingsManager.AddOption(new IntSliderOption((ConfigEntry<int>)configEntry, new IntSliderConfig() { min = 0, max = (int)maxOut }));
                return;
            }

            if (configEntry.DefaultValue.GetType() == typeof(float))
            {
                if ( autoScale && ((ConfigEntry<float>)configEntry).Value != 0f )
                {
                    maxOut = ((ConfigEntry<float>)configEntry).Value * 10f;
                    incrementOut = ((ConfigEntry<float>)configEntry).Value / 10f;
                }
                ModSettingsManager.AddOption(new StepSliderOption((ConfigEntry<float>)configEntry, new StepSliderConfig() { min = 0, max = maxOut, increment = incrementOut }));
                return;
                //ModSettingsManager.AddOption(new StepSliderOption((ConfigEntry<float>)configEntry, new StepSliderConfig() { min = 0, max = ((ConfigEntry<float>)configEntry).Value * 10f, increment = ((ConfigEntry<float>)configEntry).Value / 10 }));
            }
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return JhinPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return JhinPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
        public enum SFXChoice
        {
            SkinDependent,
            Base,
            HighNoon,
            BloodMoon,
            Project
        }
    }
}