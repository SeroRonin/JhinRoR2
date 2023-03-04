using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Utils;

namespace JhinMod.Modules.CustomProjectiles
{
    public class ProjectileDancingGrenade : LightningOrb
    {
        public float damageCoefficientOnBounceKill;
        public override void Begin()
        {
            string path = "Prefabs/Effects/OrbEffects/HuntressGlaiveOrbEffect";
            base.duration = base.distanceToTarget / this.speed;
            base.canBounceOnSameTarget = false;
            EffectData effectData = new EffectData
            {
                origin = this.origin,
                genericFloat = base.duration
            };

            effectData.SetHurtBoxReference(this.target);
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>(path), effectData, true);
        }

        public override void OnArrival()
        {
            if (this.target)
            {
                HealthComponent healthComponent = this.target.healthComponent;
                if (healthComponent)
                {
                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.damage = this.damageValue;
                    damageInfo.attacker = this.attacker;
                    damageInfo.inflictor = this.inflictor;
                    damageInfo.force = Vector3.zero;
                    damageInfo.crit = this.isCrit;
                    damageInfo.procChainMask = this.procChainMask;
                    damageInfo.procCoefficient = this.procCoefficient;
                    damageInfo.position = this.target.transform.position;
                    damageInfo.damageColorIndex = this.damageColorIndex;
                    damageInfo.damageType = this.damageType;
                    healthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
                }

                //Did we kill the target we hit?
                this.failedToKill |= (!healthComponent || healthComponent.alive);

                Util.PlaySound("Play_Seroronin_Jhin_QHitLast", this.target.gameObject);
                
                if (this.bouncesRemaining > 0)
                {
                    for (int i = 0; i < this.targetsToFindPerBounce; i++)
                    {
                        if (this.bouncedObjects != null)
                        {
                            if (this.canBounceOnSameTarget)
                            {
                                this.bouncedObjects.Clear();
                            }
                            this.bouncedObjects.Add(this.target.healthComponent);
                        }
                        HurtBox hurtBox = this.PickNextTarget(this.target.transform.position);
                        if (hurtBox)
                        {
                            ProjectileDancingGrenade lightningOrb = new ProjectileDancingGrenade();
                            lightningOrb.search = this.search;
                            lightningOrb.origin = this.target.transform.position;
                            lightningOrb.target = hurtBox;
                            lightningOrb.attacker = this.attacker;
                            lightningOrb.inflictor = this.inflictor;
                            lightningOrb.teamIndex = this.teamIndex;
                            lightningOrb.damageValue = this.damageValue;
                            
                            lightningOrb.bouncesRemaining = this.bouncesRemaining - 1;
                            lightningOrb.isCrit = this.isCrit;
                            lightningOrb.bouncedObjects = this.bouncedObjects;
                            lightningOrb.lightningType = this.lightningType;
                            lightningOrb.procChainMask = this.procChainMask;
                            lightningOrb.procCoefficient = this.procCoefficient;
                            lightningOrb.damageColorIndex = this.damageColorIndex;
                            lightningOrb.damageCoefficientOnBounceKill = this.damageCoefficientOnBounceKill;
                            lightningOrb.speed = this.speed;
                            lightningOrb.range = this.range;
                            lightningOrb.damageType = this.damageType;
                            lightningOrb.duration = base.distanceToTarget / this.speed;

                            //If we killed, add a percentage of current damage on top
                            if (!this.failedToKill)
                            {
                                lightningOrb.damageValue += this.damageValue * this.damageCoefficientOnBounceKill;
                            }
                            OrbManager.instance.AddOrb(lightningOrb);
                            Util.PlaySound("Play_Seroronin_Jhin_QBounce", this.target.gameObject );
                        }
                        else
                        {
                            Util.PlaySound("Play_Seroronin_Jhin_QHitLast", this.target.gameObject);
                        }
                    }
                    return;
                }
            }
        }
    }
}
