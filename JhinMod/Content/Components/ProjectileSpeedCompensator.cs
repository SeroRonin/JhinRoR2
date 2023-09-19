using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using JhinMod.Modules;
using R2API.Utils;
using RoR2.Projectile;
using System.Linq;
using RoR2;

namespace JhinMod.Content.Components
{
    /// <summary>
    /// This component checks between our last known position and our current position
    /// If there is an enemy we skipped over, explode at that location instead
    /// </summary>
    public class ProjectileSpeedCompensator : MonoBehaviour
    {
        ProjectileSimple simpleComponent;
        ProjectileController controllerComponent;
        ProjectileImpactExplosion explosionComponent;
        public Vector3 previousPos;

        public float initialDistance;
        public float projectileSpeed;

        private void Awake()
        {

        }

        private void Start()
        {
            simpleComponent = this.GetComponent<ProjectileSimple>();
            controllerComponent = this.GetComponent<ProjectileController>();
            explosionComponent = this.GetComponent<ProjectileImpactExplosion>();

            projectileSpeed = simpleComponent.desiredForwardSpeed;
            previousPos = this.transform.position;
        }

        private void LateUpdate()
        {
            RaycastHit[] array = Physics.RaycastAll(previousPos, this.transform.forward, (projectileSpeed * Time.deltaTime), RoR2.LayerIndex.entityPrecise.mask, QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in array) 
            {
                if (hit.collider == null) continue;
                if ( !hit.collider.GetComponent<HurtBox>() ) continue;
                if ( !hit.collider.GetComponent<HurtBox>().healthComponent ) continue;

                this.transform.position = array.FirstOrDefault().transform.position;
                break;
            }

            previousPos = this.transform.position;
        }
    }
}
