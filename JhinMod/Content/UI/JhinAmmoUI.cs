using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using JhinMod.Content.Components;
using UnityEngine.UI;
using R2API.Utils;
using RoR2;

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
        public SkillLocator skillLocator { get; set; }

        public void Update()
        {
            this.UpdateAmmoUI();
        }

        private void Awake()
        {
            /*
            var _text = this.gameObject.GetComponentInChildren<Transform>().Find("Text");
            var _currentGrit = _text.GetComponentInChildren<Transform>().Find("CurrentGrit");
            var _currentText = _currentGrit.GetComponent<TextMeshProUGUI>();
            var _maxGrit = _text.GetComponentInChildren<Transform>().Find("MaxGrit");
            var _maxText = _maxGrit.GetComponent<TextMeshProUGUI>();
            var _gritBar = this.gameObject.GetComponentInChildren<Transform>().Find("Grit");
            gritBar = _gritBar.GetComponent<Image>();
            var _gritBarBG = this.gameObject.GetComponentInChildren<Transform>().Find("Background");
            gritBarBG = _gritBarBG.GetComponent<Image>();
            if (_currentText) currentGritText = _currentText;
            if (_maxText) maxGritText = _maxText;
            */

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
                if ( hasOverride )
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

                //Animate circle based on Autoreload timer
                if ( ammoComponent.startedReload || ammoComponent.ammoCount == 0)
                {
                    Circle.fillAmount = 0f;
                }
                else if ( ammoComponent.CanStartReload() )
                {
                    Circle.fillAmount = (ammoComponent.reloadAutoDelay - ammoComponent.reloadStopwatch) / ammoComponent.reloadAutoDelay;
                }
                else
                {
                    Circle.fillAmount = 1f;
                }

            }
        }
    }
}
