using System;
using UnityEngine;

namespace JhinMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "Jhin is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

        internal const float swordDamageCoefficient = 2.8f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 16f;

        internal const float whisperDamageCoefficient = 8f;

        internal const float dancingGrenadeDamageCoefficient = 6f;

        internal const float dancingGrenadeBounceKillDamageCoefficient = 0.35f;

        internal const float deadlyFlourishDamageCoefficient = 5f;

        internal const float curtainCallDamageCoefficient = 16f;

        /// <summary>
        /// The percent of missing health Execute skills use as bonus damage
        /// </summary>
        internal const float executeMissingHealthDamagePercent = 0.3f;

        /// <summary>
        /// The maximum percentage of our damage that Execute skills can add as additional damage.
        /// </summary>
        internal const float executeDamagePercentCap = 1f;
    }
}