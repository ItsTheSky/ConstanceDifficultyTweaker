using System;
using BepInEx.Logging;
using Constance;
using HarmonyLib;
using UnityEngine;

namespace ConstanceDifficultyTweaker.Patches.Player;

[HarmonyPatch(typeof(CConPlayerPotion))]
public static class PlayerPotionPatches
{
    private static ManualLogSource Logger =
        BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);
    
    public static void ForceUpdatePotionCount()
    {
        try
        {
            // TODO: Add support for multiple player if one day
            var obj = GameObject.Find("Player-0");
            if (obj == null)
            {
                Logger.LogWarning("Cannot find Player-0 object to update potion count.");
                return;
            }
            
            obj.GetComponent<CConPlayerPotion>().CallPrivateMethod("UpdatePotionCount");
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while forcing UpdatePotionCount: " + ex);
        }
    }
    
    [HarmonyPatch("UpdatePotionCount", [])]
    [HarmonyPrefix]
    public static bool PrefixUpdatePotionCount(CConPlayerPotion __instance)
    {
        Logger.LogDebug("PrefixUpdatePotionCount called. Value: " + SharedConfigValues.HealthPotionCount);
        if (SharedConfigValues.HealthPotionCount == null) 
            return true; // no changes

        __instance.UpdatePotionCount(__instance.Current, (int) SharedConfigValues.HealthPotionCount.Value);
        return false;
    }
    
}