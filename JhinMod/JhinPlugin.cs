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
using JhinMod.Content.UI;
using JhinMod.Content.Components;
using JhinMod.Modules;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace JhinMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
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
        public const string MODVERSION = "1.0.0";

        public const string DEVELOPER_PREFIX = "SERORONIN";

        public static JhinPlugin instance;
        public bool RoR2OptionsEnabled;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions")) RoR2OptionsEnabled = true;
            if (RoR2OptionsEnabled)
            {
                Modules.Config.CreateRiskofOptionsCompat();
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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.UI.HUD.Awake += HUD_Awake;
            RoR2.UI.HUD.onHudTargetChangedGlobal += HUD_onHudTargetChangedGlobal;
        }

        /*
        private void GlobalEventManager_OnHitAll(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo damageInfo, GameObject hitObject)
        {
            orig(self, damageInfo, hitObject);

            bool active = NetworkServer.active;
            if (damageInfo.attacker)
            {
                CharacterBody component = damageInfo.attacker.GetComponent<CharacterBody>();
                if (component)
                {
                    CharacterMaster master = component.master;
                    if (master)
                    {
                        Inventory inventory = master.inventory;
                        if (master.inventory)
                        {
                            if (!damageInfo.procChainMask.HasProc(ProcType.Behemoth))
                            {
                                int itemCount = inventory.GetItemCount(RoR2Content.Items.Behemoth);
                                if (itemCount > 0 && damageInfo.procCoefficient != 0f)
                                {
                                    float num = (1.5f + 2.5f * (float)itemCount) * damageInfo.procCoefficient;
                                    float damageCoefficient = 0.6f;
                                    float baseDamage = Util.OnHitProcDamage(damageInfo.damage, component.damage, damageCoefficient);
                                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                                    {
                                        origin = damageInfo.position,
                                        scale = num,
                                        rotation = Util.QuaternionSafeLookRotation(damageInfo.force)
                                    }, true);
                                    BlastAttack blastAttack = new BlastAttack();
                                    blastAttack.position = damageInfo.position;
                                    blastAttack.baseDamage = baseDamage;
                                    blastAttack.baseForce = 0f;
                                    blastAttack.radius = num;
                                    blastAttack.attacker = damageInfo.attacker;
                                    blastAttack.inflictor = null;
                                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                                    blastAttack.crit = damageInfo.crit;
                                    blastAttack.procChainMask = damageInfo.procChainMask;
                                    blastAttack.procCoefficient = 0f;
                                    blastAttack.damageColorIndex = DamageColorIndex.Item;
                                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                                    blastAttack.damageType = damageInfo.damageType;
                                    blastAttack.Fire();
                                }
                            }
                            if ((component.HasBuff(RoR2Content.Buffs.AffixBlue) ? 1 : 0) > 0)
                            {
                                float damageCoefficient2 = 0.5f;
                                float damage = Util.OnHitProcDamage(damageInfo.damage, component.damage, damageCoefficient2);
                                float force = 0f;
                                Vector3 position = damageInfo.position;
                                ProjectileManager.instance.FireProjectile(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LightningStake"), position, Quaternion.identity, damageInfo.attacker, damage, force, damageInfo.crit, DamageColorIndex.Item, null, -1f);
                            }
                        }
                    }
                }
            }
        }*/

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
                    if (NetworkServer.active)
                    {
                        attackerBody.AddTimedBuff(Modules.Buffs.jhinCritMovespeedBuff, Modules.Config.passiveDuration.Value);
                    }
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                //If we are the right survivor, recalculate our states
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

        /// <summary>
        /// Calculates additional base damage based on attack speed bonus
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attakSpeedBonus"></param>
        /// <param name="attackSpeedLocked"></param>
        /// <returns></returns>
        public float CalculateDamageBonus(float damage, float attakSpeedBonus, float attackSpeedLocked)
        {
            var percentDam = 0.0025f; //What percentage, as a decimal, of damage do we want to add per percent of bonus attackspeed?
            var percentAtkSpd = 100 * (attakSpeedBonus / attackSpeedLocked); //Convert bonus attack speed from Multiplier to Percent
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
            var percentMove = 0.004f; //What percentage, as a decimal, of damage do we want to add per percent of bonus attackspeed?
            var percentAtkSpd = 100 * (attakSpeedBonus / attackSpeedLocked); //Convert bonus attack speed from Multiplier to Percent
            var movespeedBonus = movespeed * (0.1f + (percentMove * percentAtkSpd));

            return movespeedBonus;
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
                        var ammoUIinstance = Instantiate(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("JhinAmmoUI"));
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
                if (ammoComponent)
                {
                    ammoUI.gameObject.SetActive(true);
                    ammoUI.ammoComponent = ammoComponent;
                    ammoUI.skillLocator = skillLocator;
                }
                else
                {
                    ammoUI.gameObject.SetActive(false);
                    ammoUI.ammoComponent = null;
                    ammoUI.skillLocator = null;
                }
            }
        }

        #endregion

        private void UnHooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.UI.HUD.Awake -= HUD_Awake;
            RoR2.UI.HUD.onHudTargetChangedGlobal -= HUD_onHudTargetChangedGlobal;
        }
    }
}