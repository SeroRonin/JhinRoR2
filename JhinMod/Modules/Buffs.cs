using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace JhinMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;
        internal static BuffDef jhinCritMovespeedBuff;
        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("JhinArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, 
                Color.white, 
                false, 
                false);

            jhinCritMovespeedBuff = AddNewBuff("Every Moment Matters",
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon"),
                new Color(1f,0f,0.44f),
                false,
                false);
        }

        //new Color(1f,0f,0.44f),
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