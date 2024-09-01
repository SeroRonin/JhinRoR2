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
        public float duration = 10f;

        [SerializeField]
        public SkillDef primaryOverrideSkillDef;
        public SkillDef primaryOverrideCritSkillDef;
        public SkillDef specialCancelSkillDef;
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
                skillName = prefix + "_JHIN_BODY_SPECIAL_ULT_NAME",
                skillNameToken = prefix + "_JHIN_BODY_SPECIAL_ULT_NAME",
                skillDescriptionToken = prefix + "_JHIN_BODY_SPECIAL_ULT_SHOT_DESCRIPTION",
                skillIcon = Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texSpecialShotIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(CurtainCallShoot)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = Config.specialCD.Value,
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
                requiredStock = 0,
                stockToConsume = 0
            });
            primaryOverrideCritSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_JHIN_BODY_SPECIAL_ULT_NAME",
                skillNameToken = prefix + "_JHIN_BODY_SPECIAL_ULT_NAME",
                skillDescriptionToken = prefix + "_JHIN_BODY_SPECIAL_ULT_SHOT_CRIT_DESCRIPTION",
                skillIcon = Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texSpecialShotCritIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(CurtainCallShootCrit)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = Config.specialCD.Value,
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

            specialCancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_JHIN_BODY_SPECIAL_ULT_NAME",
                skillNameToken = prefix + "_JHIN_BODY_SPECIAL_ULT_NAME",
                skillDescriptionToken = prefix + "_JHIN_BODY_SPECIAL_ULT_CANCEL_DESCRIPTION",
                skillIcon = Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texSpecialCancelIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(CurtainCallCancel)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = Config.specialCD.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
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

            SetOverride(primaryOverrideSkillDef, "primary" );

            var sm = Helpers.GetEntityStateMachine(this.gameObject, "Slide");
            sm.SetNextStateToMain();
            SetOverride(specialCancelSkillDef, "special");
        }

        public override void OnExit()
        {
            var ammoRemaining = jhinStateController.ammoCount;
            jhinStateController.isUlting = false;
            jhinStateController.ResetUlt();

            Helpers.StopSoundDynamic("UltMusic", base.gameObject);

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
                base.skillLocator.special.UnsetSkillOverride(this, this.specialCancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);

                var slideSM = Helpers.GetEntityStateMachine(this.gameObject, "Slide");
                slideSM.SetNextStateToMain();
                var specialSkill = base.skillLocator.special;
                // TODO: refund up to 50% of ult cooldown based on unspent shots
                specialSkill.stock -= 1;
            }

            base.OnExit();
        }

        public void SetOverride( SkillDef skillDef, string skillSlot )
        {
            string skillSlotComp = skillSlot.ToLower();

            if (base.isAuthority && base.skillLocator)
            {
                switch (skillSlotComp)
                {
                    case "primary":
                        base.skillLocator.primary.SetSkillOverride(this, skillDef, GenericSkill.SkillOverridePriority.Replacement);
                        break;
                    case "special":
                        base.skillLocator.special.SetSkillOverride(this, skillDef, GenericSkill.SkillOverridePriority.Contextual);
                        break;
                    default:
                        break;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!jhinStateController.ultHasSetLastShot && this.jhinStateController.ammoCount == 1)
            {
                jhinStateController.ultHasSetLastShot = true;
                SetOverride( primaryOverrideCritSkillDef, "primary" );
            }

            if (jhinStateController.ultHasSetLastShot && jhinStateController.ultHasFiredLastShot)
            {
                jhinStateController.isUlting = false;
            }

            if (base.isAuthority && (!jhinStateController.isUlting || this.fixedAge > this.duration))
            {
                this.outer.SetNextStateToMain();
            }
        }

    }
}