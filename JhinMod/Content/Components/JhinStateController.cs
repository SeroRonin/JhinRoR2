using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using R2API.Utils;
using EntityStates;
using JhinMod.SkillStates;
using JhinMod.SkillStates.BaseStates;
using JhinMod.Modules;

namespace JhinMod.Content.Components
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(TeamComponent))]
    public class JhinStateController : NetworkBehaviour
    {
        public int ammoCount;     //How much ammo do we currently have?
        public int ammoMax = 4;  //How much ammo can we store? DON'T CHANGE, GAME IS NOT SET UP TO SUPPORT MORE

        public float reloadTime = 2.5f; //How long does our reload take?
        public float reloadGraceDelay = 1f; //How long after emptying our ammo do we attempt a reload?
        public float reloadAutoDelay = Config.primaryAutoReloadTime.Value; //How long after doing nothing do we attempt a reload?

        public float reloadStopwatch;
        public float timeSinceFire;

        public bool passiveCritArmed;

        public bool startedReload;
        public bool interrupted;
        public bool paused;
        public bool isUlting;

        public bool ultHasSetLastShot;
        public bool ultHasFiredLastShot;

        public SerializableEntityStateType reloadState;

        private EntityStateMachine jhinStateMachine;
        private Animator modelAnimator;

        public GameObject modelFX;

        private void Awake()
        {
            jhinStateMachine = Helpers.GetEntityStateMachine(this.gameObject, "WeaponMode");
            var modelLocator = this.GetComponent<ModelLocator>();
            modelAnimator = modelLocator.modelTransform.GetComponent<Animator>();
        }

        private void Start()
        {
            this.ammoCount = ammoMax;
            this.reloadStopwatch = 0f;

            if (modelFX == null)
            {
                var modelFXprefab = Helpers.GetVFXDynamic("ModelFX", this.gameObject);
                if (modelFXprefab != null)
                {
                    modelFX = UnityEngine.Object.Instantiate<GameObject>(modelFXprefab, this.gameObject.transform); 
                    var bindPairComp = modelFX.GetComponent<BindPairLocator>();
                    bindPairComp.target = this.gameObject.GetComponent<ModelLocator>().gameObject;
                    bindPairComp.BindPairs();
                }
            }
        }

        private void FixedUpdate()
        {
            this.UpdateTimers();

            //Start reload from autoreload timer
            if (!this.startedReload && this.reloadStopwatch >= this.reloadAutoDelay )
            {
                this.EnterReloadState();
            }

            //Start reload from empty criteria
            if (!this.startedReload && this.ammoCount <= 0 )
            {
                //Start reload from last bullet fired
                if (!this.interrupted && this.timeSinceFire < 0.5f)
                {
                    this.EnterReloadState();
                }
                //Start reload from grace delay, used to adjust how long to wait after interrupting reload with another skill
                else if (this.reloadStopwatch > this.reloadGraceDelay)
                {
                    this.EnterReloadState();
                }
            }

            if (!isUlting)
            {
                if ( this.ammoCount == 1 )
                {
                    if ( !passiveCritArmed )
                    {
                        passiveCritArmed = true;
                        jhinStateMachine.SetNextState(new JhinWeaponPassiveCritReadyState());
                    }
                }
                else if ( passiveCritArmed )
                { 
                    this.passiveCritArmed = false;
                    jhinStateMachine.SetNextStateToMain();
                }
            }

            modelAnimator.SetFloat("Reload.playbackRate", this.paused ? 0f : 1f);

        }


        private void UpdateTimers()
        {
            if (!paused)
            {
                this.timeSinceFire += Time.deltaTime;

                //We still have bullets, reset timer if not already reset
                if ( this.CanStartReload() && !paused )
                {
                    this.reloadStopwatch += Time.deltaTime;
                }
                //We have max bullets, reset timer if not already reset
                else if ( this.reloadStopwatch != 0  )
                {
                    this.reloadStopwatch = 0f;
                }
            }
            else 
            {
                if ( ammoCount >= 0 )
                {
                    this.reloadStopwatch = 0f;
                }
                else
                    this.reloadStopwatch = reloadAutoDelay;

            }
        }

        public bool CanStartReload()
        {
            if (this.startedReload) return false;
            if (this.ammoCount < this.ammoMax) return true;
            return false;
        }

        public void EnterReloadState()
        {
            var skillLocator = GetComponent<SkillLocator>();
            skillLocator.primary.stateMachine.SetNextState( new WhisperReload());

            this.startedReload = true;
        }

        public void Reload( bool full = false, int count = 1)
        {
            if (full) ammoCount = ammoMax;
            else ammoCount += count;

            this.ResetReload();
        }

        public void PauseReload(bool interrupt = false, float delay = 1f )
        {
            this.paused = true;
            this.ResetReload(interrupt, delay);
        }

        public void StopReload( bool interrupt = false, float delay = 1f, bool ignoreStateChange = false )
        {
            var skillLocator = GetComponent<SkillLocator>();

            skillLocator.primary.stateMachine.SetNextStateToMain();

            this.paused = false;

            this.ResetReload( interrupt, delay );
        }

        public void ResetReload( bool interrupt = false, float delay = 1f)
        {
            Helpers.StopSound("Reloads", base.gameObject);

            this.startedReload = false;
            this.reloadGraceDelay = delay;
            this.reloadStopwatch = 0f;
            this.interrupted = interrupt;
        }

        public void TakeAmmo( int ammo )
        {
            if (this.CanTakeAmmo(ammo) )
            {
                ammoCount -= ammo;
            }
        }

        public bool CanTakeAmmo( int ammo )
        {
            if (ammoCount <= 0) return false;
            if (ammo > ammoCount) return false;
            return true;
        }

        public void ResetUlt()
        {
            this.ultHasSetLastShot = false;
            this.ultHasFiredLastShot = false;
            this.StopReload( true );
            this.Reload( true );
        }


        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write(this.ammoCount);
                return true;
            }
            bool flag = false;
            if ((base.syncVarDirtyBits & 1U) != 0U)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(this.ammoCount);
            }
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
            }
            return flag;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if (initialState)
            {
                this.ammoCount = reader.ReadInt16();
                return;
            }
            int num = (int)reader.ReadPackedUInt32();
            if ((num & 1) != 0)
            {
                this.ammoCount = reader.ReadInt16();
            }
        }
    }
}
