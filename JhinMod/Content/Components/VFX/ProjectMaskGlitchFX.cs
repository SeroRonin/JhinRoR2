using System;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Component that animates Project Jhin's mask
    /// </summary>
    public class ProjectMaskGlitchFX : MonoBehaviour
    {
        [SerializeField]
        public MeshRenderer maskRenderer;

        [SerializeField]
        public float minimumStableTime = 2;
        [SerializeField]
        public float maximumStableTime = 15;

        [SerializeField]
        public float timeBetweenAttempts = 0.2f;

        [SerializeField]
        public float maskFrameHoldMin = 0.1f;
        [SerializeField]
        public float maskFrameHoldMax = 0.3f;
        public float maskFrameHoldTime = 0.2f;

        [SerializeField]
        public float glitchFrameHoldMin = 0.1f;
        [SerializeField]
        public float glitchFrameHoldMax = 0.3f;
        public float glitchFrameHoldTime = 0.2f;

        public float timeSinceTry;
        public float timeSinceMask;
        public float timeSinceGlitch;

        [SerializeField]
        public float maskGlitchChance = 5f;
        [SerializeField]
        public float maskGlitchMultipleChance = 30f;

        public int maskFrame = 0;
        public int glitchActive = 0;

        public List<int> possibleMaskFrames = new List<int> { 1, 2, 3 };

        private void Awake()
        {
        }

        private void Start()
        {

        }

        private void Update()
        {
            timeSinceTry += Time.deltaTime;
            timeSinceMask += Time.deltaTime;
            timeSinceGlitch += Time.deltaTime;
            if (timeSinceTry < timeBetweenAttempts)
            {
                return;
            }

            // Current frame is a glitch frame, roll to see if we glitch multiple times
            if (maskFrame != 0)
            {
                if (timeSinceMask > maskFrameHoldTime)
                {
                    RollMask(maskGlitchMultipleChance, maskFrame);
                }
            }

            if (timeSinceMask > minimumStableTime)
            {
                if (timeSinceMask > maskFrameHoldTime)
                {
                    RollMask( maskGlitchChance );
                }

            }

            if (glitchActive != 0)
            {
                if (timeSinceGlitch > glitchFrameHoldTime)
                {
                    RollGlitchOverlay( maskGlitchMultipleChance );
                }
            }
            if (timeSinceGlitch > minimumStableTime)
            {
                if (timeSinceGlitch > glitchFrameHoldTime)
                {
                    RollGlitchOverlay( maskGlitchChance );
                }
            }

            timeSinceTry = 0;
        }

        public void RollMask(float chance, int avoidFrame = 0)
        {
            var randomPercent = UnityEngine.Random.Range(0, 100);
            if (randomPercent <= chance)
            {
                var possibleFrames = new List<int>(possibleMaskFrames);

                if (avoidFrame != 0)
                {
                    possibleFrames.Remove(avoidFrame);
                }

                var possibleFrameCount = possibleFrames.Count;
                var randomInt = UnityEngine.Random.Range(0, possibleFrameCount);
                maskFrame = possibleFrames[randomInt];

                maskFrameHoldTime = UnityEngine.Random.Range(maskFrameHoldMin, maskFrameHoldMax);

                timeSinceMask = 0;
            }
            else
            {
                maskFrame = 0;
            }

            maskRenderer.materials[0].SetInt("_MaskFrame", maskFrame);
        }

        public void RollGlitchOverlay(float chance)
        {
            var randomPercent = UnityEngine.Random.Range(0, 100);
            if (randomPercent <= chance)
            {
                glitchActive = 1;

                var xOffset = UnityEngine.Random.Range(0, 1f);
                var yOffset = UnityEngine.Random.Range(0, 1f);
                maskRenderer.materials[0].SetVector("_GlitchOffset", new Vector2(xOffset,yOffset));

                glitchFrameHoldTime = UnityEngine.Random.Range(glitchFrameHoldMin, glitchFrameHoldMax);
                timeSinceGlitch = 0;
            }
            else
            {
                glitchActive = 0;
            }
            maskRenderer.materials[0].SetInt("_GlitchActive", glitchActive);
        }
    }
