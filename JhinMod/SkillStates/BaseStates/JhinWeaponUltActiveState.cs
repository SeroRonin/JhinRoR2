using System;
using RoR2;
using RoR2.Skills;
using EntityStates;
using JhinMod.Content.Components;
using UnityEngine.Networking;
using UnityEngine;
using JhinMod.Modules;
using R2API.Utils;

namespace JhinMod.SkillStates.BaseStates
{
    public class JhinWeaponUltActiveState : BaseState
    {

        [SerializeField]
        public SkillDef primaryOverrideSkillDef;
        public SkillDef primaryOverrideCritSkillDef;
        protected JhinStateController jhinStateController;
        protected Animator animatorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.jhinStateController = base.GetComponent<JhinStateController>();
            this.animatorComponent = base.GetModelAnimator();

            string prefix = JhinPlugin.DEVELOPER_PREFIX;
            primaryOverrideSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_JHIN_BODY_SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "_JHIN_BODY_SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "_JHIN_BODY_SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialShotIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(CurtainCallShoot)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 4,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1
            });
            primaryOverrideCritSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_JHIN_BODY_SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "_JHIN_BODY_SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "_JHIN_BODY_SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialShotCritIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(CurtainCallShootCrit)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1
            });

            if (animatorComponent)
            {
                var layerIndex = animatorComponent.GetLayerIndex("UpperBody Ult, Override");
                animatorComponent.SetLayerWeight(layerIndex, 1f);
            }

            base.PlayAnimation("UpperBody, Override", "CurtainCallStart");

            SetOverride(primaryOverrideSkillDef);
        }
        public override void OnExit()
        {
            if (animatorComponent)
            {
                var layerIndex = this.animatorComponent.GetLayerIndex("UpperBody Ult, Override");
                animatorComponent.SetLayerWeight(layerIndex, 0f);
            }

            base.PlayAnimation("UpperBody, Override", "CurtainCallEnd");

            if (base.isAuthority && base.skillLocator)
            {
                base.skillLocator.primary.UnsetSkillOverride(this, this.primaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                base.skillLocator.primary.UnsetSkillOverride(this, this.primaryOverrideCritSkillDef, GenericSkill.SkillOverridePriority.Replacement);
            }

            base.OnExit();
        }
        public void SetOverride( SkillDef skillDef)
        {
            if (base.isAuthority && base.skillLocator)
            {
                base.skillLocator.primary.SetSkillOverride(this, skillDef, GenericSkill.SkillOverridePriority.Replacement);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!jhinStateController.ultHasSetLastShot && base.skillLocator.primary.stock == 1)
            {
                jhinStateController.ultHasSetLastShot = true;
                SetOverride( primaryOverrideCritSkillDef );
            }

            if (jhinStateController.ultHasSetLastShot && jhinStateController.ultHasFiredLastShot)
            {
                jhinStateController.isUlting = false;
            }
        }

    }
}