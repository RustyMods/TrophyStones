using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace TrophyStones;

public class BossStonePatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public static class BossStonePatcher
    {
        public static void Postfix(ZNetScene __instance)
        {
            if (!__instance) return;
            ClonePrefabData(__instance, "BossStone_Eikthyr_Buildable", "BossStone_Eikthyr");
            ClonePrefabData(__instance, "BossStone_TheElder_Buildable", "BossStone_TheElder");
            ClonePrefabData(__instance, "BossStone_Bonemass_Buildable", "BossStone_Bonemass");
            ClonePrefabData(__instance, "BossStone_DragonQueen_Buildable", "BossStone_DragonQueen");
            ClonePrefabData(__instance, "BossStone_Yagluth_Buildable", "BossStone_Yagluth");
            ClonePrefabData(__instance, "BossStone_TheQueen_Buildable", "BossStone_TheQueen");
        }

        private static void ClonePrefabData(
            ZNetScene scene, string buildablePrefabName, string originalPrefabName)
        {
            if (!scene) return;
            
            // GET ORIGINAL AND NEW PREFABS FROM ZNETSCENE
            GameObject newPrefab = scene.GetPrefab(buildablePrefabName);
            GameObject originalPrefab = scene.GetPrefab(originalPrefabName);
            if (!newPrefab || !originalPrefab) return;
            
            // GET ORIGINAL SCRIPTS
            BossStone originalPrefabBossStoneScript = originalPrefab.GetComponent<BossStone>();
            RuneStone originalPrefabRuneStoneScript = originalPrefab.GetComponent<RuneStone>();
            ItemStand originalPrefabItemStandScript = originalPrefab.GetComponentInChildren<ItemStand>();

            // GET NEW SCRIPTS
            BossStone newPrefabBossStoneScript = newPrefab.GetComponent<BossStone>();
            RuneStone newPrefabRuneStoneScript = newPrefab.GetComponent<RuneStone>();
            ItemStand newPrefabItemStandScript = newPrefab.GetComponentInChildren<ItemStand>();
            Piece newPrefabPieceScript = newPrefab.GetComponent<Piece>();
            WearNTear newPrefabWearNTearScript = newPrefab.GetComponent<WearNTear>();

            // GET ORIGINAL BOSS STONE DATA
            EffectList.EffectData[] step1 = originalPrefabBossStoneScript.m_activateStep1.m_effectPrefabs;
            EffectList.EffectData[] step2 = originalPrefabBossStoneScript.m_activateStep2.m_effectPrefabs;
            EffectList.EffectData[] step3 = originalPrefabBossStoneScript.m_activateStep3.m_effectPrefabs;
            string completedMessage = originalPrefabBossStoneScript.m_completedMessage;
            
            // GET ORIGINAL RUNE STONE DATA
            string runestoneName = originalPrefabRuneStoneScript.m_name;
            string runestoneDescription = originalPrefabRuneStoneScript.m_text;
            
            // GET ORIGINAL ITEM STAND DATA
            string itemStandName = originalPrefabItemStandScript.m_name;
            List<ItemDrop.ItemData.ItemType> itemStandSupportedTypes = originalPrefabItemStandScript.m_supportedTypes;
            List<ItemDrop> itemStandSuportedItems = originalPrefabItemStandScript.m_supportedItems;
            EffectList.EffectData[] itemStandEffects = originalPrefabItemStandScript.m_effects.m_effectPrefabs;
            StatusEffect guardianPower = originalPrefabItemStandScript.m_guardianPower;
            EffectList.EffectData[] itemStandActivatePowerEffects =
                originalPrefabItemStandScript.m_activatePowerEffects.m_effectPrefabs;
            EffectList.EffectData[] activatePowerEffectsPlayer = originalPrefabItemStandScript.m_activatePowerEffectsPlayer.m_effectPrefabs;
            
            // GET VFX & SFX FOR PIECE PLACEMENT EFFECTS
            EffectList.EffectData vfxPlaceStoneWallData = new EffectList.EffectData
            {
                m_prefab = scene.GetPrefab("vfx_Place_stone_wall_2x1"),
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
                m_prefab = scene.GetPrefab("sfx_build_hammer_stone"),
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
                m_prefab = scene.GetPrefab("vfx_RockDestroyed"),
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
                m_prefab = scene.GetPrefab("sfx_rock_destroyed"),
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
                m_prefab = scene.GetPrefab("vfx_RockHit"),
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
                m_prefab = scene.GetPrefab("sfx_rock_hit"),
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
                m_prefab = scene.GetPrefab("vfx_Place_throne02"),
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
            
            // OVERWRITE BOSS STONE DATA
            newPrefabBossStoneScript.m_activateStep1.m_effectPrefabs = step1;
            newPrefabBossStoneScript.m_activateStep2.m_effectPrefabs = step2;
            newPrefabBossStoneScript.m_activateStep3.m_effectPrefabs = step3;
            newPrefabBossStoneScript.m_completedMessage = completedMessage;
            
            // OVERWRITE RUNE STONE DATA
            newPrefabRuneStoneScript.m_name = runestoneName;
            newPrefabRuneStoneScript.m_text = runestoneDescription;
            
            // OVERWRITE ITEM STAND DATA
            newPrefabItemStandScript.name = itemStandName;
            newPrefabItemStandScript.m_supportedTypes = itemStandSupportedTypes;
            newPrefabItemStandScript.m_supportedItems = itemStandSuportedItems;
            newPrefabItemStandScript.m_effects.m_effectPrefabs = itemStandEffects;
            newPrefabItemStandScript.m_guardianPower = guardianPower;
            newPrefabItemStandScript.m_activatePowerEffects.m_effectPrefabs = itemStandActivatePowerEffects;
            newPrefabItemStandScript.m_activatePowerEffectsPlayer.m_effectPrefabs = activatePowerEffectsPlayer;
            
            // SET PIECE DATA
            newPrefabPieceScript.m_placeEffect.m_effectPrefabs = placementEffectsData;
            
            // SET WEAR N TEAR DATA
            newPrefabWearNTearScript.m_destroyedEffect.m_effectPrefabs = destroyedEffectList;
            newPrefabWearNTearScript.m_hitEffect.m_effectPrefabs = hitEffectList;
            newPrefabWearNTearScript.m_switchEffect.m_effectPrefabs = switchEffectList;
        }
    }

    [HarmonyPatch(typeof(ItemStand), nameof(ItemStand.UpdateAttach))]
    public static class ItemStandPatcher
    {
        static bool Prefix(ItemStand __instance)
        {
            // Create a new Piece.Requirement array and append items to it using a foreach loop
            var netViewOverrideName = __instance.m_netViewOverride.name;
            var pieceScript = __instance.m_netViewOverride.GetComponent<Piece>();
            
            if (netViewOverrideName == "BossStone_Eikthyr_Buildable(Clone)")
            {
                GameObject EikthyrTrophy = ZNetScene.instance.GetPrefab("TrophyEikthyr");
                Piece.Requirement EikthyrTrophyData = new Piece.Requirement
                {
                    m_resItem = EikthyrTrophy.GetComponent<ItemDrop>(),
                    m_amount = 1,
                    m_extraAmountOnlyOneIngredient = 0,
                    m_amountPerLevel = 0,
                    m_recover = true
                };

                var clonedResources = pieceScript.m_resources;

                List<Piece.Requirement> newResourceList = new(clonedResources) { EikthyrTrophyData };
                // If you need an array, you can convert the list back to an array
                Piece.Requirement[] newResourceArray = newResourceList.ToArray();

                pieceScript.m_resources = newResourceArray;
            }

            return true;
        }
        
        

        // {
        //     var netViewOverrideName = __instance.m_netViewOverride.name;
        //     var pieceScript = __instance.m_netViewOverride.GetComponent<Piece>();
        //     if (netViewOverrideName == "BossStone_Eikthyr_Buildable(Clone)")
        //     {
        //         GameObject EikthyrTrophy = ZNetScene.instance.GetPrefab("TrophyEikthyr");
        //         Piece.Requirement EikthyrTrophyData = new Piece.Requirement
        //         {
        //             m_resItem = EikthyrTrophy.GetComponent<ItemDrop>(),
        //             m_amount = 1,
        //             m_extraAmountOnlyOneIngredient = 0,
        //             m_amountPerLevel = 0,
        //             m_recover = true
        //         };
        //         //
        //         var clonedResources = pieceScript.m_resources;
        //         Piece.Requirement[] newResourceList = new Piece.Requirement[3]; 
        //         foreach (var resource in clonedResources)
        //         {
        //             newResourceList.AddToArray(resource);
        //         }
        //         newResourceList.AddToArray(EikthyrTrophyData);
        //         
        //         //
        //         Piece.Requirement[] newResources =
        //         {
        //             EikthyrTrophyData
        //         };
        //         pieceScript.m_resources = newResources;
        //     }
        //
        //     if (netViewOverrideName == "BossStone_TheElder_Buildable(Clone)")
        //     {
        //         GameObject ElderTrophy = ZNetScene.instance.GetPrefab("TrophyTheElder");
        //         Piece.Requirement ElderTrophyData = new Piece.Requirement
        //         {
        //             m_resItem = ElderTrophy.GetComponent<ItemDrop>(),
        //             m_amount = 1,
        //             m_extraAmountOnlyOneIngredient = 0,
        //             m_amountPerLevel = 0,
        //             m_recover = true
        //         };
        //         // var clonedResources = pieceScript.m_resources;
        //         Piece.Requirement[] newResources =
        //         {
        //             ElderTrophyData
        //         };
        //         pieceScript.m_resources = newResources;
        //     }
        //     if (netViewOverrideName == "BossStone_Bonemass_Buildable(Clone)")
        //     {
        //         GameObject BonemassTrophy = ZNetScene.instance.GetPrefab("TrophyBonemass");
        //         Piece.Requirement BonemassTrophyData = new Piece.Requirement
        //         {
        //             m_resItem = BonemassTrophy.GetComponent<ItemDrop>(),
        //             m_amount = 1,
        //             m_extraAmountOnlyOneIngredient = 0,
        //             m_amountPerLevel = 0,
        //             m_recover = true
        //         };
        //         // var clonedResources = pieceScript.m_resources;
        //         Piece.Requirement[] newResources =
        //         {
        //             BonemassTrophyData
        //         };
        //         pieceScript.m_resources = newResources;
        //     }
        //     if (netViewOverrideName == "BossStone_DragonQueen_Buildable(Clone)")
        //     {
        //         GameObject DragonQueenTrophy = ZNetScene.instance.GetPrefab("TrophyDragonQueen");
        //         Piece.Requirement DragonQueenTrophyData = new Piece.Requirement
        //         {
        //             m_resItem = DragonQueenTrophy.GetComponent<ItemDrop>(),
        //             m_amount = 1,
        //             m_extraAmountOnlyOneIngredient = 0,
        //             m_amountPerLevel = 0,
        //             m_recover = true
        //         };
        //         // var clonedResources = pieceScript.m_resources;
        //         Piece.Requirement[] newResources =
        //         {
        //             DragonQueenTrophyData
        //         };
        //         pieceScript.m_resources = newResources;
        //     }
        //     if (netViewOverrideName == "BossStone_Yagluth_Buildable(Clone)")
        //     {
        //         GameObject YagluthTrophy = ZNetScene.instance.GetPrefab("TrophyGoblinKing");
        //         Piece.Requirement YagluthTrophyData = new Piece.Requirement
        //         {
        //             m_resItem = YagluthTrophy.GetComponent<ItemDrop>(),
        //             m_amount = 1,
        //             m_extraAmountOnlyOneIngredient = 0,
        //             m_amountPerLevel = 0,
        //             m_recover = true
        //         };
        //         // var clonedResources = pieceScript.m_resources;
        //         Piece.Requirement[] newResources =
        //         {
        //             YagluthTrophyData
        //         };
        //         pieceScript.m_resources = newResources;
        //     }
        //     if (netViewOverrideName == "BossStone_TheQueen_Buildable(Clone)")
        //     {
        //         GameObject SeekerQueenTrophy = ZNetScene.instance.GetPrefab("TrophySeekerQueen");
        //         Piece.Requirement SeekerQueenTrophyData = new Piece.Requirement
        //         {
        //             m_resItem = SeekerQueenTrophy.GetComponent<ItemDrop>(),
        //             m_amount = 1,
        //             m_extraAmountOnlyOneIngredient = 0,
        //             m_amountPerLevel = 0,
        //             m_recover = true
        //         };
        //         // var clonedResources = pieceScript.m_resources;
        //         Piece.Requirement[] newResources =
        //         {
        //             SeekerQueenTrophyData
        //         };
        //         pieceScript.m_resources = newResources;
        //     }
        //     return true;
        // }
    }
}