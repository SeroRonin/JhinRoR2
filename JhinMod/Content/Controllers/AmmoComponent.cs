using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RoR2;
using JhinMod.SkillStates;
using EntityStates;
using R2API.Utils;
using System.Xml;
using UnityEngine.Networking;

namespace JhinMod.Content.Controllers
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(TeamComponent))]
    public class AmmoComponent : NetworkBehaviour
    {
        public int ammoCount;     //How much ammo do we currently have?
        public int ammoMax = 4;  //How much ammo can we store?

        public float reloadTime = 2.5f; //How long does our reload take?
        public float reloadGraceDelay = 1f; //How long after emptying our ammo do we attempt a reload?
        public float reloadAutoDelay = 10f; //How long after doing nothing do we attempt a reload?

        public float reloadStopwatch;
        public float timeSinceEmpty;

        public bool startedReload;
        public bool interrupted;

        public SerializableEntityStateType reloadState;

        private void Awake()
        {
        }

        private void Start()
        {
            this.ammoCount = ammoMax;
            this.reloadStopwatch = 0f;
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
                if (!this.interrupted && this.timeSinceEmpty < 0.5f)
                {
                    this.EnterReloadState();
                }
                //Start reload from grace delay, used to adjust how long to wait after interrupting reload with another skill
                else if (this.reloadStopwatch > this.reloadGraceDelay)
                {
                    this.EnterReloadState();
                }
            }
        }

        private void UpdateTimers()
        {
            //Just exhausted our last bullet, start timer
            if (this.ammoCount == 0)
            {
                this.timeSinceEmpty += Time.deltaTime;
            }
            //We still have bullets, reset timer if not already reset
            else if (timeSinceEmpty != 0f)
            {
                this.timeSinceEmpty = 0f;
            }

            if (this.CanStartReload())
            {
                this.reloadStopwatch += Time.deltaTime;
            }
            //We have max bullets, reset timer if not already reset
            else if (this.reloadStopwatch != 0)
            {
                this.reloadStopwatch = 0f;
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
            //this.entityStateMachine.SetNextState(new WhisperReload()); //FIX, Freezes character movement
            ChatMessage.Send($"Reloading, Grace: {this.reloadGraceDelay}");
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

        public void StopReload( bool interrupt = false, float delay = 1f )
        {
            var skillLocator = GetComponent<SkillLocator>();
            skillLocator.primary.stateMachine.SetNextStateToMain();

            Util.PlaySound("JhinStopReload", base.gameObject);
            Util.PlaySound("JhinStopReloadEmpty", base.gameObject);
            if (this.startedReload) this.startedReload = false;
            this.reloadGraceDelay = delay;
            this.reloadStopwatch = 0f;
            this.interrupted = interrupt;
        }
        public void ResetReload()
        {
            this.startedReload = false;
            this.interrupted = false;
            this.reloadStopwatch = 0f;
            this.reloadGraceDelay = 1f;
        }

        public void TakeAmmo( int ammo )
        {
            if (this.CanTakeAmmo(ammo) )
            {
                ammoCount -= ammo;
                ChatMessage.Send($"Ammo taken, ammoCount: {ammoCount}");
            }
            if (ammoCount == 0)
            {
                timeSinceEmpty = 0f;
            }
        }

        public bool CanTakeAmmo( int ammo )
        {
            if (ammoCount <= 0) return false;
            if (ammo > ammoCount) return false;
            return true;
        }

        /*
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
        }*/
    }
}
