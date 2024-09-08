using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using R2API.Networking;
using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.BulletAttack;
using R2API.Utils;
using JhinMod.Content.Components;
using static RoR2.DotController;
using JhinMod.Modules;
using System.Collections.Generic;
using System.Net.Mail;
using System.ComponentModel;
using static UnityEngine.SendMouseEvents;

namespace JhinMod.SkillStates
{
    public class WhisperPrimary : BaseSkillState
    {
        public static float damageCoefficient = Modules.Config.primaryDamageCoefficient.Value;
        public static float procCoefficient = 1f;
        public static float baseDuration = 2.57f;
        //public static float baseFireDelayPercent = 0.15625f; //League value, is actually 4ish frames late
        public static float baseFireDelayPercent = 0.1f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private JhinStateController jhinStateController;
        private float duration;
        private float durationAnimation; //Used to get the right animation speed
        private float fireTime;
        private bool hasFired;
        private bool isCrit;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            jhinStateController = base.GetComponent<JhinStateController>();
            jhinStateController.timeSinceFire = 0f;
            jhinStateController.isAttacking = true;
            jhinStateController.ResetReload();

            Helpers.StopSound("PassiveCritSpin", base.gameObject, skinName: "Jhin");
            Helpers.StopSound("PassiveCritMusic", base.gameObject, skinName: "Jhin");

            base.characterBody.SetAimTimer(3f);
            

            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1);

            this.durationAnimation = WhisperPrimary.baseDuration * (this.characterBody.baseAttackSpeed / this.characterBody.attackSpeed);
            this.duration = 1 / this.characterBody.attackSpeed;
            this.fireTime = WhisperPrimary.baseFireDelayPercent * this.durationAnimation;
            this.muzzleString = "Muzzle";
            this.isCrit = RollCrit();
            //this.earlyExitTime = 1 / this.characterBody.attackSpeed;

            if (Config.primaryInstantShot.Value == true && shotIndex != 4 )
            {
                this.fireTime = 0f;
            }
            else
            {
                var soundUID = Helpers.PlaySound(shotIndex == 4 ? "PassiveCritCast" : $"AttackCast{shotIndex}", base.gameObject, defaultToBase: false);

                //We didn't get a sound for the current variant, attempt using first variant for this skin before defaulting to base skin audio
                if (soundUID == AkSoundEngine.AK_INVALID_PLAYING_ID)
                    Helpers.PlaySound("AttackCast1", base.gameObject);
            }

