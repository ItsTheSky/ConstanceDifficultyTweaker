using System;
using BepInEx.Logging;
using Constance;
using HarmonyLib;
using Leo;
using UnityEngine;

namespace ConstanceDifficultyTweaker.Patches.Bosses
{
    [HarmonyPatch]
    public static class BulletHellJugglerPatches
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);

        [HarmonyPatch(typeof(ConStateAbility_BulletHellJuggler_Projectiles),
            "SpawnSmallCurveProjectiles")]
        [HarmonyPrefix]
        public static void SpawnSmallCurveProjectiles_Prefix(ConStateAbility_BulletHellJuggler_Projectiles __instance)
        {
            Logger.LogWarning(
                $"PATCH APPELÉ ! ModifyJugglerProjectiles={JugglerConfigValues.ModifyJugglerProjectiles}, Count={JugglerConfigValues.JugglerSmallProjectilesCount}");
            
            //CConUiJournalSubPageCarouselItem

            if (!JugglerConfigValues.ModifyJugglerProjectiles)
                return;

            var sm = __instance.GetPrivateField<CConStateMachine_BulletHellJuggler.ConStateMachine_BulletHellJuggler>("SM");
            var customCount = JugglerConfigValues.JugglerSmallProjectilesCount;
            sm.Config.curveSmallProjectilesCount = customCount;
        }
    }
}