using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RoR2;

namespace JhinMod.Modules
{
    internal static class Helpers
    {
        internal const string agilePrefix = "<style=cIsUtility>Agile.</style> ";
        internal const string stunningPrefix = "<style=cIsUtility>Stunning.</style> ";
        internal const string executingPrefix = "<style=cIsUtility>Executing.</style> ";

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
        public static void PlaySoundDynamic(string soundID, GameObject player, GameObject parent = null)
        {
            if ( parent == null) parent = player;
            int skinIndex = (int)player.GetComponent<CharacterBody>().skinIndex;
            Util.PlaySound($"Play_Seroronin_{GetSkinName(skinIndex)}_{soundID}", parent);
        }
        public static void StopSoundDynamic(string soundID, GameObject player, GameObject parent = null)
        {
            if (parent == null) parent = player;
            int skinIndex = (int)player.GetComponent<CharacterBody>().skinIndex;
            Util.PlaySound($"Stop_Seroronin_{GetSkinName(skinIndex)}_{soundID}", parent);
        }
        public static string GetSkinName(int skinIndex)
        {
            var index = skinIndex;
            if (Config.sfxChoice.Value != Config.SFXChoice.SkinDependent) index = (int)Config.sfxChoice.Value;

            //returns skin String based on Enum found in Modules.Config
            //if we don't get a match, just use the base name

            if (index == 1) return "HighNoonJhin";
            if (index == 2) return "BloodMoonJhin";
            //if (index == 3) return "SKTT1Jhin";
            if (index == 4) return "ProjectJhin";
            //if (index == 5) return "ShanHaiJhin";
            //if (index == 6) return "DWGJhin";
            //if (index == 7) return "EmpyreanJhin";
            return "Jhin";
        }
    }
}