using HarmonyLib;
using UnityEngine;

namespace TrophyStones;

public class SwordForsakenPatch
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    public static class SwordForsakenPatcher
    {
        public static void Postfix(ZNetScene __instance)
        {
            if (!__instance) return;

            GameObject KromSword = __instance.GetPrefab("THSwordKrom");
            GameObject SwordForsaken = __instance.GetPrefab("SwordForsaken_RS");
            if (!KromSword || !SwordForsaken) return;

            var KromItemDropScript = KromSword.GetComponent<ItemDrop>();
            var ForsakenItemDropScript = SwordForsaken.GetComponent<ItemDrop>();
            var ForsakenSharedItemData = ForsakenItemDropScript.m_itemData.m_shared;

            var KromHitEffects = KromItemDropScript.m_itemData.m_shared.m_attack.m_hitEffect.m_effectPrefabs;
            var KromHitTerrainEffects =
                KromItemDropScript.m_itemData.m_shared.m_attack.m_hitTerrainEffect.m_effectPrefabs;
            var KromBlockEffects = KromItemDropScript.m_itemData.m_shared.m_blockEffect.m_effectPrefabs;
            var KromTriggerEffects = KromItemDropScript.m_itemData.m_shared.m_triggerEffect.m_effectPrefabs;
            var KromTrailEffects = KromItemDropScript.m_itemData.m_shared.m_trailStartEffect.m_effectPrefabs;

            ForsakenSharedItemData.m_attack.m_hitEffect.m_effectPrefabs = KromHitEffects;
            ForsakenSharedItemData.m_hitTerrainEffect.m_effectPrefabs = KromHitTerrainEffects;
            ForsakenSharedItemData.m_blockEffect.m_effectPrefabs = KromBlockEffects;
            ForsakenSharedItemData.m_triggerEffect.m_effectPrefabs = KromTriggerEffects;
            ForsakenSharedItemData.m_trailStartEffect.m_effectPrefabs = KromTrailEffects;
        }
    }
}