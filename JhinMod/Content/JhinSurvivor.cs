using BepInEx.Configuration;
using IL.RoR2.Orbs;
using JhinMod.Content.Components;
using JhinMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using EntityStates;
using System;
using System.Collections.Generic;
using UnityEngine;
using JhinMod.SkillStates.BaseStates;
using JhinMod.Modules.SkillDefs;

namespace JhinMod.Modules.Survivors
{
    internal class JhinSurvivor : SurvivorBase
    {
        //used when building your character using the prefabs you set up in unity
        //don't upload to thunderstore without changing this
        public override string prefabBodyName => "Jhin";

        public const string JHIN_PREFIX = JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => JHIN_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "JhinBody",
            bodyNameToken = JHIN_PREFIX + "NAME",
            subtitleNameToken = JHIN_PREFIX + "SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texJhinIcon"),
            bodyColor = new Color(1f, 0f, 0.44f),

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            //Stats
            maxHealth = Modules.Config.healthBase.Value,
            healthGrowth = Modules.Config.healthGrowth.Value,

            healthRegen = Modules.Config.regenBase.Value,
            regenGrowth = Modules.Config.regenGrowth.Value,

            armor = Modules.Config.armorBase.Value,
            armorGrowth = Modules.Config.armorGrowth.Value,

            damage = Modules.Config.damageBase.Value,
            damageGrowth = Modules.Config.damageGrowth.Value,

            attackSpeed = Modules.Config.attackSpeedBase.Value - Modules.Config.attackSpeedGrowth.Value,
            attackSpeedGrowth = Modules.Config.attackSpeedGrowth.Value,

            crit = Modules.Config.critBase.Value,
            critGrowth = Modules.Config.critGrowth.Value,

            moveSpeed = Modules.Config.movementSpeedBase.Value,
            moveSpeedGrowth = Modules.Config.movementSpeedGrowth.Value,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
                new CustomRendererInfo
                {
                    childName = "JhinMesh",
                    material = Materials.CreateHopooMaterial("matJhin"),
                }
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new JhinItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();

            this.bodyPrefab.AddComponent<JhinTracker>();
            this.bodyPrefab.AddComponent<JhinStateController>();
            var ultStateMachine = this.bodyPrefab.AddComponent<EntityStateMachine>();
            ultStateMachine.customName = "WeaponMode";
            ultStateMachine.mainStateType = new SerializableEntityStateType(typeof(JhinWeaponMainState));

            CharacterDeathBehavior characterDeathBehavior = bodyPrefab.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(JhinMod.SkillStates.BaseStates.AnimatedDeathState));
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = JhinPlugin.DEVELOPER_PREFIX;

            #region Passive
            SkillLocator skillloc = bodyPrefab.GetComponent<SkillLocator>();
            skillloc.passiveSkill.enabled = true;
            skillloc.passiveSkill.skillNameToken = JHIN_PREFIX + "PASSIVE_NAME";
            skillloc.passiveSkill.skillDescriptionToken = JHIN_PREFIX + "PASSIVE_DESCRIPTION";
            skillloc.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon");
            #endregion

            #region Primary
            //Creates a skilldef for a typical primary 
            /*
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef<JhinAmmoSkillDef>(new SkillDefInfo(JHIN_PREFIX + "PRIMARY_WHISPER_NAME",
                                                                                      JHIN_PREFIX + "PRIMARY_WHISPER_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.WhisperPrimary)),
                                                                                      "Weapon",
                                                                                      true));
            */

            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef<JhinAmmoSkillDef>(new SkillDefInfo
            {
                skillName = JHIN_PREFIX + "PRIMARY_WHISPER_NAME",
                skillNameToken = JHIN_PREFIX + "PRIMARY_WHISPER_NAME",
                skillDescriptionToken = JHIN_PREFIX + "PRIMARY_WHISPER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.WhisperPrimary)),
                activationStateMachineName = "Weapon",
                
                interruptPriority = EntityStates.InterruptPriority.Any,
                isCombatSkill = true,
                baseRechargeInterval = 0,

                requiredStock = 0,
                stockToConsume = 0,

                cancelSprintingOnActivation = false,
                
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_EXECUTING_WHISPER", "KEYWORD_RELOAD" }
            });

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region Secondary
            SkillDef secondarySkillDef = Modules.Skills.CreateSkillDef<JhinTrackingSkillDef>(new SkillDefInfo
            {
                skillName = JHIN_PREFIX + "SECONDARY_GRENADE_NAME",
                skillNameToken = JHIN_PREFIX + "SECONDARY_GRENADE_NAME",
                skillDescriptionToken = JHIN_PREFIX + "SECONDARY_GRENADE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.DancingGrenade)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Config.secondaryCD.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef);
            #endregion

            #region Utility
            SkillDef utilitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = JHIN_PREFIX + "UTILITY_FLOURISH_NAME",
                skillNameToken = JHIN_PREFIX + "UTILITY_FLOURISH_NAME",
                skillDescriptionToken = JHIN_PREFIX + "UTILITY_FLOURISH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.DeadlyFlourish)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = Config.utilityCD.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING" }
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef);
            #endregion

            #region Special
            SkillDef specialSkillDef = Modules.Skills.CreateSkillDef<JhinUltSkillDef>(new SkillDefInfo
            {
                skillName = JHIN_PREFIX + "SPECIAL_ULT_NAME",
                skillNameToken = JHIN_PREFIX + "SPECIAL_ULT_NAME",
                skillDescriptionToken = JHIN_PREFIX + "SPECIAL_ULT_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CurtainCall)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Config.specialCD.Value,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Vehicle,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_EXECUTING_SPECIAL", "KEYWORD_RELOAD" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, specialSkillDef);
            #endregion

            #region Non-selectable

            #endregion
        }

        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(JHIN_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkinIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
            //    "JhinMesh");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            #region High Noon

            //creating a new skindef as we did before
            SkinDef highnoonSkin = Modules.Skins.CreateSkinDef(JHIN_PREFIX + "HIGHNOON_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texHighNoonSkinIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            highnoonSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "jhinMeshHighNoon");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            highnoonSkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matJhinHighNoon");

            skins.Add(highnoonSkin);

            #endregion

            #region Blood Moon

            //creating a new skindef as we did before
            SkinDef bloodmoonSkin = Modules.Skins.CreateSkinDef(JHIN_PREFIX + "BLOODMOON_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texBloodMoonSkinIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            bloodmoonSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "jhinMeshBloodMoon");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            bloodmoonSkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matJhinBloodMoon");

            skins.Add(bloodmoonSkin);

            #endregion

            #region PROJECT

            //creating a new skindef as we did before
            SkinDef projectSkin = Modules.Skins.CreateSkinDef(JHIN_PREFIX + "PROJECT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texProjectSkinIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            projectSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "jhinMeshProject");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            projectSkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matJhinProject");

            skins.Add(projectSkin);
            
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texProjectSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshJhinSwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshJhinAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matJhinAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matJhinAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matJhinAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}