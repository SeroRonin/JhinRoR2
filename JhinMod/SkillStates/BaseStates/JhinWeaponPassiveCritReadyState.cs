using System;
using RoR2;
using RoR2.Skills;
using EntityStates;
using JhinMod.Content.Components;
using UnityEngine.Networking;
using UnityEngine;
using JhinMod.Modules;
using R2API.Utils;
using JhinMod.Modules.SkillDefs;

namespace JhinMod.SkillStates.BaseStates
{
    public class JhinWeaponPassiveCritReadyState : BaseState
    {

        [SerializeField]
        public SkillDef primaryOverrideSkillDef;
        protected JhinStateController jhinStateController;
        protected Animator animatorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.jhinStateController = base.GetComponent<JhinStateController>();
            this.animatorComponent = base.GetModelAnimator();

            string prefix = JhinPlugin.DEVELOPER_PREFIX + "_JHIN_BODY_";
            /*
            primaryOverrideSkillDef = Modules.Skills.CreateSkillDef<JhinAmmoSkillDef>(new SkillDefInfo(prefix + "PRIMARY_WHISPER_NAME",
                                                                                      prefix + "PRIMARY_WHISPER_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryCritIcon"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.WhisperPrimary)),
                                                                                      "Weapon",
                                                                                      true));
            */
            primaryOverrideSkillDef = Modules.Skills.CreateSkillDef<JhinAmmoSkillDef>(new SkillDefInfo
            {
                skillName = prefix + "PRIMARY_WHISPER_NAME",
                skillNameToken = prefix + "PRIMARY_WHISPER_NAME",
                skillDescriptionToken = prefix + "PRIMARY_WHISPER_CRIT_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryCritIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.WhisperPrimary)),
                activationStateMachineName = "Weapon",

                interruptPriority = EntityStates.InterruptPriority.Any,
                isCombatSkill = true,
                baseRechargeInterval = 0,

                requiredStock = 0,
                stockToConsume = 0,

                cancelSprintingOnActivation = false,

                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_EXECUTING" }
            });

            Util.PlaySound("Play_Seroronin_Jhin_PassiveCritSpin", base.gameObject);
            Util.PlaySound("Play_Seroronin_Jhin_PassiveCritMusic", base.gameObject);

            if (animatorComponent)
            {
                var layerIndex = animatorComponent.GetLayerIndex("UpperBody Idle, Override");
                animatorComponent.SetLayerWeight(layerIndex, 1f);
            }


            if (base.isAuthority && base.skillLocator)
            {
                base.skillLocator.primary.SetSkillOverride(this, primaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            }
        }
        public override void OnExit()
        {
            Util.PlaySound("Stop_Seroronin_Jhin_PassiveCritSpin", base.gameObject);
            Util.PlaySound("Stop_Seroronin_Jhin_PassiveCritMusic", base.gameObject);

            if (animatorComponent)
            {
                var layerIndex = this.animatorComponent.GetLayerIndex("UpperBody Idle, Override");
                animatorComponent.SetLayerWeight(layerIndex, 0f);
            }


            if (base.isAuthority && base.skillLocator)
            {
                base.skillLocator.primary.UnsetSkillOverride(this, this.primaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            /*
            if (base.isAuthority && this.voidSurvivorController && this.voidSurvivorController.corruption <= this.voidSurvivorController.minimumCorruption && !this.voidSurvivorController.isPermanentlyCorrupted && this.voidSurvivorController.bodyStateMachine)
            {
                this.voidSurvivorController.bodyStateMachine.SetInterruptState(new ExitCorruptionTransition(), InterruptPriority.Skill);
            }*/
        }

    }
}