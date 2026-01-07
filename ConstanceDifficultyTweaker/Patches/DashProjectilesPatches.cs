using System;
using Constance;
using HarmonyLib;

namespace ConstanceDifficultyTweaker.Patches;

[HarmonyPatch(typeof(SConInspiration_DashProjectiles))]
public class DashProjectilesPatches
{
    
    [HarmonyPatch(nameof(SConInspiration_DashProjectiles.SpawnProjectiles))]
    [HarmonyPrefix]
    public static void SpawnProjectiles_Prefix(SConInspiration_DashProjectiles __instance, CConPlayerEntity player, Action<ConAttackSummary> onHit)
    {
        // we have to calculate the angles based on the amount of projectiles we want to spawn
        // don't forget to shift by 45 deg
        var desiredProjectiles = (int) SharedConfigValues.CopycatProjectilesCount;
        var angles = new float[desiredProjectiles];
        var angleStep = 360f / desiredProjectiles;
        for (var i = 0; i < desiredProjectiles; i++)
        {
            angles[i] = i * angleStep + 45f;
        }
        __instance.SetPrivateField("angles", angles);
    }
    
}