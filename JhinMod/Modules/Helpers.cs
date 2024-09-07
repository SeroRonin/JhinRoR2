using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RoR2;
using System.Linq;
using R2API.Utils;
using BepInEx;
using BepInEx.Configuration;
using System.Collections;

namespace JhinMod.Modules
{
    internal static class Helpers
    {
        internal const string agilePrefix = "<style=cIsUtility>Agile.</style> ";
        internal const string stunningPrefix = "<style=cIsUtility>Stunning.</style> ";
        internal const string captivatingPrefix = "<color=#ff5078>Captivating.</color> ";
        internal const string executingPrefix = "<color=#ff5078>Executing.</color> ";

        internal static string KeywordText(string keyword, string sub)
        {
            return $"<style=cKeywordName>{keyword}</style><style=cSub>{sub}</style>";
        }

        internal static string ScepterDescription(string desc)
        {
            return "\n<color=#d299ff>SCEPTER: " + desc + "</color>";
        }

        public static T[] Append<T>(ref T[] array, List<T> list)
        {
            var orig = array.Length;
            var added = list.Count;
            Array.Resize<T>(ref array, orig + added);
            list.CopyTo(array, orig);
            return array;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);

        public static EntityStateMachine GetEntityStateMachine( GameObject gameObject, string stateMachineName)
        {
            var allSMs = gameObject.GetComponents<EntityStateMachine>();
            EntityStateMachine stateMachine = null;
            foreach (EntityStateMachine entSM in allSMs)
            {
                if (entSM.customName == stateMachineName)
                {
                    stateMachine = entSM;
                    break;
                }
            }
            return stateMachine;
        }

        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anyt$$anonymous$$ng specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

        /// <summary>
        /// Calculates a height based a parabola rooted at (0,0) and ending at (0, totalDistance)
        /// </summary>
        /// <param name="totalDistance"></param>
        /// <param name="currentDistance"></param>
        /// <returns></returns>
        public static float GetParabolaHeight(float totalDistance, float currentDistance)
        {
            // a(x - b)^2 + c
            // a * sqrd + c
            float heightModif = 2f; //Higher number = less height
            float a = -1f / (totalDistance * heightModif);
            float b = totalDistance / 2f;
            float c = totalDistance / (4f * heightModif);
            float x = currentDistance;
            float squared = (float)Math.Pow((x - b), 2f);


            return (float)((a * squared) + c);
        }

        public static uint PlaySound(string soundID, GameObject player, GameObject parent = null, string skinName = "", bool defaultToBase = true)
        {
            if ( parent == null) parent = player;
            int skinIndex = (int)player.GetComponent<CharacterBody>().skinIndex;

            // Use dynamic skin unless specified
            if (skinName == "")
                skinName = GetSkinNameSFX(skinIndex);

            var soundUID = Util.PlaySound($"Play_Seroronin_{skinName}_{soundID}", parent);

            //We didn't get a sound event, try to use base sound
            if ( soundUID == AkSoundEngine.AK_INVALID_PLAYING_ID && defaultToBase)
            {
                soundUID = Util.PlaySound($"Play_Seroronin_Jhin_{soundID}", parent);
            }

            return soundUID;
        }

        public static uint StopSound(string soundID, GameObject player, GameObject parent = null, string skinName = "", bool defaultToBase = true)
        {
            if (parent == null) parent = player;
            int skinIndex = (int)player.GetComponent<CharacterBody>().skinIndex;

            // Use dynamic skin unless specified
            if (skinName == "")
                skinName = GetSkinNameSFX(skinIndex);
            var soundUID = Util.PlaySound($"Stop_Seroronin_{skinName}_{soundID}", parent);

            //We didn't get a sound event, try to use base sound
            if (soundUID == AkSoundEngine.AK_INVALID_PLAYING_ID)
            {
                soundUID = Util.PlaySound($"Stop_Seroronin_Jhin_{soundID}", parent);
            }

            return soundUID;
        }

