using EmotesAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace JhinMod.Content
{
    public static class CustomEmotesAPISupport
    {
        internal static void HookCustomEmoteAPI()
        {
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            
        }
        internal static void UnhookCustomEmoteAPI()
        {
            On.RoR2.SurvivorCatalog.Init -= SurvivorCatalog_Init;
            CustomEmotesAPI.animChanged -= CustomEmotesAPI_animChanged;

        }
        internal static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();

            foreach (var item in SurvivorCatalog.allSurvivorDefs)
            {
                if (item.bodyPrefab.name == "JhinBody")
                {
                    var skele = Modules.Asset.mainAssetBundle.LoadAsset<GameObject>("emoteJhin");
                    CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele);
                    skele.GetComponentInChildren<BoneMapper>().scale = 1.1f;
                }
            }
        }

        internal static void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            if (newAnimation != "none")
            {
                if (mapper.transform.name == "emoteJhin")
                {
                    mapper.transform.parent.Find("JhinMeshWeapon").gameObject.SetActive(false);
                }
            }
            else
            {
                if (mapper.transform.name == "emoteJhin")
                {
                    mapper.transform.parent.Find("JhinMeshWeapon").gameObject.SetActive(true);
                }
            }
        }
    }
}
