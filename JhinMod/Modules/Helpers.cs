using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JhinMod.Modules
{
    internal static class Helpers
    {
        internal const string agilePrefix = "<style=cIsUtility>Agile.</style> ";
        internal const string stunningPrefix = "<style=cIsUtility>Stunning.</style> ";
        internal const string executingPrefix = "<style=cIsUtility>Executing.</style> ";

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
        public static float GetParabolaHeight( float totalDistance, float currentDistance)
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
    }
}