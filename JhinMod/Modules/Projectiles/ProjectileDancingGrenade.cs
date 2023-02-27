using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace JhinMod.Modules
{
    internal class ProjectileDancingGrenade
    {
    }
}
namespace JhinMod.Modules
{
    // Token: 0x02000B16 RID: 2838
    public class DancingGrenade : LightningOrb
    {
        public float damageCoefficientOnSuccessfulKill = 0.35f;
        public override void Begin()
        {
            string path = "Prefabs/Effects/OrbEffects/HuntressGlaiveOrbEffect";
            base.duration = base.distanceToTarget / this.speed;
            base.lightningType= (LightningOrb.LightningType)LightningType.Count;

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

                var damagebonus = 0f;

                if (!this.failedToKill)
                {
                    // INCREASE DAMAGE BY PERCENTAGE
                    damagebonus = this.damageValue * this.damageCoefficientOnSuccessfulKill;
                }

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
                            DancingGrenade lightningOrb = new DancingGrenade();
                            lightningOrb.search = this.search;
                            lightningOrb.origin = this.target.transform.position;
                            lightningOrb.target = hurtBox;
                            lightningOrb.attacker = this.attacker;
                            lightningOrb.inflictor = this.inflictor;
                            lightningOrb.teamIndex = this.teamIndex;
                            if (!this.failedToKill)
                            {
                                lightningOrb.damageValue = this.damageValue + damagebonus;
                            }
                            else
                            {
                                lightningOrb.damageValue = this.damageValue;
                            }
                            lightningOrb.bouncesRemaining = this.bouncesRemaining - 1;
                            lightningOrb.isCrit = this.isCrit;
                            lightningOrb.bouncedObjects = this.bouncedObjects;
                            lightningOrb.lightningType = this.lightningType;
                            lightningOrb.procChainMask = this.procChainMask;
                            lightningOrb.procCoefficient = this.procCoefficient;
                            lightningOrb.damageColorIndex = this.damageColorIndex;
                            lightningOrb.damageCoefficientPerBounce = this.damageCoefficientPerBounce;
                            lightningOrb.speed = this.speed;
                            lightningOrb.range = this.range;
                            lightningOrb.damageType = this.damageType;
                            lightningOrb.failedToKill = this.failedToKill;
                            OrbManager.instance.AddOrb(lightningOrb);
                        }
                    }
                    return;
                }
            }
        }

        public HurtBox PickNextTarget(Vector3 position)
        {
            if (this.search == null)
            {
                this.search = new BullseyeSearch();
            }
            this.search.searchOrigin = position;
            this.search.searchDirection = Vector3.zero;
            this.search.teamMaskFilter = TeamMask.allButNeutral;
            this.search.teamMaskFilter.RemoveTeam(this.teamIndex);
            this.search.filterByLoS = false;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.range;
            this.search.RefreshCandidates();
            HurtBox hurtBox = (from v in this.search.GetResults()
                               where !this.bouncedObjects.Contains(v.healthComponent)
                               select v).FirstOrDefault<HurtBox>();
            if (hurtBox)
            {
                this.bouncedObjects.Add(hurtBox.healthComponent);
            }
            return hurtBox;
        }
    }
}
