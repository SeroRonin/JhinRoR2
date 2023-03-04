using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using JhinMod.Modules;

namespace JhinMod.SkillStates
{
    public class CurtainCallShootCrit : CurtainCallShoot
    {
        new public string explosionSoundEffect = "Plya_Seroronin_Jhin_UltHitLast";

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