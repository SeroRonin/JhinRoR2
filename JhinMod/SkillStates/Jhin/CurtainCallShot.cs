using System;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.LemurianMonster;

namespace JhinMod.SkillStates
{
    // Token: 0x0200029F RID: 671
    public class CurtainCallShot : FireFireball
    {

        // Token: 0x06000BE1 RID: 3041 RVA: 0x000313CC File Offset: 0x0002F5CC
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 10f;
        }
        public override void FixedUpdate()
        {
            this.fixedAge += Time.fixedDeltaTime;
        }
    }
}

