using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrophyStones;

public class CustomOfferAltarPatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public static class CustomOfferAltarPatcher
    {
        public static void Postfix(ZNetScene __instance)
        {
            if (!__instance) return;
            
            GameObject CustomOfferAltar = __instance.GetPrefab("CustomOfferAltar");
            if (!CustomOfferAltar)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get custom offer altar");
                return;
            }
            
            var smelterScript = CustomOfferAltar.GetComponent<Smelter>();
            var pieceScript = CustomOfferAltar.GetComponent<Piece>();
            var wearNTearScript = CustomOfferAltar.GetComponent<WearNTear>();
            if (!wearNTearScript)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get wear n tear script");
                return;
            }
            
            // GET SMELTER DATA
            GameObject sfxWishbonePing = __instance.GetPrefab("sfx_WishbonePing_far");
            GameObject vfxBowlAddItem = __instance.GetPrefab("vfx_bowl_AddItem");

            EffectList.EffectData sfxWishbone = new EffectList.EffectData
            {
                m_prefab = sfxWishbonePing,
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData vfxAddItem = new EffectList.EffectData
            {
                m_prefab = vfxBowlAddItem,
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            EffectList.EffectData[] OreAddEffects = new EffectList.EffectData[2]
            {
                sfxWishbone,
                vfxAddItem
            };

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
            EffectList.EffectData[] switchEffectList = new EffectList.EffectData[1] { vfxPlaceThrone02 };
            
            // SET PIECE DATA
            pieceScript.m_placeEffect.m_effectPrefabs = placementEffectsData;
            // SET WEAR N TEAR DATA
            wearNTearScript.m_destroyedEffect.m_effectPrefabs = destroyedEffectList;
            wearNTearScript.m_hitEffect.m_effectPrefabs = hitEffectList;
            wearNTearScript.m_switchEffect.m_effectPrefabs = switchEffectList;
            // SET SMELTER DATA
            Smelter.ItemConversion neckTrophyData = createConversionData(__instance, "TrophyNeck", "NeckTail");
            Smelter.ItemConversion boarTrophyData = createConversionData(__instance, "TrophyBoar", "LeatherScraps");
            Smelter.ItemConversion deerTrophyData = createConversionData(__instance, "TrophyDeer", "DeerHide");
            List<Smelter.ItemConversion> conversionData = new List<Smelter.ItemConversion>
            {
                neckTrophyData,
                boarTrophyData,
                deerTrophyData
            };
            
            smelterScript.m_conversion = conversionData;
            smelterScript.m_oreAddedEffects.m_effectPrefabs = OreAddEffects;
            smelterScript.m_produceEffects.m_effectPrefabs = OreAddEffects;
        }

        private static Smelter.ItemConversion createConversionData(ZNetScene scene, string fromObjName, string toObjName)
        {
            ItemDrop fromItemDrop = scene.GetPrefab(fromObjName).GetComponent<ItemDrop>();
            ItemDrop toItemDrop = scene.GetPrefab(toObjName).GetComponent<ItemDrop>();

            Smelter.ItemConversion conversionData = new Smelter.ItemConversion
            {
                m_from = fromItemDrop,
                m_to = toItemDrop
            };
            return conversionData;
        }
    }
}