﻿using System;
using RoR2;
using RoR2.Skills;
using EntityStates;
using JhinMod.Content.Controllers;
using UnityEngine.Networking;
using UnityEngine;
using JhinMod.Modules;
using R2API.Utils;

namespace JhinMod.SkillStates.BaseStates
{
    public class JhinPassiveCritReadyState : BaseState
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
            primaryOverrideSkillDef = Modules.Skills.CreateSkillDef<JhinAmmoSkillDef>(new SkillDefInfo(prefix + "PRIMARY_WHISPER_NAME",
                                                                                      prefix + "PRIMARY_WHISPER_DESCRIPTION",
                                                                                      Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryCritIcon"),
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.WhisperPrimary)),
                                                                                      "Weapon",
                                                                                      true));

            Util.PlaySound("JhinPassiveCritSpin", base.gameObject);
            Util.PlaySound("JhinPassiveCritMusic", base.gameObject);

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
            Util.PlaySound("JhinStopPassiveCritSpin", base.gameObject);
            Util.PlaySound("JhinStopPassiveCritMusic", base.gameObject);

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