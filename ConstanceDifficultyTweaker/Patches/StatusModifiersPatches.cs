using BepInEx.Logging;
using Constance;
using HarmonyLib;

namespace ConstanceDifficultyTweaker.Patches
{
    [HarmonyPatch(typeof(ConStatusModifiers))]
    public class StatusModifiersPatches
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);

        [HarmonyPatch(nameof(ConStatusModifiers.Reset))]
        [HarmonyPostfix]
        public static void Reset(ConStatusModifiers __instance)
        {
            // Logger.LogInfo("Applying custom status modifiers defaults...");
            
            // Logger.LogInfo($" - GravityScale: {oldGravity} -> {__instance.GravityScale}");
        }
        
    }

    [HarmonyPatch]
    public static class PuppetsCursePatches
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);
    
        [HarmonyPatch(typeof(SConStatusEffect_PuppetsCurse.ConStatusEffectInstance_PuppetCurse), 
            "ApplyModifiers", 
            new[] { typeof(IConEntity), typeof(ConStatusModifiers) })]
        [HarmonyPrefix]
        public static bool ApplyModifiers_Status(
            IConEntity entity, 
            ConStatusModifiers modifiers)
        {
            if (SharedConfigValues.DashThroughPenaltyDisabled)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch]
    public static class PuppetsCurseAttackPatches
    {
        [HarmonyPatch(typeof(SConStatusEffect_PuppetsCurse.ConStatusEffectInstance_PuppetCurse), 
            "Constance.IConEntityAttackModifierProvider.ApplyModifiers")]
        [HarmonyPrefix]
        public static bool ApplyModifiers_Attack(
            SConStatusEffect_PuppetsCurse.ConStatusEffectInstance_PuppetCurse __instance,
            IConEntity attacker,
            IConAttackable toAttack,
            SConAttackDescriptor.AttackModifiers modifiers,
            SConAttackDescriptor descriptor)
        {
            if (SharedConfigValues.DashThroughPenaltyDisabled)
            {
                return false;
            }
            return true;
        }
    }
}