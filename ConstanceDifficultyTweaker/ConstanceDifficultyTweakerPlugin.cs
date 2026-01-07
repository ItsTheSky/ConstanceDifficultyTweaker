using System;
using BepInEx;
using ConstanceDifficultyTweaker.Localizations;
using ConstanceDifficultyTweaker.Patches.UI;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace ConstanceDifficultyTweaker
{
    
    [BepInPlugin("net.itsthesky.ConstanceDifficultyTweaker", "ConstanceDifficultyTweaker", "1.0.0")]
    public class ConstanceDifficultyTweakerPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("ConstanceDifficultyTweaker is initializing...");
            
            LocalizationLoader.Setup();
            var harmony = new Harmony("net.itsthesky.ConstanceDifficultyTweaker");
            harmony.PatchAll();
            
            SceneManager.sceneLoaded += UISetup.OnSceneLoaded;
            
            Logger.LogInfo("ConstanceDifficultyTweaker has initialized successfully.");
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= UISetup.OnSceneLoaded;
        }
    }
    
}