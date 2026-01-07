using System;
using System.Collections.Generic;
using BepInEx.Logging;
using Constance;
using ConstanceDifficultyTweaker.Localizations;
using ConstanceDifficultyTweaker.Patches.UI.Settings;
using ConstanceDifficultyTweaker.Utilities;
using HarmonyLib;
using Leo;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ConstanceDifficultyTweaker.Patches.UI
{
    /// <summary>
    /// Class responsible to inject custom UI once the ProdMainScene is loaded. (as it's only loaded once)
    /// </summary>
    public class UISetup
    {
        private static ManualLogSource Logger =
            BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            bool? isMainMenu = scene.name == "ProdStartMenu" ? true : (scene.name == "ProdMainScene" ? false : null);
            if (isMainMenu == null)
                return;

            Logger.LogInfo("Injecting custom settings pages...");
            try
            {
                InjectPages(isMainMenu.Value);
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to inject custom settings pages: {e}");
                return;
            }

            Logger.LogInfo("Custom settings pages injected successfully.");
        }

        #region Custom Parameter Page

        private static void InjectPages(bool isMainMenu)
        {
            var settings = CDTConstantSetting.CreateAllFromClass(typeof(SharedConfigValues));

            UIUtilities.CreateSettingsPage(isMainMenu, "difficulty", LangKeys.SETTINGS_UI_TITLE,
                settings, SpriteRegistry.SETTINGS_CAROUSEL_ICON, Logger);
        }

        #endregion
    }
}