using System.Collections.Generic;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace TrophyStones;

public class SetForsakenStoneData
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public class SetForsakenStone
    {
        public static void Postfix(ZNetScene __instance)
        {
            // Get grey dwarf shaman heal effect
            GameObject shaman = __instance.GetPrefab("Greydwarf_Shaman");
            Humanoid gsHumanoidScript = shaman.GetComponent<Humanoid>();
            GameObject gsHealObj = gsHumanoidScript.m_defaultItems[2];
            ItemDrop gsHealItemDrop = gsHealObj.GetComponent<ItemDrop>();
            StatusEffect gsHeal = gsHealItemDrop.m_itemData.m_shared.m_attackStatusEffect;
            GameObject HealAoeEffect = gsHealItemDrop.m_itemData.m_shared.m_attack.m_attackProjectile;
        
            EffectList.EffectData fxGreydwarfShamanHealAoE = new EffectList.EffectData
            {
                m_prefab = HealAoeEffect,
                m_enabled = true,
                m_variant = -1,
                m_attach = false,
                m_inheritParentRotation = false,
                m_inheritParentScale = false,
                m_randomRotation = false,
                m_scale = false,
                m_childTransform = ""
            };
            // Get goblin shaman shield effect
            GameObject goblinShaman = __instance.GetPrefab("GoblinShaman");
            GameObject goblinShamanAttackProtect = goblinShaman.GetComponent<Humanoid>().m_defaultItems[0];
            var gsProtectSharedData = goblinShamanAttackProtect.GetComponent<ItemDrop>().m_itemData.m_shared;
            StatusEffect gsShieldStatusEffect = gsProtectSharedData.m_attackStatusEffect;

            EffectList.EffectData vfxGoblinShield = gsShieldStatusEffect.m_startEffects.m_effectPrefabs[0];
            
            SetStoneData(
                __instance,
                "StartStone",
                vfxGoblinShield,
                "Skjálf Protects",
                100f,
                "Absorbs 100 points of damage",
                true,
                "gpower",
                50f,
                "Skjálf Protection",
                "Skjálf Protection Fades",
                true,
                "SwordForsaken_RS",
                "Skjálf releases her power"
            );
            SetStoneData(
                __instance, 
                "StartStone1",
                fxGreydwarfShamanHealAoE,
                "Skjálf Heals",
                100f,
                "Heals 100 points",
                true,
                "gpower",
                10f,
                "Skjálf heals",
                "healing ends",
                false,
                "SwordForsaken_RS",
                "Skjálf releases her regenerative abilities"
            );

        }
    }

    private static void SetStoneData(
        ZNetScene scene, 
        string objName,
        EffectList.EffectData startEffect,
        string guardianPowerName = "",
        float coolDownValue = 10f,
        string gpToolTip = "",
        bool flashIconToggle = false,
        string gpActivateAnim = "gpower",
        float duration = 10f,
        string gpStartMsg = "",
        string gpStopMsg = "",
        bool fxGPAnimationToggle = true,
        string supportedItem = "SwordForsaken_RS",
        string completedMessage = "Skjálf releases her power",
        string placementEffectName1 = "vfx_Place_stone_wall_2x1",
        string placementEffectName2 = "sfx_build_hammer_stone",
        string destroyEffectName1 = "vfx_RockDestroyed",
        string destroyEffectName2 = "sfx_rock_destroyed",
        string hitEffectName1 = "vfx_RockHit",
        string hitEffectName2 = "sfx_rock_hit",
        string switchEffectName = "vfx_Place_throne02",
        string cloneBossName = "BossStone_Bonemass"
        )
    {
        GameObject ForsakenStone1 = scene.GetPrefab(objName);
        SetPieceScript(
            scene, 
            ForsakenStone1, 
            placementEffectName1, 
            placementEffectName2);
        SetWearNTearScript(
            scene, 
            ForsakenStone1, 
            destroyEffectName1, 
            destroyEffectName2, 
            hitEffectName1, 
            hitEffectName2, 
            switchEffectName);
        SetBossStoneScript(scene, ForsakenStone1, cloneBossName, completedMessage);
        SetItemStandScript(
            scene, 
            ForsakenStone1,
            startEffect,
            supportedItem,
            guardianPowerName,
            supportedItem,
            coolDownValue,
            gpToolTip,
            flashIconToggle,
            gpActivateAnim,
            duration,
            gpStartMsg,
            gpStopMsg,
            fxGPAnimationToggle
        );
       
    }

    private static void SetPieceScript(
        ZNetScene scene, 
        GameObject gameObject, 
        string effectName1 = "vfx_Place_stone_wall_2x1",
        string effectName2 = "sfx_build_hammer_stone")
    {
        EffectList.EffectData effect1 = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(effectName1),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData effect2 = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(effectName2),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData[] placementEffects = new EffectList.EffectData[2]
        {
            effect1,
            effect2
        };

        Piece pieceScript = gameObject.GetComponent<Piece>();
        pieceScript.m_placeEffect.m_effectPrefabs = placementEffects;
    }

    private static void SetWearNTearScript(
        ZNetScene scene, 
        GameObject gameObject, 
        string destroyedEffectName1 = "vfx_RockDestroyed", 
        string destroyEffectName2 = "sfx_rock_destroyed", 
        string hitEffectName1 = "vfx_RockHit", 
        string hitEffectName2 = "sfx_rock_hit", 
        string switchEffectName = "vfx_Place_throne02")
    {
        // Format destroy effect data
        EffectList.EffectData destroyEffectData1 = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(destroyedEffectName1),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData destroyEffectData2 = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(destroyEffectName2),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData[] newDestroyedEffects = new EffectList.EffectData[2]
            { destroyEffectData1, destroyEffectData2 };

        // Format hit effect data
        EffectList.EffectData HitEffectData1 = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(hitEffectName1),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData HitEffectData2 = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(hitEffectName2),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData[] newHitEffects = new EffectList.EffectData[2] 
            { HitEffectData1, HitEffectData2 };

        // Format switch effect data
        EffectList.EffectData switchEffectData = new EffectList.EffectData
        {
            m_prefab = scene.GetPrefab(switchEffectName),
            m_enabled = true,
            m_variant = -1,
            m_attach = false,
            m_inheritParentRotation = false,
            m_inheritParentScale = false,
            m_randomRotation = false,
            m_scale = false,
            m_childTransform = ""
        };
        EffectList.EffectData[] newSwitchEffects = new EffectList.EffectData[1] 
            { switchEffectData };
        
        // Set data
        WearNTear WearNTearScript = gameObject.GetComponent<WearNTear>();
        WearNTearScript.m_destroyedEffect.m_effectPrefabs = newDestroyedEffects;
        WearNTearScript.m_hitEffect.m_effectPrefabs = newHitEffects;
        WearNTearScript.m_switchEffect.m_effectPrefabs = newSwitchEffects;
    }

    private static void SetBossStoneScript(
        ZNetScene scene,
        GameObject gameObject,
        string cloneBossName = "BossStone_Bonemass",
        string completedMessage = ""
    )
    {
        // Get original data
        GameObject bossObj = scene.GetPrefab(cloneBossName);
        BossStone bossScript = bossObj.GetComponent<BossStone>();
        EffectList.EffectData[] step1Effects = bossScript.m_activateStep1.m_effectPrefabs;
        EffectList.EffectData[] step2Effects = bossScript.m_activateStep2.m_effectPrefabs;
        EffectList.EffectData[] step3Effects = bossScript.m_activateStep3.m_effectPrefabs;

        ItemStand itemStandScript = bossObj.GetComponentInChildren<ItemStand>();
        EffectList.EffectData[] itemStandEffects = itemStandScript.m_effects.m_effectPrefabs;
        // Get new scripts
        BossStone newBossStoneScript = gameObject.GetComponent<BossStone>();
        ItemStand newItemStandScript = gameObject.GetComponent<ItemStand>();
        // Set trophy input effects
        newBossStoneScript.m_activateStep1.m_effectPrefabs = step1Effects;
        newBossStoneScript.m_activateStep2.m_effectPrefabs = step2Effects;
        newBossStoneScript.m_activateStep3.m_effectPrefabs = step3Effects;
        newBossStoneScript.m_completedMessage = completedMessage;

        newItemStandScript.m_effects.m_effectPrefabs = itemStandEffects;
    }

    private static void SetItemStandScript(
        ZNetScene scene,
        GameObject obj,
        EffectList.EffectData startEffect,
        string supportedItem = "SwordForsaken_RS",
        string powerName = "Power",
        string iconName = "SwordForsaken_RS",
        float coolDown = 10f,
        string tooltip = "Odin powers",
        bool flashIcon = false,
        string activateAnimation = "gpower",
        float duration = 30f,
        string startMessage = "",
        string stopMessage = "",
        bool fxGPActivation = true
    )
    {
        if (!obj) return;
        // Get boss stone data
        GameObject boss = scene.GetPrefab("BossStone_Bonemass");
        ItemDrop supportedItemData = scene.GetPrefab(supportedItem).GetComponent<ItemDrop>();
        ItemStand itemStand = boss.GetComponentInChildren<ItemStand>();
        // Get boss stone activate power effects
        EffectList.EffectData[] activatePower = itemStand.m_activatePowerEffects.m_effectPrefabs;
        EffectList.EffectData[] activatePowerPlayer = itemStand.m_activatePowerEffectsPlayer.m_effectPrefabs;
        List<ItemDrop> newItemDropList = new List<ItemDrop> { supportedItemData };
        
        // Get boss activation effects
        EffectList.EffectData fxBossActivation = itemStand.m_guardianPower.m_startEffects.m_effectPrefabs[0];

        // Get new Item stand script
        ItemStand newItemStandScript = obj.GetComponent<ItemStand>();
        StatusEffect guardianPower = newItemStandScript.m_guardianPower;
        if (!guardianPower) return;
        
        // Set activation effects
        newItemStandScript.m_activatePowerEffects.m_effectPrefabs = activatePower;
        newItemStandScript.m_activatePowerEffectsPlayer.m_effectPrefabs = activatePowerPlayer;
        newItemStandScript.m_supportedItems = newItemDropList;
        
        // Create new start effect list
        EffectList.EffectData[] newEffects = new EffectList.EffectData[1] { startEffect };
        EffectList.EffectData[] newEffectswithGuardianFX = new EffectList.EffectData[2]
        {
            startEffect,
            fxBossActivation
        };
        // Configure Guardian power
        GameObject iconObj = scene.GetPrefab(iconName);
        if (!iconObj) return;

        guardianPower.m_startEffects.m_effectPrefabs = fxGPActivation ? newEffectswithGuardianFX : newEffects;
        guardianPower.m_name = powerName;
        guardianPower.m_icon = iconObj.GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
        guardianPower.m_cooldown = coolDown;
        guardianPower.m_tooltip = tooltip;
        guardianPower.m_flashIcon = flashIcon;
        guardianPower.m_activationAnimation = activateAnimation;
        guardianPower.m_ttl = duration;
        guardianPower.m_startMessage = startMessage;
        guardianPower.m_stopMessage = stopMessage;
    }
}