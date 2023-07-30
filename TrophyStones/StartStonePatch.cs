using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using UnityEngine;

namespace TrophyStones;

public class StartStonePatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public static class StartStonePatcher
    {
        public static void Postfix(ZNetScene __instance)
        {
            if (!__instance) return;

            GameObject StartStone = __instance.GetPrefab("StartStone");
            if (!StartStone) {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get start stone");
                return;
            }
            var pieceScript = StartStone.GetComponent<Piece>();
            var wearNTearScript = StartStone.GetComponent<WearNTear>();
            var BossStoneScript = StartStone.GetComponent<BossStone>();
            var itemStandScript = StartStone.GetComponent<ItemStand>();
            
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
            // GET BOSS STONE DATA
            GameObject BossStoneBonemass = __instance.GetPrefab("BossStone_Bonemass");
            if (!BossStoneBonemass)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get boss stone bonemass");
                return;
            }
            TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "got boss stone bonemass");

            var originalBossStoneScript = BossStoneBonemass.GetComponent<BossStone>();
            TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "got boss stone script");
            EffectList.EffectData[] step1EffectPrefabs = originalBossStoneScript.m_activateStep1.m_effectPrefabs;
            EffectList.EffectData[] step2EffectPrefabs = originalBossStoneScript.m_activateStep2.m_effectPrefabs;
            EffectList.EffectData[] step3EffectPrefabs = originalBossStoneScript.m_activateStep3.m_effectPrefabs;
            string completedMessage = "Skjálf releases her power";
            var bonemassItemStand = BossStoneBonemass.GetComponentInChildren<ItemStand>();
            if (!bonemassItemStand)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get item stand data from bonemass");
                return;
            }
            EffectList.EffectData[] itemStandEffectsData = bonemassItemStand.m_effects.m_effectPrefabs;
            TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, $"item stand effect data count {itemStandEffectsData.Length}");
            // GET ITEM STAND DATA
            GameObject swordForsaken = __instance.GetPrefab("SwordForsaken_RS");
            if (!swordForsaken)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get forsaken sword");
                return;
            }
            var swordForsakenItemDropScript = swordForsaken.GetComponent<ItemDrop>();

            var itemStandSupportedItems = new List<ItemDrop> { swordForsakenItemDropScript };
            GameObject goblinShaman = __instance.GetPrefab("GoblinShaman");
            if (!goblinShaman)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get goblin shaman");
                return;
            }

            var goblinShamanHumanoidScript = goblinShaman.GetComponent<Humanoid>();
            var goblinShamanDefaultItems = goblinShamanHumanoidScript.m_defaultItems;
            var goblinShamanProtectPrefab = goblinShamanDefaultItems[0];
            if (!goblinShamanProtectPrefab)
            {
                TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "failed to get protect prefab");
                return;
            }
            var ProtectPrefabItemDropScript = goblinShamanProtectPrefab.GetComponent<ItemDrop>();
            var ProtectStatusEffect = ProtectPrefabItemDropScript.m_itemData.m_shared.m_attackStatusEffect;
            var bonemassItemStandScript = BossStoneBonemass.GetComponentInChildren<ItemStand>();
            var activatePowerEffects = bonemassItemStandScript.m_activatePowerEffects.m_effectPrefabs;
            var activatePowerEffectsPlayerData = bonemassItemStandScript.m_activatePowerEffectsPlayer.m_effectPrefabs;
            // SET PIECE DATA
            pieceScript.m_placeEffect.m_effectPrefabs = placementEffectsData;
            // SET WEAR N TEAR DATA
            wearNTearScript.m_destroyedEffect.m_effectPrefabs = destroyedEffectList;
            wearNTearScript.m_hitEffect.m_effectPrefabs = hitEffectList;
            wearNTearScript.m_switchEffect.m_effectPrefabs = switchEffectList;
            // SET BOSS STONE DATA
            BossStoneScript.m_activateStep1.m_effectPrefabs = step1EffectPrefabs;
            BossStoneScript.m_activateStep2.m_effectPrefabs = step2EffectPrefabs;
            BossStoneScript.m_activateStep3.m_effectPrefabs = step3EffectPrefabs;
            BossStoneScript.m_completedMessage = completedMessage;
            // CONFIGURE STATUS EFFECTS
            EffectList.EffectData fxGP_Activation = bonemassItemStandScript.m_guardianPower.m_startEffects.m_effectPrefabs[0];
            EffectList.EffectData ShieldEffect = ProtectStatusEffect.m_startEffects.m_effectPrefabs[0];
            ShieldEffect.m_scale = true;
            
            EffectList.EffectData[] newStartEffects = new EffectList.EffectData[2]
            {
                fxGP_Activation,
                ShieldEffect
            };
            
            ProtectStatusEffect.m_startEffects.m_effectPrefabs = newStartEffects;
            
            ProtectStatusEffect.m_name = "Skjálf Protection";
            ProtectStatusEffect.m_icon = swordForsaken.GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
            ProtectStatusEffect.m_cooldown = 100f;
            ProtectStatusEffect.m_tooltip = "Absorbs 100 points of damage";
            ProtectStatusEffect.m_flashIcon = true;
            ProtectStatusEffect.m_activationAnimation = "gpower";
            ProtectStatusEffect.m_ttl = 50;
            ProtectStatusEffect.m_startMessage = "Skjálf Protection";
            ProtectStatusEffect.m_stopMessage = "Skjálf Protection Fades";
            
            // SET ITEM STAND DATA
            itemStandScript.m_supportedItems = itemStandSupportedItems;
            itemStandScript.m_effects.m_effectPrefabs = itemStandEffectsData;
            itemStandScript.m_guardianPower = ProtectStatusEffect;
            itemStandScript.m_activatePowerEffects.m_effectPrefabs = activatePowerEffects;
            itemStandScript.m_activatePowerEffectsPlayer.m_effectPrefabs = activatePowerEffectsPlayerData;
            
            TrophyStonesPlugin.TrophyStonesLogger.Log(LogLevel.Warning, "successfully patched start stone");
        }
    }
}