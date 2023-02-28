using System;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using UnityEngine;
using EntityStates.Railgunner.Scope;
using JhinMod.Content.Controllers;
using JhinMod.Modules;
using EntityStates;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using EntityStates.LemurianMonster;

namespace JhinMod.SkillStates
{
    public class CurtainCall : BaseSkillState
    {
        private float duration = 10f;
        private AmmoComponent ammoComponent;
        public override void OnEnter()
        {
            base.OnEnter();
            ammoComponent = base.GetComponent<AmmoComponent>();
            string prefix = JhinPlugin.DEVELOPER_PREFIX;

            primaryOverride = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_JHIN_BODY_SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "_JHIN_BODY_SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "_JHIN_BODY_SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialShotIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(CurtainCallShot)),
                activationStateMachineName = "Slide",
                baseMaxStock = 4,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillLocator skillLocator = base.skillLocator;
			GenericSkill genericSkill = (skillLocator != null) ? skillLocator.primary : null;
			if (genericSkill)
			{
				this.TryOverrideSkill(genericSkill);
				genericSkill.onSkillChanged += this.TryOverrideSkill;
			}

            if (base.isAuthority)
            {
                //this.loopPtr = LoopSoundManager.PlaySoundLoopLocal(base.gameObject, this.loopSound);
            }
        }

        public override void OnExit()
        {
            if (this.loopPtr.isValid)
            {
                //LoopSoundManager.StopSoundLoopLocal(this.loopPtr);
            }
            SkillLocator skillLocator = base.skillLocator;
            GenericSkill genericSkill = (skillLocator != null) ? skillLocator.primary : null;
            if (genericSkill)
            {
                genericSkill.onSkillChanged -= this.TryOverrideSkill;
            }
            if (this.overriddenSkill)
            {
                this.overriddenSkill.UnsetSkillOverride(this, this.primaryOverride, GenericSkill.SkillOverridePriority.Contextual);
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && this.fixedAge > this.duration)
            {
                this.outer.SetNextState(this.GetNextState());
            }
        }

        private void TryOverrideSkill(GenericSkill skill)
        {
            if (skill && !this.overriddenSkill && !skill.HasSkillOverrideOfPriority(GenericSkill.SkillOverridePriority.Contextual))
            {
                this.overriddenSkill = skill;
                this.overriddenSkill.SetSkillOverride(this, this.primaryOverride, GenericSkill.SkillOverridePriority.Contextual);
                //this.overriddenSkill.stock = base.skillLocator.secondary.stock;
            }
        }

        protected virtual BaseWindDown GetNextState()
        {
            return new BaseWindDown();
        }

        // Token: 0x04000AA6 RID: 2726
        [SerializeField]
        public SkillDef primaryOverride;

        // Token: 0x04000AA7 RID: 2727
        [SerializeField]
        public LoopSoundDef loopSound;

        // Token: 0x04000AA8 RID: 2728
        private GenericSkill overriddenSkill;

        // Token: 0x04000AA9 RID: 2729
        private LoopSoundManager.SoundLoopPtr loopPtr;
    }
}
