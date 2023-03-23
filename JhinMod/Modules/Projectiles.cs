using JhinMod.Content.Components;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace JhinMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject ultMissilePrefab;

        internal static void RegisterProjectiles()
        {
            CreateBomb();
            CreateRocket();

            AddProjectile(bombPrefab);
            AddProjectile(ultMissilePrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }


        //CREDIT BASE: ROCKET SURVIVOR
        private static void CreateRocket()
        {
            //REPLACE, using default assets
            GameObject rocketPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotGrenadeLauncherProjectile.prefab").WaitForCompletion().InstantiateClone("JhinUltProjectile", true);//"RoR2/Base/Drones/PaladinRocket.prefab"

            ProjectileSimple ps = rocketPrefab.GetComponent<ProjectileSimple>();
            ps.desiredForwardSpeed = 150f;
            ps.lifetime = 20f;

            //ProjectileImpactExplosion pie = rocketPrefab.GetComponent<ProjectileImpactExplosion>();
            ProjectileImpactExplosion[] impactEvents = rocketPrefab.GetComponentsInChildren<ProjectileImpactExplosion>();
            for (int i = 0; i < impactEvents.Length; i++)
            {
                UnityEngine.Object.Destroy(impactEvents[i]);
            }
            CustomProjectileImpactExplosion custompie = rocketPrefab.AddComponent<CustomProjectileImpactExplosion>();
            InitializeImpactExplosion(custompie);

            //REPLACE, using default assets
            GameObject explosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion().InstantiateClone("JhinUltExplosionVFX", false);
            EffectComponent ec = explosionEffect.GetComponent<EffectComponent>();
            ec.soundName = "Play_Seroronin_Jhin_UltHit"; //INVESTIGATE, make this sound change based on last shot
            Modules.Content.AddEffectDef(new EffectDef(explosionEffect));
            //EntityStates.RocketSurvivorSkills.Primary.FireRocket.explosionEffectPrefab = explosionEffect;

            custompie.blastDamageCoefficient = 1f;
            custompie.blastRadius = 8f;
            custompie.destroyOnEnemy = true;
            custompie.destroyOnWorld = true;
            custompie.lifetime = 12f;
            custompie.impactEffect = explosionEffect;
            custompie.timerAfterImpact = false;
            custompie.lifetimeAfterImpact = 0f;
            custompie.blastAttackerFiltering = AttackerFiltering.NeverHitSelf;
            custompie.falloffModel = BlastAttack.FalloffModel.Linear;

            //Remove built-in sounds
            AkEvent[] akEvents = rocketPrefab.GetComponentsInChildren<AkEvent>();
            for (int i = 0; i < akEvents.Length; i++)
            {
                UnityEngine.Object.Destroy(akEvents[i]);
            }
            AkGameObj akgo = rocketPrefab.GetComponent<AkGameObj>();
            if (akgo)
            {
                UnityEngine.Object.Destroy(akgo);
            }

            /*
            rocketPrefab.AddComponent<AddToRocketTrackerComponent>();
            BlastJumpComponent bjc = rocketPrefab.AddComponent<BlastJumpComponent>();
            bjc.force = 2000f;
            bjc.horizontalMultiplier = 1.5f;
            bjc.aoe = 8f;
            bjc.requireAirborne = true;
            */

            /*
            DamageAPI.ModdedDamageTypeHolderComponent mdc = rocketPrefab.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            mdc.Add(DamageTypes.ScaleForceToMass);
            mdc.Add(DamageTypes.SweetSpotModifier);
            */

            ultMissilePrefab = rocketPrefab;

            /*
            EntityStates.RocketSurvivorSkills.Primary.FireRocket.projectilePrefab = rocketPrefab;
            */
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "JhinBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("JhinBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("JhinBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("JhinBombGhost");
            bombController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        public static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}