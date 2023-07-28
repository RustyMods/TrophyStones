using HarmonyLib;
using UnityEngine;

namespace TrophyStones;

public class VegvisirPatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public static class VegvisirPatcher
    {
        public static void Postfix(ZNetScene __instance)
        {
            if (!__instance) return;
            GameObject vegvisirHaldor = __instance.GetPrefab("Vegvisir_Haldor");
            if (!vegvisirHaldor) return;
            
            var pieceScript = vegvisirHaldor.GetComponent<Piece>();
            var wearNTearScript = vegvisirHaldor.GetComponent<WearNTear>();
            
            // GET VFX & SFX FOR PIECE PLACEMENT EFFECTS
            EffectList.EffectData vfxPlaceStoneWallData = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("vfx_Place_stone_wall_2x1"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData sfxBuildHammerStoneData = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("sfx_build_hammer_stone"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData[] placementEffectsData = new EffectList.EffectData[2]
            {
                vfxPlaceStoneWallData,
                sfxBuildHammerStoneData
            };
            
            // GET VFX & SFX FOR WEAR N TEAR SCRIPT
            EffectList.EffectData vfxRockDestroyed = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("vfx_RockDestroyed"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData sfxRockDestroyed = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("sfx_rock_destroyed"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData[] destroyedEffectList = new EffectList.EffectData[2]
            {
                vfxRockDestroyed,
                sfxRockDestroyed
            };
            EffectList.EffectData vfxRockHit = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("vfx_RockHit"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData sfxRockHit = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("sfx_rock_hit"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData[] hitEffectList = new EffectList.EffectData[2]
            {
                vfxRockHit,
                sfxRockHit
            };
            EffectList.EffectData vfxPlaceThrone02 = new EffectList.EffectData
            {
                m_prefab = __instance.GetPrefab("vfx_Place_throne02"),
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData[] switchEffectList = new EffectList.EffectData[1]
            {
                vfxPlaceThrone02
            };
            pieceScript.m_placeEffect.m_effectPrefabs = placementEffectsData;
            wearNTearScript.m_destroyedEffect.m_effectPrefabs = destroyedEffectList;
            wearNTearScript.m_hitEffect.m_effectPrefabs = hitEffectList;
            wearNTearScript.m_switchEffect.m_effectPrefabs = switchEffectList;
        }
    }
}