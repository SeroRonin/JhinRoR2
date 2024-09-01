using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using JhinMod.Content.Components;
using UnityEngine.UI;
using R2API.Utils;
using RoR2;
using JhinMod.SkillStates;
using JhinMod.Modules;
using JhinMod.SkillStates.BaseStates;

namespace JhinMod.Content.UI
{
    public class JhinAmmoUI : MonoBehaviour
    {
        private Image Circle;
        private Image CircleBG;
        private Image Shot1;
        private Image Shot2;
        private Image Shot3;
        private Image Shot4;
        private bool hidden;

        private Color critColor = new Color(1f, 0f, 0.4f);
        private Color critBGColor = new Color(0.5f, 0f, 0.2f);

        public  JhinStateController ammoComponent { get; set; }
        public EntityStateMachine ultActiveStateMachine { get; set; }
        public SkillLocator skillLocator { get; set; }

        public void Update()
        {
            this.UpdateAmmoUI();
        }

        private void Awake()
        {
            Circle = this.gameObject.GetComponentInChildren<Transform>().Find("circle").GetComponent<Image>();
            CircleBG = this.gameObject.GetComponentInChildren<Transform>().Find("circle_bg").GetComponent<Image>();
            Shot1 = this.gameObject.GetComponentInChildren<Transform>().Find("shot1").GetComponent<Image>();
            Shot2 = this.gameObject.GetComponentInChildren<Transform>().Find("shot2").GetComponent<Image>();
            Shot3 = this.gameObject.GetComponentInChildren<Transform>().Find("shot3").GetComponent<Image>();
            Shot4 = this.gameObject.GetComponentInChildren<Transform>().Find("shot4").GetComponent<Image>();
            
        }

        private void Start()
        {
            this.UpdateAmmoUI();
        }

        private void UpdateAmmoUI()
        {
            if (this.ammoComponent)
            {
                var ammo = ammoComponent.ammoCount;

                //Disable UI if we have don't have base primary available
                var hasOverride = skillLocator.primary.HasSkillOverrideOfPriority(GenericSkill.SkillOverridePriority.Replacement);
                if ( hasOverride && !ammoComponent.isUlting )
                {
                    if (!hidden)
                    {
                        hidden = true;
                        Circle.enabled = false;
                        CircleBG.enabled = false;
                        Shot1.enabled = false;
                        Shot2.enabled = false;
                        Shot3.enabled = false;
                        Shot4.enabled = false;
                    }
                }
                else
                {
                    hidden = false;
                    Circle.enabled = true;
                    CircleBG.enabled = true;
                    Shot1.enabled = true;
                    Shot2.enabled = true;
                    Shot3.enabled = true;
                    Shot4.enabled = true;
                }

                //Change UI colors based on ammo
                if (ammo == 1)
                {
                    Circle.color = critColor;
                    CircleBG.color = critBGColor;

                    Shot1.color = critBGColor;
                    Shot2.color = critBGColor;
                    Shot3.color = critBGColor;
                    Shot4.color = critColor;
                }
                else
                {
                    Shot1.color = ammo > 3 ? Color.white : Color.grey;
                    Shot2.color = ammo > 2 ? Color.white : Color.grey;
                    Shot3.color = ammo > 1 ? Color.white : Color.grey;
                    Shot4.color = ammo != 0 ? Color.white : Color.grey;
                    Circle.color = Color.white;
                    CircleBG.color = Color.grey;
                }

                

                //Animate circle based on various situations
                //Display remaining ult time
                if ( ammoComponent.isUlting )
                {
                    var ultSkill = ultActiveStateMachine.state as JhinWeaponUltActiveState;
                    if (ultSkill != null)
                    {
                        Circle.fillAmount = (ultSkill.duration - ultSkill.fixedAge) / ultSkill.duration;
                    }
                }

                //Display remaining auto-reload time
                else if ( ammoComponent.CanStartReload() )
                {
                    if (ammoComponent.ammoCount != 0)
                    {
                        Circle.fillAmount = (ammoComponent.reloadAutoDelay - ammoComponent.reloadStopwatch) / ammoComponent.reloadAutoDelay;
                    }
                    else
                    {
                        Circle.fillAmount = 0f;
                    }
                }

                //Display reload duration
                else if ( ammoComponent.startedReload )
                {
                    var primarySkill = skillLocator.primary.stateMachine.state as WhisperReload;
                    if (primarySkill != null)
                    {
                        Circle.fillAmount = primarySkill.fixedAge / primarySkill.duration;
                    }
                }
                else
                {
                    Circle.fillAmount = 1f;
                }

            }
        }
    }
}
