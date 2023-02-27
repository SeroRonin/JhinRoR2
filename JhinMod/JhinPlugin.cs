using BepInEx;
using JhinMod.Modules.Survivors;
using JhinMod.SkillStates;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Networking;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace JhinMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class JhinPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.seroronin.JhinMod";
        public const string MODNAME = "JhinMod";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "SERORONIN";

        public static JhinPlugin instance;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new JhinSurvivor().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (damageInfo.crit && damageInfo.attacker && !damageInfo.rejected)
            {
                CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                if (attackerBody && attackerBody.baseNameToken == JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_NAME" )
                {
                    if (NetworkServer.active)
                    {
                        attackerBody.AddTimedBuff(Modules.Buffs.jhinCritMovespeedBuff, 2f);
                    }
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            
            if (self)
            {
                if (self.baseNameToken == JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_NAME")
                {
                    var premodDam = self.damage;
                    var premodAtkSpd = self.attackSpeed;
                    var premodMovespeed = self.moveSpeed;

                    var atkSpdLocked = self.baseAttackSpeed + (self.levelAttackSpeed * self.level);
                    var atkSpdBonus = Mathf.Max(premodAtkSpd - atkSpdLocked, 0);
                    var damBonus = CalculateDamageBonus(premodDam, atkSpdBonus, atkSpdLocked);

                    /*
                    ChatMessage.Send($"Damage Premod: {premodDam}");
                    ChatMessage.Send($"Damage Bonus: {damBonus}");
                    ChatMessage.Send($"AS Total: {premodAtkSpd}");
                    ChatMessage.Send($"AS Locked: {atkSpdLocked}");
                    ChatMessage.Send($"AS Bonus: {atkSpdBonus}");
                    */

                    //Apply bonus damage
                    self.damage += damBonus;

                    //Lock our attackspeed
                    self.attackSpeed = atkSpdLocked; 
                    
                    if (self.HasBuff(Modules.Buffs.jhinCritMovespeedBuff))
                    {
                        var movespeedBonus = CalculateMovespeedBonus(premodMovespeed, atkSpdBonus, atkSpdLocked);
                        self.moveSpeed += movespeedBonus;
                    }
                }
                
            }
            
        }
        public float CalculateDamageBonus( float damage, float attakSpeedBonus, float attackSpeedLocked )
        {
            var percentDam = 0.0025f ; //What percentage, as a decimal, of damage do we want to add per percent of bonus attackspeed?
            var percentAtkSpd = 100 * (attakSpeedBonus / attackSpeedLocked) ; //Convert bonus attack speed from Multiplier to Percent
            var damageBonus = damage * ( percentDam * percentAtkSpd);

            return damageBonus;
        }
        public float CalculateMovespeedBonus(float movespeed, float attakSpeedBonus, float attackSpeedLocked)
        {
            var percentMove = 0.004f; //What percentage, as a decimal, of damage do we want to add per percent of bonus attackspeed?
            var percentAtkSpd = 100 * (attakSpeedBonus / attackSpeedLocked); //Convert bonus attack speed from Multiplier to Percent
            var movespeedBonus = movespeed * (0.1f + (percentMove * percentAtkSpd) );

            return movespeedBonus;
        }
    }
}