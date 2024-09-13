using BepInEx;
using JhinMod.Content.UI;
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
using JhinMod.Content.Components;
using JhinMod.Modules;
using EntityStates;
using EmotesAPI;
using HG;


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace JhinMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
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
        public const string MODUID = "com.seroronin.JhinMod";
        public const string MODNAME = "JhinMod";
        public const string MODVERSION = "1.4.1";

        public const string DEVELOPER_PREFIX = "SERORONIN";

        public static JhinPlugin instance;
        public bool emoteSetup;

        public bool CustomEmotesActive;

        private void Awake()
        {
            instance = this;

            //Multiplayer testing, do not release with this
            //On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };

            Log.Init(Logger);
            Modules.Asset.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                Modules.Config.CreateRiskofOptionsCompat();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                Log.Warning("EMOTES ACTIVE");
                CustomEmotesActive = true;
            }
            else
            {
                Log.Warning("EMOTES INACTIVE");
            }

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

        public void OnDestroy()
        {
            try
            {
                UnHooks();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message + " - " + e.StackTrace);
            }
        }

        private void Hook()
        {
            On.RoR2.SurvivorMannequins.SurvivorMannequinSlotController.ApplyLoadoutToMannequinInstance += SurvivorMannequinSlotController_ApplyLoadoutToMannequinInstance;

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_OnTakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.UI.HUD.Awake += HUD_Awake;
            RoR2.UI.HUD.onHudTargetChangedGlobal += HUD_onHudTargetChangedGlobal;

            //Hooks of Heresy
            On.EntityStates.Mage.Weapon.BaseChargeBombState.OnEnter += SlicingMaelstrom_Charge_OnEnter;
            On.EntityStates.Mage.Weapon.BaseChargeBombState.OnExit += SlicingMaelstrom_Charge_OnExit;
            //Strides of Heresy
            On.EntityStates.GhostUtilitySkillState.OnEnter += Shadowfade_OnEnter;
            On.EntityStates.GhostUtilitySkillState.OnExit += Shadowfade_OnExit;
            //Essence of Heresy
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += Ruin_OnEnter;

            On.EntityStates.FrozenState.OnEnter += FrozenState_OnEnter;
            On.EntityStates.FrozenState.OnExit += FrozenState_OnExit;

            if ( CustomEmotesActive )
            {
                Content.CustomEmotesAPISupport.HookCustomEmoteAPI();
                CustomEmotesAPI.CreateNameTokenSpritePair("SERORONIN_JHIN_BODY_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texCustomEmotes_JhinIcon"));
            }

            //MP testings, disable when not testing on local machine
            //On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };
        }

        private void UnHooks()
        {
            On.RoR2.SurvivorMannequins.SurvivorMannequinSlotController.ApplyLoadoutToMannequinInstance -= SurvivorMannequinSlotController_ApplyLoadoutToMannequinInstance;

            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.UI.HUD.Awake -= HUD_Awake;
            RoR2.UI.HUD.onHudTargetChangedGlobal -= HUD_onHudTargetChangedGlobal;
            
            //Hooks of Heresy
            On.EntityStates.Mage.Weapon.BaseChargeBombState.OnEnter -= SlicingMaelstrom_Charge_OnEnter;
            On.EntityStates.Mage.Weapon.BaseChargeBombState.OnExit -= SlicingMaelstrom_Charge_OnExit;
            //Strides of Heresy
            On.EntityStates.GhostUtilitySkillState.OnEnter -= Shadowfade_OnEnter;
            On.EntityStates.GhostUtilitySkillState.OnExit -= Shadowfade_OnExit;
            //Essence of Heresy
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter -= Ruin_OnEnter;

            On.EntityStates.FrozenState.OnEnter -= FrozenState_OnEnter;
            On.EntityStates.FrozenState.OnExit -= FrozenState_OnExit;


            if ( CustomEmotesActive )
            {
                Content.CustomEmotesAPISupport.UnhookCustomEmoteAPI();
            }
        }

        public static Dictionary<Transform, GameObject> playerLobbyModelFX = new Dictionary<Transform, GameObject>();

        //Creates Dictionary entries for each lobby display, and adds model FX for those if needed
        private void SurvivorMannequinSlotController_ApplyLoadoutToMannequinInstance(On.RoR2.SurvivorMannequins.SurvivorMannequinSlotController.orig_ApplyLoadoutToMannequinInstance orig, RoR2.SurvivorMannequins.SurvivorMannequinSlotController self)
        {
            orig(self);

            if (!self.mannequinInstanceTransform)
            {
                return;
            }

            var displayPrefab = self.mannequinInstanceTransform;
            BodyIndex bodyIndexFromSurvivorIndex = SurvivorCatalog.GetBodyIndexFromSurvivorIndex(self.currentSurvivorDef.survivorIndex);
            int skinIndex = (int)self.currentLoadout.bodyLoadoutManager.GetSkinIndex(bodyIndexFromSurvivorIndex);

            if (displayPrefab.gameObject.name.Contains("JhinDisplay"))
            {
                if ( JhinPlugin.playerLobbyModelFX.ContainsKey( self.mannequinInstanceTransform ) )
                {
                    if ( JhinPlugin.playerLobbyModelFX[self.mannequinInstanceTransform] != null )
                    {
                        GameObject.Destroy(JhinPlugin.playerLobbyModelFX[self.mannequinInstanceTransform]);
                        JhinPlugin.playerLobbyModelFX.Remove( self.mannequinInstanceTransform );
                    }
                }
                if ( !JhinPlugin.playerLobbyModelFX.ContainsKey( self.mannequinInstanceTransform ) )
                {
                    var modelFXprefab = Helpers.GetVFXDynamic("ModelFX", skinIndex);
                    if (modelFXprefab != null)
                    {
                        var modelFX = UnityEngine.Object.Instantiate<GameObject>(modelFXprefab, self.mannequinInstanceTransform);
                        var bindPairComp = modelFX.GetComponent<BindPairLocator>();
                        bindPairComp.target = self.mannequinInstanceTransform.gameObject;
                        bindPairComp.BindPairs();
                        JhinPlugin.playerLobbyModelFX[self.mannequinInstanceTransform] = modelFX;
                    }
                }
            }
        }

        //Handles passive speed boost on crit
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            //Did we succesfully crit?
            if (damageInfo.crit && damageInfo.attacker && !damageInfo.rejected)
            {
                CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();

                //If we are the right survivor, add the movespeed buff
                if (attackerBody && attackerBody.baseNameToken == JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_NAME" )
                {
                    if ( NetworkServer.active )
                    {
                        attackerBody.AddTimedBuff(Modules.Buffs.jhinCritMovespeedBuff, Modules.Config.passiveBuffDuration.Value);
                    }
                }
            }
        }

        //Handles Deadly Flourish marks
        private void HealthComponent_OnTakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if ( info.HasModdedDamageType(Modules.Buffs.JhinMarkDamage) )
            {
                if (NetworkServer.active)
                {
                    self.body.AddTimedBuff(Modules.Buffs.jhinMarkDebuff, Modules.Config.utilityMarkDuration.Value);
                }
            }
            if (self.body.HasBuff(Modules.Buffs.jhinMarkDebuff) && info.HasModdedDamageType(Modules.Buffs.JhinConsumeMarkDamage) )
            {
                if (NetworkServer.active)
                {
                    self.body.ClearTimedBuffs(Modules.Buffs.jhinMarkDebuff);
                    self.body.AddTimedBuff(RoR2Content.Buffs.Nullified, Modules.Config.utilityRootDuration.Value);
                }
            }
            orig(self, info);
        }

        //Handles attackspeed to attack damage percent bonus
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                //If we are the right survivor, recalculate our stats
                if (self.baseNameToken == JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_NAME")
                {
                    var premodDam = self.damage;
                    var premodAtkSpd = self.attackSpeed;
                    var premodMovespeed = self.moveSpeed;

                    var atkSpdLocked = self.baseAttackSpeed + (self.levelAttackSpeed * self.level);
                    var atkSpdBonus = Mathf.Max(premodAtkSpd - atkSpdLocked, 0);
                    var damBonus = CalculateDamageBonus(premodDam, atkSpdBonus, atkSpdLocked);

                    var jhinStateController = self.GetComponent<JhinStateController>();
                    if (jhinStateController)
                    {
                        jhinStateController.preModAtkSpeed = premodAtkSpd;
                    }

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

        /// <summary>
        /// Calculates additional base damage based on attack speed bonus
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attakSpeedBonus"></param>
        /// <param name="attackSpeedLocked"></param>
        /// <returns></returns>
        public float CalculateDamageBonus(float damage, float attakSpeedBonus, float attackSpeedLocked)
        {
            var percentDam = Modules.Config.passiveDamageConversion.Value;
            var percentAtkSpd = (attakSpeedBonus / attackSpeedLocked);
            var damageBonus = damage * (percentDam * percentAtkSpd);

            return damageBonus;
        }

        /// <summary>
        /// Calculates additional movespeed granted by critical hit buff
        /// </summary>
        /// <param name="movespeed"></param>
        /// <param name="attakSpeedBonus"></param>
        /// <param name="attackSpeedLocked"></param>
        /// <returns></returns>
        public float CalculateMovespeedBonus(float movespeed, float attakSpeedBonus, float attackSpeedLocked)
        {
            var percentMove = Modules.Config.passiveMovespeedConversion.Value;
            var percentAtkSpd = (attakSpeedBonus / attackSpeedLocked);
            var movespeedBonus = movespeed * (0.1f + (percentMove * percentAtkSpd)); //Add a flat +10% movement speed, in case we don't have any attackspeed

            return movespeedBonus;
        }

        #region Heresy Skill Patches
        //Here we add hooks that patch some logic into Heresy Skill Overrides, so that they function properly with the custom ammo logic
        private void SlicingMaelstrom_Charge_OnEnter(On.EntityStates.Mage.Weapon.BaseChargeBombState.orig_OnEnter orig, EntityStates.Mage.Weapon.BaseChargeBombState self)
        {
            orig(self);

            if (self is EntityStates.GlobalSkills.LunarNeedle.ChargeLunarSecondary)
            {
                var ammoComponent = self.GetComponent<JhinStateController>();
                if ( ammoComponent )
                {
                    ammoComponent.PauseReload();
                }
            }
        }
        private void SlicingMaelstrom_Charge_OnExit(On.EntityStates.Mage.Weapon.BaseChargeBombState.orig_OnExit orig, EntityStates.Mage.Weapon.BaseChargeBombState self)
        {
            orig(self);

            if (self is EntityStates.GlobalSkills.LunarNeedle.ChargeLunarSecondary)
            {
                var ammoComponent = self.GetComponent<JhinStateController>();
                if ( ammoComponent )
                {
                    ammoComponent.StopReload();
                }
            }
        }

        private void Shadowfade_OnEnter(On.EntityStates.GhostUtilitySkillState.orig_OnEnter orig, EntityStates.GhostUtilitySkillState self)
        {
            var ammoComponent = self.GetComponent<JhinStateController>();
            if ( ammoComponent != null )
            {
                if ( ammoComponent.ammoCount != 0 )
                {
                    ammoComponent.PauseReload();
                }

                var modelFx = ammoComponent.modelFX;
                if ( modelFx )
                {
                    modelFx.SetActive( false );
                }
                var ultFX = ammoComponent.ultFX;
                if ( ultFX )
                {
                    ultFX.SetActive( false );
                }
            }
            orig(self);
        }
        private void Shadowfade_OnExit(On.EntityStates.GhostUtilitySkillState.orig_OnExit orig, EntityStates.GhostUtilitySkillState self)
        {
            var ammoComponent = self.GetComponent<JhinStateController>();
            if ( ammoComponent != null )
            {
                if ( ammoComponent.ammoCount != 0)
                {
                    ammoComponent.StopReload();
                }

                var modelFx = ammoComponent.modelFX;
                if ( modelFx )
                {
                    modelFx.SetActive( true );
                }
                var ultFX = ammoComponent.ultFX;
                if ( ultFX )
                {
                    ultFX.SetActive( true );
                }
            }
            orig(self);
        }

        private void Ruin_OnEnter(On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self)
        {
            var ammoComponent = self.GetComponent<JhinStateController>();
            if ( ammoComponent != null && ammoComponent.ammoCount != 0 )
            {
                ammoComponent.StopReload();
            }
            orig(self);
        }
        #endregion

        public void FrozenState_OnEnter(On.EntityStates.FrozenState.orig_OnEnter orig, EntityStates.FrozenState self )
        {
            orig(self);

            var ammoComponent = self.GetComponent<JhinStateController>();
            if ( ammoComponent )
            {
                ammoComponent.PauseReload();
            }
        }
        public void FrozenState_OnExit(On.EntityStates.FrozenState.orig_OnExit orig, EntityStates.FrozenState self)
        {
            var ammoComponent = self.GetComponent<JhinStateController>();
            if ( ammoComponent )
            {
                ammoComponent.StopReload();
            }

            orig(self);
        }

        #region UI
        private JhinAmmoUI ammoUI;

        private void CreateAmmoUI(RoR2.UI.HUD hud)
        {
            if (!ammoUI)
            {
                if (hud != null && hud.mainUIPanel != null)
                {
                    ammoUI = hud.mainUIPanel.GetComponentInChildren<JhinAmmoUI>();
                    if (!ammoUI)
                    {
                        var ammoUIinstance = Instantiate(Modules.Asset.mainAssetBundle.LoadAsset<GameObject>("JhinAmmoUI"));
                        ammoUI = ammoUIinstance.AddComponent<JhinAmmoUI>();
                        ammoUIinstance.transform.SetParent(hud.mainUIPanel.transform);

                        var rectTransform = ammoUIinstance.GetComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                        rectTransform.pivot = new Vector2(0.5f, 0.5f);
                        rectTransform.sizeDelta = new Vector2(1, 1);
                        rectTransform.anchoredPosition = new Vector2(530, -355);
                        rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                        rectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                        ammoUIinstance.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            CreateAmmoUI(self);
            orig(self);
        }

        private void HUD_onHudTargetChangedGlobal(RoR2.UI.HUD obj)
        {
            if (obj && obj.targetBodyObject && ammoUI)
            {
                var ammoComponent = obj.targetBodyObject.GetComponent<JhinStateController>();
                var skillLocator = obj.targetBodyObject.GetComponent<SkillLocator>();
                var ultActiveStateMachine = Helpers.GetEntityStateMachine(obj.targetBodyObject, "WeaponMode");
                if (ammoComponent)
                {
                    ammoUI.gameObject.SetActive(true);
                    ammoUI.ammoComponent = ammoComponent;
                    ammoUI.skillLocator = skillLocator;
                    ammoUI.ultActiveStateMachine = ultActiveStateMachine;
                }
                else
                {
                    ammoUI.gameObject.SetActive(false);
                    ammoUI.ammoComponent = null;
                    ammoUI.skillLocator = null;
                    ammoUI.ultActiveStateMachine = null;
                }
            }
        }

        #endregion
    }
}