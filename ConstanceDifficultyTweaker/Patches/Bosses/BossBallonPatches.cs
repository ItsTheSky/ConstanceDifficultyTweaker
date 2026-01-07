using BepInEx.Logging;
using Constance;
using HarmonyLib;
using Leo;
using UnityEngine;

namespace ConstanceDifficultyTweaker.Patches.Bosses
{
    [HarmonyPatch]
    public class BossBallonPatches
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);

        [HarmonyPatch(typeof(ConStateAbility_BossBalloon_ThrowBalloon),
            nameof(ConStateAbility_BossBalloon_ThrowBalloon.ThrowPogoBalloon))]
        [HarmonyPrefix]
        public static bool ThrowPogoBalloon_Prefix(ConStateAbility_BossBalloon_ThrowBalloon __instance, int laneIndex)
        {
            Logger.LogWarning($"PATCH APPELÉ ! ThrowPogoBalloon_Prefix LaneIndex={laneIndex}");
            
            var sm = __instance.GetPrivateField<CConStateMachine_BossBalloon.ConStateMachine_BossBalloon>("SM");
            if (laneIndex < 0 || laneIndex >= sm.Config.pogoLaneXPositions.Count)
                return true;
            
            var selfEntity = __instance.GetPrivateField<CConCharacterEntity>("Entity");
            
            var entity = __instance.SceneRegistry.PoolManager.Get<IConEntity>(sm.Config.pogoBalloonProjectile,
                new Vector2(sm.Config.pogoLaneXPositions[laneIndex] + sm.Component.arenaArea.Rect.center.x,
                    selfEntity.Body.RigidY + Random.Range(-sm.Config.pogoBalloonYRange, sm.Config.pogoBalloonYRange) +
                    sm.Config.pogoBalloonYOffset), Quaternion.identity, sm.Entity.LevelScene.AsParent());
            
            __instance.GetPrivateField<ConEntityTracker<IConEntity>>("_pogoBalloons").Add(entity);
            entity.AttachOwner(selfEntity);
            entity.Get<CConEntityAnimator>().SetClip(new ConAnimationClipId("Init"));
            var config = new ConProjectileConfig()
            {
                corruptedSpeed = sm.Config.pogoFlightSpeed
            };
            entity.Get<CConProjectileMovement_Fixed>().InitProjectile(config, Vector2.down);
            return true;
        }
    }
}