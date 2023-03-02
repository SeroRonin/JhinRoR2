using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JhinMod.Modules.CustomProjectiles
{
    internal class BulletAttackCustomFalloff : BulletAttack
    {
        private static new float CalcFalloffFactor(BulletAttack.FalloffModel falloffModel, float distance)
        {
            return 0.5f + Mathf.Clamp01(Mathf.InverseLerp(100f, 50f, distance)) * 0.5f;
        }
    }
}
