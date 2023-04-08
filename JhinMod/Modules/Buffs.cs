using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace JhinMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;

        /// <summary>
        /// Movespeed buff Jhin gets weapon he crits
        /// </summary>
        internal static BuffDef jhinCritMovespeedBuff;

        internal static BuffDef jhinMarkDebuff;

        internal static DamageAPI.ModdedDamageType JhinMarkDamage = DamageAPI.ReserveDamageType();
        internal static DamageAPI.ModdedDamageType JhinConsumeMarkDamage = DamageAPI.ReserveDamageType();

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("JhinArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, 
                Color.white, 
                false, 
                false);

            jhinCritMovespeedBuff = AddNewBuff("Every Moment Matters",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/CloakSpeed").iconSprite,
                new Color(1f,0f,0.44f),
                false,
                false);

            jhinMarkDebuff = AddNewBuff("Marked",
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffPassiveIcon"),
                new Color(1f, 0f, 0.44f),
                false,
                true);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}