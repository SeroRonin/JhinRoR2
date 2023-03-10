using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using JhinMod.Modules;
using R2API.Utils;

namespace JhinMod.Content.Components
{
    public class BounceVisualizer : MonoBehaviour
    {
        public float initialDistance;
        public float projectileSpeed;
        public float timeActive;
        public bool isActive;

        private void Awake()
        {
        }

        private void Start()
        {
            timeActive = 0f;
        }

        private void Update()
        {
            if ( !isActive )
            {
                return;
            }

            timeActive += Time.deltaTime;

            var distanceTravelled = initialDistance - (projectileSpeed * timeActive);
            ChatMessage.Send($"initial {initialDistance}, speed {projectileSpeed}, travelled {distanceTravelled}");
            var y = Helpers.GetParabolaHeight(initialDistance, distanceTravelled);

            gameObject.transform.GetChild(0).localPosition = new Vector3(0, y, 0);
        }
    }
}
