using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using JhinMod.Modules;
using R2API.Utils;

namespace JhinMod.Content.Components
{
    /// <summary>
    /// Component that creates moving VFX objects to replicate tracers
    /// </summary>
    public class CustomTracer : MonoBehaviour
    {
        public float initialDistance;
        public float projectileSpeed;
        public float timeActive;

        public float maxDistance = 1000f;
        public float maxLifetime = 50f;

        public bool isActive;

        public Vector3 origin;
        public Vector3 target;
        public Vector3 moveVector;
        public Vector3 velocity;

        private void Awake()
        {
        }

        private void Start()
        {
            timeActive = 0f;
            moveVector = (target - origin).normalized;
            velocity = moveVector * projectileSpeed;
        }

        private void Update()
        {
            if ( !isActive )
            {
                return;
            }

            timeActive += Time.deltaTime;

            // Destroy self if max life or distance reached
            if (timeActive > maxLifetime)
            {
                Destroy(gameObject, 2f);
                StopParticles();
            }

            var distanceTravelled = Vector3.Distance(gameObject.transform.position, origin);
            if (distanceTravelled > maxDistance)
            {
                Destroy(gameObject, 2f);
                StopParticles();
            }
            else
                TickVelocity();
        }
        public virtual void TickVelocity()
        {
            gameObject.transform.position += velocity * Time.deltaTime;
        }
        
        public virtual void StopParticles()
        {
            ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
            foreach ( var system in systems ) 
            {
                system.Stop(true);
            }
        }
    }
}
