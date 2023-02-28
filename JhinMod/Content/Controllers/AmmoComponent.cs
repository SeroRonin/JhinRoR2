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
        private CharacterBody characterBody;
        private InputBankTest inputBank;
        private TeamComponent teamComponent;
        private EntityStateMachine entityStateMachine;

        public int ammoCount;     //How much ammo do we currently have?
        public int ammoMax = 4;  //How much ammo can we store?

        public float reloadTime = 2.5f; //How long does our reload take?
        public float reloadGraceDelay = 1f; //How long after emptying our ammo do we attempt a reload?
        public float reloadAutoDelay = 10f; //How long after doing nothing do we attempt a reload?

        public float reloadStopwatch;
        public float timeSinceEmpty;

        public bool needsReload;
        public bool startedReload;
        public bool interruptReload;

        public SerializableEntityStateType reloadState;

        private void Awake()
        {
            //this.indicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>(indicatorPrefab));
        }

        private void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
            this.entityStateMachine = base.GetComponent<EntityStateMachine>();
            this.ammoCount = ammoMax;
            this.reloadStopwatch = 0f;
        }

        private void OnEnable()
        {
            //this.indicator.active = true;
        }

        private void OnDisable()
        {
            //this.indicator.active = false;
        }

        private void FixedUpdate()
        {
            if (ammoCount == 0)
            {
                timeSinceEmpty += Time.deltaTime;
            }
            if (CanStartReload())
            {
                this.reloadStopwatch += Time.deltaTime;
            }
            else if (this.reloadStopwatch != 0)
            {
                this.reloadStopwatch = 0f;
            }
            if (!this.startedReload && this.reloadStopwatch >= this.reloadAutoDelay )
            {
                EnterReloadState();
            }
            if (!this.startedReload && ammoCount <= 0 )
            {
                if (timeSinceEmpty < 0.5f)
                {
                    EnterReloadState();
                }
                else if (this.reloadStopwatch > this.reloadGraceDelay)
                {
                    EnterReloadState();
                }
            }
        }

        public bool CanStartReload()
        {
            if (startedReload) return false;
            if (ammoCount < ammoMax) return true;
            return false;
        }

        public void EnterReloadState()
        {
            //this.entityStateMachine.SetNextState(new WhisperReload()); //FIX, Freezes character movement
            var skillLocator = GetComponent<SkillLocator>();
            skillLocator.primary.stateMachine.SetNextState( new WhisperReload());

            startedReload = true;
        }

        public void Reload( bool full = false, int count = 1)
        {
            if (full) ammoCount = ammoMax;
            else ammoCount += count;

            ChatMessage.Send($"Reloaded, ammoCount: {ammoCount}");
            startedReload = false;
        }

        public void StopReload()
        {
            var skillLocator = GetComponent<SkillLocator>();
            skillLocator.primary.stateMachine.SetNextStateToMain();

            if (startedReload) startedReload = false;
            reloadStopwatch = 0;
        }

        public void TakeAmmo( int ammo )
        {
            if ( CanTakeAmmo(ammo) )
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
