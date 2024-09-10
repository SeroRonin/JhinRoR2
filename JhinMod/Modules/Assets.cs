using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;
using IL.RoR2.Orbs;
using RoR2.Orbs;
using JhinMod.Content.Components;
using static RoR2.VFXAttributes;

namespace JhinMod.Modules
{
    internal static class Asset
    {
        #region Henry's Stuff
        // particle effects
        internal static GameObject swordSwingEffect;
        internal static GameObject swordHitImpactEffect;
        internal static GameObject bombExplosionEffect;

        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;
        #endregion

        #region Jhin's Stuff
        // particle effects

        //Base
        internal static GameObject baseDeadlyFlourishBeamEffect;
        internal static GameObject baseDeadlyFlourishMuzzleEffect;
        internal static GameObject baseGrenadeGhost;

        //Project
        internal static GameObject projectMaskEffect;
        internal static GameObject projectMuzzleflashEffect;
        internal static GameObject projectMuzzleflashFourthEffect;
        internal static GameObject projectTracerEffect;
        internal static GameObject projectTracerFourthEffect;
        internal static GameObject projectDeadlyFlourishMuzzleEffect;
        internal static GameObject projectGrenadeGhost;
        internal static GameObject projectGrenadeImpactEffect;
        internal static GameObject projectGrenadeImpactKillEffect;
        internal static GameObject projectUltModelEffect;

        //DWG
        internal static GameObject DWGUltModelEffect;

        //Dynamic VFX Prefab Tables
        public static Dictionary<string, GameObject> vfxPrefabs = new Dictionary<string, GameObject>{};

        // networked hit sounds
        #endregion

        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        private const string assetbundleName = "jhinassetbundle";
        private const string csProjName = "JhinMod";
        
