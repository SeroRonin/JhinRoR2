using JhinMod.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using UnityEngine;

namespace JhinMod.Content.Components
{
    public class LobbySoundPlayer : MonoBehaviour
    {
        private uint soundUID;

        private void OnEnable()
        {
            this.Invoke("PlaySelectSound", 0.1f);
        }

        private void PlaySelectSound()
        {
            this.soundUID = Helpers.PlaySound("RecallOut", this.gameObject, skinName: "Jhin");
        }

        private void OnDestroy()
        {
            if ( this.soundUID != AkSoundEngine.AK_INVALID_PLAYING_ID )
            {
                AkSoundEngine.StopPlayingID( this.soundUID );
            }
        }
    }
}