        /// <summary>
        /// Spawns a unique VFX prefab, using string and a skin name generated from GetSkinName()
        /// </summary>
        /// <param name="vfxString"></param>
        /// <param name="player"></param>
        /// <param name="attachmentName"></param>
        /// <param name="parent"></param>
        /// <param name="transmit"></param>
        public static void PlayVFXDynamic(string vfxString, GameObject player, string attachmentName = "", bool useAim = false, Ray aimRay = new Ray(), GameObject parent = null, bool transmit = false)
        {
            GameObject effectPrefab = null;

            //Use the player if target parent goes null
            if (parent == null) parent = player;
            if (!player)
            {
                return;
            }

            int skinIndex = (int)player.GetComponent<CharacterBody>().skinIndex;
            string skinName = GetSkinNameVFX(skinIndex);

            //Search table for matching key, otherwise use default skin
            if (Asset.vfxPrefabs.ContainsKey( $"{skinName}_{vfxString}" ))
            {
                effectPrefab = Asset.vfxPrefabs[$"{skinName}_{vfxString}"];
            }
            else if ( Asset.vfxPrefabs.ContainsKey($"Jhin_{vfxString}" ) ) //Redunant ContainsKey check, but neccessary to prevent NREs stopping future code
            {
                effectPrefab = Asset.vfxPrefabs[$"Jhin_{vfxString}"];
            }

            if (effectPrefab == null)
            {
                return;
            }

            //Generate Effect transform info
            EffectData effectData = new EffectData
            {
                origin = parent.gameObject.transform.position
            };

            //Find attachment
            if (attachmentName != "")
            {
                ModelLocator component = parent.GetComponent<ModelLocator>();
                if (component && component.modelTransform)
                {
                    ChildLocator component2 = component.modelTransform.GetComponent<ChildLocator>();
                    if (component2)
                    {
                        int childIndex = component2.FindChildIndex(attachmentName);
                        Transform transform = component2.FindChild(childIndex);
                        if (transform)
                        {
                            effectData.origin = transform.position;
                        }
                    }
                }
            }
            if (useAim)
            {
                effectData.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
            }

            //Spawn Effect
            EffectManager.SpawnEffect(effectPrefab, effectData, transmit);
        }

        public static GameObject GetVFXDynamic(string vfxString, int skinIndex)
        {
            GameObject effectPrefab = null;

            string skinName = GetSkinNameVFX(skinIndex);

            //Search table for matching key, otherwise use default skin
            if (Asset.vfxPrefabs.ContainsKey($"{skinName}_{vfxString}"))
            {
                effectPrefab = Asset.vfxPrefabs[$"{skinName}_{vfxString}"];
            }
            else if (Asset.vfxPrefabs.ContainsKey($"Jhin_{vfxString}")) //Redunant ContainsKey check, but neccessary to prevent NREs stopping future code
            {
                effectPrefab = Asset.vfxPrefabs[$"Jhin_{vfxString}"];
            }

            return effectPrefab;
        }

        public static GameObject GetVFXDynamic(string vfxString, GameObject player)
        {
            int skinIndex = (int)player.GetComponent<CharacterBody>().skinIndex;
            return GetVFXDynamic(vfxString, skinIndex);
        }



        public static GameObject SpawnVFXDynamic(string vfxString, GameObject player, bool shouldParent)
        {
            var prefab = GetVFXDynamic(vfxString, player);

            if (prefab == null) { return null; }

            if (shouldParent)
            {
                return UnityEngine.Object.Instantiate<GameObject>(prefab, player.transform);
            }
            else
            {
                return UnityEngine.Object.Instantiate<GameObject>(prefab, player.transform.position, player.transform.rotation);
            }
        }

        public static GameObject SpawnVFXDynamic(string vfxString, int index, Transform parent)
        {
            var prefab = GetVFXDynamic(vfxString, index);

            if (prefab == null) { return null; }

            if (parent)
            {
                return UnityEngine.Object.Instantiate<GameObject>(prefab, parent);
            }
            else
            {
                return UnityEngine.Object.Instantiate<GameObject>(prefab);
            }
        }

        public static string GetSkinName(int skinIndex)
        {
            var index = skinIndex;

            //var skinName = ((Config.SkinOptions)index).ToString("f");
            //if (string.IsNullOrEmpty(skinName)) return "Jhin";
            //else return skinName;

            //returns skin String based on Enum found in Modules.Config
            //if we don't get a match, just use the base name
            //unimplemented ones are commented out because SFX cannot determine whether or not it exists in the soundbank
            //we simply ignore them
            //VFX doesn't care because it CAN check if they exist before calling them
            switch (index) 
            {
                case 0: return "Jhin";
                case 1: return "HighNoonJhin";
                case 2: return "BloodMoonJhin";
                case 3: return "SKTT1Jhin";
                case 4: return "ProjectJhin";
                case 5: return "ShanHaiJhin";
                case 6: return "DWGJhin";
                case 7: return "EmpyreanJhin";
                case 8: return "SoulFighterJhin";
                default: return "Jhin";
            }
        }

        public static string GetSkinNameSFX(int skinIndex)
        {
            var index = skinIndex;
            if (Config.sfxOverride.Value != Config.SkinOptions.Dynamic) index = (int)Config.sfxOverride.Value;

            return GetSkinName(index);
        }

        public static string GetSkinNameVFX( int skinIndex)
        {
            var index = skinIndex;
            if (Config.vfxOverride.Value != Config.SkinOptions.Dynamic) index = (int)Config.vfxOverride.Value;

            return GetSkinName(index);
        }
    }
}