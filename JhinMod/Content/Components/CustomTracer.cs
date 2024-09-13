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
        public float maxDistance = 1000f;
        public float projectileSpeed = 100f;

        public Vector3 origin = new Vector3();
        public Vector3 target = new Vector3();
        public Vector3 moveVector = new Vector3();
        public Vector3 velocity = new Vector3();

        public float timeActive;
        public bool isActive = true;

        private void Awake()
        {
            UpdateTracer();
        }

        private void Start()
        {
            UpdateTracer();
        }

        public virtual void UpdateTracer()
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

            // Destroy self if distance reached
            var distanceTravelled = Vector3.Distance(gameObject.transform.position, origin);
            if (distanceTravelled > maxDistance)
            {
                Destroy(gameObject, 5f);
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
