using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using JhinMod.Modules;
using R2API.Utils;
using UnityEngine.AddressableAssets;
using R2API;

namespace JhinMod.SkillStates
{
    public class CurtainCallShootCrit : CurtainCallShoot
    {
        public override bool CheckCrit()
        {
            return true;
        }
        public override void OnExit()
        {
            this.jhinStateController.ultHasFiredLastShot = true;
            base.OnExit();
        }
    }
}