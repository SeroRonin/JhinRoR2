using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using R2API.Utils;
using static UnityEngine.SendMouseEvents;

namespace JhinMod.Modules.CustomProjectiles
{
    public class CustomBlastAttack : BlastAttack
    {
        public delegate void ModifyOutgoingDamageCallback(CustomBlastAttack blastAttack, CustomBlastAttack.HitPoint hitInfo, BlastAttack.BlastAttackDamageInfo damageInfo);

        public CustomBlastAttack.ModifyOutgoingDamageCallback modifyOutgoingDamageCallback = null;
        private void HandleHits(BlastAttack.HitPoint[] hitPoints)
        {
            var executeDam = 0f;
            //Custom, calculate maximum execute damage based on highest health
            /*
            foreach (BlastAttack.HitPoint hitPoint in hitPoints)
            {
                HealthComponent healthComponent = hitPoint.hurtBox ? hitPoint.hurtBox.healthComponent : null;
                if (healthComponent)
                {
                    var currentHigh = executeDam;
                    var missingHealth = hitPoint.hurtBox.healthComponent.fullHealth - hitPoint.hurtBox.healthComponent.health;
                    var outputDamage = missingHealth * StaticValues.executeMissingHealthDamagePercent;
                    executeDam = Math.Max(executeDam, outputDamage);
                }
            }
            */

            Vector3 b = this.position;
            foreach (BlastAttack.HitPoint hitPoint in hitPoints)
            {
                float num = Mathf.Sqrt(hitPoint.distanceSqr);
                float num2 = 0f;
                Vector3 a = (num > 0f) ? ((hitPoint.hitPosition - b) / num) : Vector3.zero;
                HealthComponent healthComponent = hitPoint.hurtBox ? hitPoint.hurtBox.healthComponent : null;
                if (healthComponent)
                {
                    switch (this.falloffModel)
                    {
                        case BlastAttack.FalloffModel.None:
                            num2 = 1f;
                            break;
                        case BlastAttack.FalloffModel.Linear:
                            num2 = 1f - Mathf.Clamp01(num / this.radius);
                            break;
                        case BlastAttack.FalloffModel.SweetSpot:
                            num2 = 1f - ((num > this.radius / 2f) ? 0.75f : 0f);
                            break;
                    }
                    BlastAttack.BlastAttackDamageInfo blastAttackDamageInfo = new BlastAttack.BlastAttackDamageInfo
                    {
                        attacker = this.attacker,
                        inflictor = this.inflictor,
                        crit = this.crit,
                        damage = this.baseDamage * num2,
                        damageColorIndex = this.damageColorIndex,
                        damageModifier = hitPoint.hurtBox.damageModifier,
                        damageType = (this.damageType | DamageType.AOE),
                        force = this.bonusForce * num2 + this.baseForce * num2 * a,
                        position = hitPoint.hitPosition,
                        procChainMask = this.procChainMask,
                        procCoefficient = this.procCoefficient,
                        hitHealthComponent = healthComponent,
                        canRejectForce = this.canRejectForce
                    };

                    //CustomBlastAttack.modifyOutgoingDamageCallback?.Invoke(this ,hitPoint, blastAttackDamageInfo);

                    //Custom, calculate execute damage per individual
                    var missingHealth = healthComponent.fullHealth - healthComponent.health;
                    executeDam = missingHealth * StaticValues.executePrimaryMissingHealthDamagePercent;
                    ChatMessage.Send($"Execute Dam Ult {executeDam}");

                    //Custom, apply execute damage per individual
                    blastAttackDamageInfo.damage += Math.Min(this.baseDamage, executeDam);

                    if (NetworkServer.active)
                    {
                        BlastAttack.PerformDamageServer(blastAttackDamageInfo);
                    }
                    else
                    {
                        BlastAttack.ClientReportDamage(blastAttackDamageInfo);
                    }
                    if (this.impactEffect != EffectIndex.Invalid)
                    {
                        EffectData effectData = new EffectData();
                        effectData.origin = hitPoint.hitPosition;
                        effectData.rotation = Quaternion.LookRotation(-a);
                        EffectManager.SpawnEffect(this.impactEffect, effectData, true);
                    }
                }
            }
        }
    }
}
