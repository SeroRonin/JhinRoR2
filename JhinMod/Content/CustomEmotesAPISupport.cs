using EmotesAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using JhinMod.Content.Components;
using System.Linq;
using JhinMod.Modules;

namespace JhinMod.Content
{
    public static class CustomEmotesAPISupport
    {
        internal static void HookCustomEmoteAPI()
        {
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            CustomEmotesAPI.CreateNameTokenSpritePair("SERORONIN_JHIN_BODY_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texCustomEmotes_JhinIcon"));
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
                    BoneMapper boneMapper = skele.GetComponentInChildren<BoneMapper>();
                    boneMapper.scale = 1.1f;
                }
            }
        }

        internal static void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            var dynBones = mapper.transform.parent.GetComponentsInChildren<DynamicBone>();

            if (newAnimation != "none")
            {
                if (mapper.transform.name.Contains("emoteJhin"))
                {
                    mapper.transform.parent.Find("JhinMeshWeapon").gameObject.SetActive(false);
                    var modelFx = mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<JhinStateController>().modelFX;
                    if ( modelFx )
                    {
                        modelFx.GetComponent<ChildLocator>().FindChild("Barrel").localScale = new Vector3( 0, 0, 0 );
                        modelFx.GetComponent<ChildLocator>().FindChild("Pistol").localScale = new Vector3( 0, 0, 0 );
                    }
                    foreach (var dynBone in dynBones)
                    {
                        dynBone.enabled = true;
                    }
                }
            }
            else
            {
                if (mapper.transform.name.Contains("emoteJhin"))
                {
                    mapper.transform.parent.Find("JhinMeshWeapon").gameObject.SetActive(true);
                    var modelFx = mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<JhinStateController>().modelFX;
                    if (modelFx)
                    {
                        modelFx.GetComponent<ChildLocator>().FindChild("Pistol").localScale = new Vector3( 1, 1, 1 );
                        modelFx.GetComponent<ChildLocator>().FindChild("Barrel").localScale = new Vector3( 1, 1, 1 );
                    }
                    foreach (var dynBone in dynBones)
                    {
                        dynBone.enabled = false;
                    }
                }
            }
        }
    }
}