        internal static void Initialize()
        {
            if (assetbundleName == "myassetbundle")
            {
                Log.Error("AssetBundle name hasn't been changed. not loading any assets to avoid conflicts");
                return;
            }

            LoadAssetBundle();
            LoadSoundbank();
            PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            try
            {
                if (mainAssetBundle == null)
                {
                    using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.{assetbundleName}"))
                    {
                        mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to load assetbundle. Make sure your assetbundle name is setup correctly\n" + e);
                return;
            }
        }

        internal static void LoadSoundbank()
        {
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.JhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.HighNoonJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.BloodMoonJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.SKTT1JhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.ProjectJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.ShanHaiJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.DWGJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.EmpyreanJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{csProjName}.SoundBanks.SoulFighterJhinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        internal static void PopulateAssets()
        {
            if (!mainAssetBundle)
            {
                Log.Error("There is no AssetBundle to load assets from.");
                return;
            }

            // feel free to delete everything in here and load in your own assets instead
            // it should work fine even if left as is- even if the assets aren't in the bundle
            
            swordHitSoundEvent = CreateNetworkSoundEventDef("JhinSwordHit");

            bombExplosionEffect = LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (bombExplosionEffect)
            {
                ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.5f;
                shakeEmitter.radius = 200f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 1f,
                    frequency = 40f,
                    cycleOffset = 0f
                };
            }

            //Base
            baseDeadlyFlourishBeamEffect = Asset.LoadEffect("Jhin_Base_DeadlyFlourishBeam", false);
            baseDeadlyFlourishMuzzleEffect = Asset.LoadEffect("Jhin_Base_DeadlyFlourishMuzzleFX", false);
            vfxPrefabs.Add("Jhin_DeadlyFlourishBeam", baseDeadlyFlourishBeamEffect);
            vfxPrefabs.Add("Jhin_DeadlyFlourishMuzzle", baseDeadlyFlourishMuzzleEffect);

            baseGrenadeGhost = Asset.CreateDancingGrenadeGhost("JhinGrenadeGhost");
            vfxPrefabs.Add("Jhin_Grenade", baseGrenadeGhost);

            //REPLACE, using commando muzzlefalsh
            vfxPrefabs.Add("Jhin_MuzzleFlash", EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab);

            //Project
            BindPairLocator projectMaskBPL;
            projectMaskEffect = CreateBindPairEffect("Jhin_Project_MaskVFX", out projectMaskBPL);
            var glitchComp = projectMaskEffect.AddComponent<ProjectMaskGlitchFX>();
            glitchComp.maskRenderer = projectMaskEffect.GetComponent<MeshRenderer>();
            projectMaskBPL.AddBindPair("Root", "Head");
            vfxPrefabs.Add("ProjectJhin_ModelFX", projectMaskEffect);

            projectMuzzleflashEffect = Asset.LoadEffect("Jhin_Project_Muzzleflash", false);
            projectMuzzleflashFourthEffect = Asset.LoadEffect("Jhin_Project_MuzzleflashFourth", false);
            vfxPrefabs.Add("ProjectJhin_MuzzleFlash", projectMuzzleflashEffect);
            vfxPrefabs.Add("ProjectJhin_MuzzleFlashFourth", projectMuzzleflashFourthEffect);

            projectTracerEffect = Asset.CreateTracerEffect("Jhin_Project_Tracer");
            projectTracerFourthEffect = Asset.CreateTracerEffect("Jhin_Project_TracerFourth");
            vfxPrefabs.Add("ProjectJhin_Tracer", projectTracerEffect);
            vfxPrefabs.Add("ProjectJhin_TracerFourth", projectTracerFourthEffect);

            projectDeadlyFlourishMuzzleEffect = Asset.LoadEffect("Jhin_Project_DeadlyFlourish_MuzzleFX");
            vfxPrefabs.Add("ProjectJhin_DeadlyFlourishMuzzle", projectDeadlyFlourishMuzzleEffect);

            projectGrenadeGhost = Asset.CreateDancingGrenadeGhost("ProjectJhinGrenadeGhost");
            projectGrenadeImpactEffect = Asset.LoadEffect("Jhin_Project_GrenadeImpact");
            projectGrenadeImpactKillEffect = Asset.LoadEffect("Jhin_Project_GrenadeImpactKill");
            vfxPrefabs.Add("ProjectJhin_GrenadeImpact", projectGrenadeImpactEffect);
            vfxPrefabs.Add("ProjectJhin_Grenade", projectGrenadeGhost);
            vfxPrefabs.Add("ProjectJhin_GrenadeImpactKill", projectGrenadeImpactKillEffect);

            BindPairLocator projectUltBPL;
            projectUltModelEffect = Asset.CreateBindPairEffect("Jhin_Project_UltModelFX", out projectUltBPL);
            projectUltBPL.AddBindPair("Root","Barrel");
            vfxPrefabs.Add("ProjectJhin_UltModelFX", projectUltModelEffect);

            //DWG
            BindPairLocator dwgUltBPL;
            DWGUltModelEffect = Asset.CreateBindPairEffect("Jhin_DWG_UltModelFX", out dwgUltBPL);
            dwgUltBPL.AddBindPair("Root", "Spine2");
            vfxPrefabs.Add("DWGJhin_UltModelFX", DWGUltModelEffect);

            //Henry Leftover
            swordSwingEffect = Asset.LoadEffect("JhinSwordSwingEffect", true);
            swordHitImpactEffect = Asset.LoadEffect("ImpactJhinSlash");
        }

        private static GameObject CreateBindPairEffect( string effectName, out BindPairLocator bindPairLocator )
        {
            var prefab = mainAssetBundle.LoadAsset<GameObject>( effectName );
            bindPairLocator = prefab.AddComponent<BindPairLocator>();

            return prefab;
        }

        private static GameObject CreateDancingGrenadeGhost(string resourceName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Log.Error("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            string path = "Prefabs/Effects/OrbEffects/HuntressGlaiveOrbEffect";
            GameObject huntressEffect = LegacyResourcesAPI.Load<GameObject>(path).InstantiateClone("DancingGrenadeEffect");

            VFXAttributes newEffectVFX = newEffect.AddComponent<VFXAttributes>();
            newEffectVFX.vfxPriority = VFXAttributes.VFXPriority.Always;
            newEffectVFX.DoNotPool = true;
            newEffect.AddComponent<BounceVisualizer>();
            newEffect.AddComponent<EffectComponent>(huntressEffect.GetComponent<EffectComponent>());
            newEffect.AddComponent<RoR2.Orbs.OrbEffect>(huntressEffect.GetComponent<RoR2.Orbs.OrbEffect>());

            var rotateComponent = newEffect.transform.GetChild(0).gameObject.AddComponent<RotateObject>();
            rotateComponent.rotationSpeed = new Vector3(360, 100, 20);

            AddNewEffectDef(newEffect, "");

            return newEffect;
        }

        private static GameObject CreateTracerEffect(string resourceName, string soundName = null, bool parentToTransform = false)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Log.Error("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            newEffect.AddComponent<CustomTracer>();
            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (Renderer i in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                i?.material?.SetHopooMaterial();
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }


        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = mainAssetBundle.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Log.Error("Trying to load a null model- check to see if the BodyName in your code matches the prefab name of the object in Unity\nFor Example, if your prefab in unity is 'mdlJhin', then your BodyName must be 'Jhin'");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Log.Error("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            VFXAttributes newEffectVFX = newEffect.AddComponent<VFXAttributes>();
            newEffectVFX.vfxPriority = VFXAttributes.VFXPriority.Always;
            newEffectVFX.DoNotPool = true;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            Modules.Content.AddEffectDef(newEffectDef);
        }
    }
}