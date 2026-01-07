using BepInEx.Logging;
using Constance;
using HarmonyLib;

namespace ConstanceDifficultyTweaker.Patches
{
    [HarmonyPatch(typeof(ConAttackUtils))]
    public class AttackUtilsPatches
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);

        [HarmonyPatch(nameof(ConAttackUtils.PerformAttack))]
        [HarmonyPrefix]
        public static void PerformAttack_Prefix(IConAttackable toAttack, ref ConAttackRequest request)
        {
            if (!request.TryGetCommand(out ConAttackCommand_Damage damageCmd))
                return;

            var isPlayerTarget = toAttack.Entity is IConPlayerEntity;
            var multiplier = isPlayerTarget 
                ? SharedConfigValues.TakenDamageMultiplier 
                : SharedConfigValues.DealtDamageMultiplier;
    
            var currentDamage = damageCmd.Damage;
            var newDamage = currentDamage * multiplier;
    
            var damageField = typeof(ConAttackCommand_Damage).GetField("Damage");
            damageField.SetValue(damageCmd, newDamage);
    
            var attackDirection = isPlayerTarget ? "ENTITY => PLAYER" : "PLAYER => ENTITY";
            Logger.LogInfo($"Modified damage from {currentDamage} to {newDamage} ({attackDirection}) [Type: {damageCmd.DamageType}]");
        }
    }
}