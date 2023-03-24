using JhinMod.Modules;
using JhinMod.Modules.CustomProjectiles;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace JhinMod.Content.Components
{
    public class CustomProjectileImpactExplosion : ProjectileImpactExplosion
    {
        /*
        protected override void DetonateServer()
        {
            ChatMessage.Send("Detonate");
            if (this.explosionEffect)
            {
                EffectManager.SpawnEffect(this.explosionEffect, new EffectData
                {
                    origin = base.transform.position,
                    scale = this.blastRadius
                }, true);
            }
            if (this.projectileDamage)
            {
                CustomBlastAttack blastAttack = new CustomBlastAttack();
                blastAttack.position = base.transform.position;
                blastAttack.baseDamage = this.projectileDamage.damage * this.blastDamageCoefficient;
                blastAttack.baseForce = this.projectileDamage.force * this.blastDamageCoefficient;
                blastAttack.radius = this.blastRadius;
                blastAttack.attacker = (this.projectileController.owner ? this.projectileController.owner.gameObject : null);
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = this.projectileController.teamFilter.teamIndex;
                blastAttack.crit = this.projectileDamage.crit;
                blastAttack.procChainMask = this.projectileController.procChainMask;
                blastAttack.procCoefficient = this.projectileController.procCoefficient * this.blastProcCoefficient;
                blastAttack.bonusForce = this.bonusBlastForce;
                blastAttack.falloffModel = this.falloffModel;
                blastAttack.damageColorIndex = this.projectileDamage.damageColorIndex;
                blastAttack.damageType = this.projectileDamage.damageType;
                blastAttack.attackerFiltering = this.blastAttackerFiltering;
                blastAttack.canRejectForce = this.canRejectForce;
                BlastAttack.Result result = blastAttack.Fire();
                this.OnBlastAttackResult(blastAttack, result);
            }
            if (this.explosionSoundString.Length > 0)
            {
                Util.PlaySound(this.explosionSoundString, base.gameObject);
            }
            if (this.fireChildren)
            {
                for (int i = 0; i < this.childrenCount; i++)
                {
                    this.FireChild();
                }
            }
        }
        */

        public override void OnBlastAttackResult(BlastAttack blastAttack, BlastAttack.Result result)
        {
            base.OnBlastAttackResult(blastAttack, result);
            var damage = 0f;
            foreach (BlastAttack.HitPoint hitPoint in result.hitPoints)
            {
                HealthComponent healthComponent = hitPoint.hurtBox ? hitPoint.hurtBox.healthComponent : null;

                if (healthComponent)
                {
                    var currentHigh = damage;
                    var missingHealth = healthComponent.fullHealth - healthComponent.health;
                    var missingHealthPercent = missingHealth / healthComponent.fullHealth;
                    damage = blastAttack.baseDamage * (missingHealthPercent * Config.specialExecutePercentage.Value);
                    
                    //Band-aid patch that fixes execute damage critting AFTER already scaling off a crit, creating up to 600% bonus damage instead of the intended 300%
                    if (blastAttack.crit)
                    {
                        damage = damage / 2;
                    }

                    ChatMessage.Send($"ult base {blastAttack.baseDamage}");
                    ChatMessage.Send($"ult bonus {damage}");

                    BlastAttack.BlastAttackDamageInfo blastAttackDamageInfo = new BlastAttack.BlastAttackDamageInfo
                    {
                        attacker = blastAttack.attacker,
                        inflictor = blastAttack.inflictor,
                        crit = blastAttack.crit,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.DeathMark,
                        damageModifier = hitPoint.hurtBox.damageModifier,
                        damageType = (blastAttack.damageType | DamageType.AOE),
                        force = new Vector3(0f,0f,0f),
                        position = hitPoint.hitPosition,
                        procChainMask = blastAttack.procChainMask,
                        procCoefficient = 0f,
                        hitHealthComponent = healthComponent,
                        canRejectForce = blastAttack.canRejectForce
                    };


                    if (NetworkServer.active)
                    {
                        BlastAttack.PerformDamageServer(blastAttackDamageInfo);
                    }
                    else
                    {
                        BlastAttack.ClientReportDamage(blastAttackDamageInfo);
                    }
                }
            }
            //blastAttack.baseDamage += damage;
        }
    }
}