            this.PlayFireAnimation();


        }
        public void PlayFireAnimation()
        {
            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1 );
            var animatorComponent = this.GetModelAnimator();

            var mult = WhisperPrimary.baseDuration / this.durationAnimation;
            var cycleOffset = Config.primaryInstantShot.Value == true ? 0.15625f * mult : 0f;

            if (shotIndex == 4)
            {
                
                var layerIndex = animatorComponent.GetLayerIndex("UpperBody, Override");
                animatorComponent.SetLayerWeight(layerIndex, 0f);
                base.PlayAnimation("FullBody Passive Crit, Override", "AttackPassiveCrit", "ShootGun.playbackRate", durationAnimation);
            }
            else if (this.isCrit)
            {
                    
                PlayAnimationOnAnimatorCustom(animatorComponent, "UpperBody, Override", "AttackCrit", "ShootGun.playbackRate", durationAnimation, cycleOffset);
            }
            else
            {
                PlayAnimationOnAnimatorCustom(animatorComponent, "UpperBody, Override", $"Attack{shotIndex}", "ShootGun.playbackRate", durationAnimation, cycleOffset);
            }

        }

        protected static void PlayAnimationOnAnimatorCustom(Animator modelAnimator, string layerName, string animationStateName, string playbackRateParam, float duration, float cycleOffset)
        {
            modelAnimator.speed = 1f;
            modelAnimator.Update(0f);
            int layerIndex = modelAnimator.GetLayerIndex(layerName);
            if (layerIndex >= 0)
            {
                modelAnimator.SetFloat(playbackRateParam, 1f);
                modelAnimator.PlayInFixedTime(animationStateName, layerIndex, cycleOffset);
                modelAnimator.Update(0f);
                float length = modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).length;
                modelAnimator.SetFloat(playbackRateParam, length / duration);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                //If we only have the last shot, crit regardless
                if (jhinStateController.ammoCount == 1)
                {
                    isCrit = true;
                    var animatorComponent = this.GetModelAnimator();
                    var layerIndex = animatorComponent.GetLayerIndex("UpperBody, Override");
                    animatorComponent.SetLayerWeight(layerIndex, 1f);
                }

                base.characterBody.SetAimTimer(this.duration * 2f);
                base.characterBody.AddSpreadBloom(1.5f);
                this.DoFireEffects();
                base.AddRecoil(-1f * WhisperPrimary.recoil, -2f * WhisperPrimary.recoil, -0.5f * WhisperPrimary.recoil, 0.5f * WhisperPrimary.recoil);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    BulletAttack bulletAttack = this.GenerateBulletAttack(aimRay);
                    //this.ModifyBullet(bulletAttack);
                    bulletAttack.Fire();
                    this.OnFireBulletAuthority(aimRay);
                }
                
                jhinStateController.TakeAmmo(1);
                jhinStateController.isAttacking = false;
            }
        }

        protected BulletAttack GenerateBulletAttack(Ray aimRay)
        {
            return new BulletAttack
            {
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                owner = base.gameObject,
                weapon = null,
                bulletCount = 1,
                damage = this.damageStat * WhisperPrimary.damageCoefficient,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                force = WhisperPrimary.force,
                HitEffectNormal = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = WhisperPrimary.procCoefficient,
                maxDistance = WhisperPrimary.range,
                radius = 0.25f,
                isCrit = isCrit,
                muzzleName = muzzleString,
                minSpread = 0f,
                maxSpread = 0f,
                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                smartCollision = false,
                sniper = false,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                tracerEffectPrefab = WhisperPrimary.tracerEffectPrefab,
                hitCallback = BulletHitCallback,
                modifyOutgoingDamageCallback = ModifyDamage
            };
        }

        public virtual bool CreateTracer( string tracerName, Ray aimRay )
        {
            //Fake/psuedo bullet raycast
            LayerMask mask = BulletAttack.defaultHitMask;
            RaycastHit raycastHit;
            Physics.SphereCast(aimRay.origin, 0.25f, aimRay.direction, out raycastHit, WhisperPrimary.range, mask, QueryTriggerInteraction.Ignore);
            
            //Create prefab
            var effectPrefab = Helpers.GetVFXDynamic(tracerName, base.gameObject);
            if (effectPrefab != null)
            {
                //Assign variables to custom component
                var tracerComp = effectPrefab.GetComponent<CustomTracer>();
                tracerComp.origin = aimRay.origin;
                tracerComp.target = raycastHit.collider != null ? raycastHit.point : aimRay.origin + (aimRay.direction * WhisperPrimary.range); //Use raycast hit or max range
                tracerComp.projectileSpeed = 250f;
                tracerComp.maxDistance = raycastHit.collider != null ? raycastHit.distance : WhisperPrimary.range; //Use raycast hit or max range

                //Update trail renderer material, does not use Unity tiling values for some reason
                //var trailRender = effectPrefab.gameObject.transform.GetChild(0).Find("Trail").gameObject.GetComponent<TrailRenderer>();
                //var materials = trailRender.materials;
                //foreach (var material in materials)
                //{
                //   material.SetTextureScale("_Texture0", new Vector2(0.05f, 1));
                //}

                //Create Effect from position data
                ModelLocator component = base.gameObject.GetComponent<ModelLocator>();
                if (component && component.modelTransform)
                {
                    ChildLocator component2 = component.modelTransform.GetComponent<ChildLocator>();
                    if (component2)
                    {
                        int childIndex = component2.FindChildIndex(this.muzzleString);
                        Transform transform = component2.FindChild(childIndex);
                        if (transform)
                        {
                            EffectData effectData = new EffectData
                            {
                                origin = transform.position,
                                rotation = Util.QuaternionSafeLookRotation(aimRay.direction)
                            };
                            effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                            EffectManager.SpawnEffect(effectPrefab, effectData, false);
                        }
                    }
                }

                //Initiate tracer movement now that all variables are set
                tracerComp.isActive = true;
                return true;
            }

            //If we got here, prefab retrieval failed
            return false;
        }

        protected virtual void DoFireEffects()
        {
            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1);
            var aimRay = base.GetAimRay();
            if (shotIndex ==  4)
            {
                Helpers.PlaySound("PassiveCritFire", base.gameObject);
                if ( !this.CreateTracer("TracerFourth", aimRay) )
                {
                    //Failed to create Fourth Shot Tracer, look for regular tracer
                    this.CreateTracer("Tracer", aimRay);
                }
                Helpers.PlayVFXDynamic("MuzzleFlashFourth", base.gameObject, this.muzzleString, true, aimRay);
            }
            else 
            {
                uint soundUID;
                if (this.isCrit)
                {
                    soundUID = Helpers.PlaySound("AttackCritFire", base.gameObject);
                }
                else
                {
                    soundUID = Helpers.PlaySound($"AttackFire{shotIndex}", base.gameObject, defaultToBase: false);
                }

                //We didn't get a sound for the current variant, attempt using first variant for this skin before defaulting to base skin audio
                if (soundUID == AkSoundEngine.AK_INVALID_PLAYING_ID)
                {
                    Helpers.PlaySound("AttackFire1", base.gameObject);
                }

                this.CreateTracer("Tracer", aimRay);
                Helpers.PlayVFXDynamic("MuzzleFlash", base.gameObject, this.muzzleString, true, aimRay);
            }
        }

        protected virtual void OnFireBulletAuthority(Ray aimRay)
        {
        }

        private bool BulletHitCallback(BulletAttack bulletAttack, ref BulletHit hitInfo)
        {
            
            var result = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            HealthComponent healthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;

            //TO DO: REPLACE WITH LESS REDUNDANT CODE
            
            /*
            if (jhinStateController.ammoCount == 1 && healthComponent && hitInfo.hitHurtBox.teamIndex != base.teamComponent.teamIndex)
            {
                
            }
            */

            return result;
        }
        
        private void ModifyDamage(BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
        {
            var skillLocator = this.GetComponent<SkillLocator>();

            if ( skillLocator && skillLocator.utility.cooldownRemaining < 4 )
            {
                R2API.DamageAPI.AddModdedDamageType(damageInfo, Modules.Buffs.JhinMarkDamage);
            }

            var shotIndex = this.jhinStateController.ammoMax - (this.jhinStateController.ammoCount - 1);
            var targetHealthComponent = hitInfo.hitHurtBox ? hitInfo.hitHurtBox.healthComponent : null;

            if (targetHealthComponent && hitInfo.hitHurtBox.teamIndex != base.teamComponent.teamIndex)
            {
                if (shotIndex == 4)
                {
                    var executeDamage = (targetHealthComponent.missingCombinedHealth) * Config.primaryExecuteMissingHealthPercentage.Value;
                    var maxDamage = (damageInfo.damage * Config.primaryExecuteDamageCap.Value);

                    if (Config.primaryExecuteDamageCap.Value != 0f)
                       damageInfo.damage += Math.Min(executeDamage, maxDamage);
                    else
                        damageInfo.damage += executeDamage;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ( base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }
            //TODO Replace this, breaks shuriken
            //if ( hasFired && base.fixedAge >= this.earlyExitTime)
            //{
            //    if (this.inputBank.skill1.down)
            //    {
            //          this.outer.SetNextState(new WhisperPrimary());
            //    }
            //}

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}