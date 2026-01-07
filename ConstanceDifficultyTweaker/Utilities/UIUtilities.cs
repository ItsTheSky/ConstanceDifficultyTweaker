using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using Constance;
using ConstanceDifficultyTweaker.Localizations;
using ConstanceDifficultyTweaker.Patches.UI.Settings;
using JetBrains.Annotations;
using Leo;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ConstanceDifficultyTweaker.Utilities
{
    /// <summary>
    /// Utilities for easily editing Constance's UI.
    /// </summary>
    public class UIUtilities
    {
        #region Constants

        // For Game Menu
        private const string ACCESSIBILITY_PAGE_PATH_GAME =
            "UI/ui_Panel_Journal/Journal/JournalContent/Page_Settings/SubPage_Accessibility";
        private const string ACCESSIBILITY_BUTTON_PATH_GAME =
            "UI/ui_Panel_Journal/Journal/JournalContent/Page_Settings/DiaryCarousel/navItem_Accessibility";
        
        // For Main Menu
        private const string ACCESSIBILITY_PAGE_PATH_MAIN =
            "UI/Menu_Settings/Menu_Settings/SubPage_Accessibility";
        private const string ACCESSIBILITY_BUTTON_PATH_MAIN =
            "UI/Menu_Settings/Menu_Settings/DiaryCarousel/navItem_Accessibility";

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new settings (sub) page in the journal UI.
        /// </summary>
        /// <param name="id">The unique ID of the page.</param>
        /// <param name="isMainMenu">IMPORTANT: wheter the method should search and setup the setting page for the main menu or the game's menu.</param>
        /// <param name="displayNameKey">The localization key for the display name/title of the page.</param>
        /// <param name="settings">The settings to include in the page.</param>
        /// <param name="logger">Optional logger for logging progress and errors.</param>
        /// <returns>The created settings page GameObject. (the sub-page, NOT the carousel button)</returns>
        [CanBeNull]
        public static GameObject CreateSettingsPage(bool isMainMenu, 
            string id,
            string displayNameKey,
            IEnumerable<IConGameSetting> settings,
            [CanBeNull] string customIcon = null,
            [CanBeNull] ManualLogSource logger = null)
        {
            logger?.LogInfo($"Creating settings page: {id} with display key: {displayNameKey} ...");

            // Clone an existing sub page
            var originalPage = FindInactive(isMainMenu ? ACCESSIBILITY_PAGE_PATH_MAIN : ACCESSIBILITY_PAGE_PATH_GAME);
            if (originalPage == null)
            {
                logger?.LogError("    [0] Failed to find original settings page to clone! (originalPage is null)");
                return null;
            }

            var subPage = Object.Instantiate(originalPage, originalPage.transform.parent);
            if (subPage == null)
            {
                logger?.LogError("    [0] Failed to clone settings page! (subPage is null)");
                return null;
            }

            subPage.name = $"SubPage_{id}";
            var comp = subPage.GetComponentInChildren<ConLocalize_TMP>();
            comp.StringReference = new LocalizedString((TableReference)LangKeys.CDT_TABLE_NAME,
                (TableEntryReference)displayNameKey);

            logger?.LogInfo($"    [1] Cloned sub page: {subPage.name}");

            // Replace previous journal page comp & setup custom settings
            var originalComponent = subPage.GetComponent<CConUiJournalPage_SettingsAccessibility>();
            var customPage = subPage.AddComponent<CConUiJournalPage_SettingsCustom>();

            // Copy all serialized fields from the original component to the custom one
            var type = typeof(AConUiJournalPage_GameSettings);
            var flags = BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Instance;

            foreach (var field in type.GetFields(flags))
            {
                try
                {
                    var value = field.GetValue(originalComponent);
                    field.SetValue(customPage, value);
                }
                catch (Exception e)
                {
                    logger?.LogWarning($"Failed to copy field '{field.Name}': {e.Message}");
                }
            }

            customPage.SetupSettings(settings);
            logger?.LogInfo($"    [2] Replaced journal page comp: {customPage.GetType().Name}");
            Object.Destroy(originalComponent);

            // Create sub page button (clone an existing one)
            var originalButton = FindInactive(isMainMenu ? ACCESSIBILITY_BUTTON_PATH_MAIN : ACCESSIBILITY_BUTTON_PATH_GAME);
            var buttonClone = Object.Instantiate(originalButton, originalButton.transform.parent);
            buttonClone.name = $"navItem_{id}";
            logger?.LogInfo($"    [3] Cloned sub page button: {buttonClone.name}");

            // Handle custom icon
            if (customIcon != null)
            {
                var sprite = SpriteLoader.LoadFromFile(customIcon);
                buttonClone.transform.Find("img_Item").GetComponent<Image>().sprite = sprite;
                logger?.LogInfo("    [3.1] Set custom icon for sub page button");
            }
            //buttonClone.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = id;

            // Redirect to the right page
            var carouselItemComp = buttonClone.GetComponent<CConUiJournalSubPageCarouselItem>();
            carouselItemComp.SetPrivateField("linkedSubPage", customPage);
            logger?.LogInfo($"    [4] Redirected to sub page: {customPage.name}");

            // Finally, add the item to the carousel
            var carousel = buttonClone.transform.parent.GetComponent<CConUiJournalSubPageCarousel>();
            carousel.AddToArrayField("items", carouselItemComp);
            logger?.LogInfo("    [5] Added sub page to carousel");

            var allItems = carousel.Items;
            int newItemIndex = allItems.Length - 1; // Le nouvel item est le dernier
            allItems[newItemIndex].OnClick += () =>
            {
                // Appeler la méthode PUBLIQUE SelectItem (pas CallPrivateMethod!)
                carousel.SelectItem(newItemIndex);
            };
            logger?.LogInfo($"    [6] Subscribed OnClick event for new carousel item (index {newItemIndex})");

            return subPage;
        }

        #endregion

        /// <summary>
        /// Finds an inactive GameObject in the scene hierarchy.
        /// </summary>
        /// <param name="path">The path to the GameObject, separated by '/'.</param>
        /// <returns>The found GameObject, or null if not found.</returns>
        private static GameObject FindInactive(string path)
        {
            var parts = path.Split('/');
            Transform current = null;

            foreach (var part in parts)
            {
                if (current == null)
                {
                    // Cherche dans les root objects (même inactifs)
                    foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
                    {
                        if (root.name == part)
                        {
                            current = root.transform;
                            break;
                        }
                    }
                }
                else
                {
                    current = current.Find(part);
                }

                if (current == null)
                    return null;
            }

            return current?.gameObject;
        }
    }

    public class CConUiJournalPage_SettingsCustom : CConUiJournalPage_SettingsAccessibility
    {
        private static ManualLogSource Logger =
            BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);

        private Dictionary<SettingCategory, List<CDTConstantSetting>> _customSettingsByCategory = new();

        public void SetupSettings(IEnumerable<IConGameSetting> settings)
        {
            foreach (var setting in settings)
            {
                if (setting is not CDTConstantSetting ccSetting) continue;

                if (!_customSettingsByCategory.ContainsKey(ccSetting.Category))
                    _customSettingsByCategory[ccSetting.Category] = [];
                _customSettingsByCategory[ccSetting.Category].Add(ccSetting);
            }

            Logger.LogInfo($"SetupSettings called with {settings.Count()} settings");
        }

        public override void OnSubPageOpened()
        {
            Logger.LogInfo("OnSubPageOpened called, using override/custom method!");
            var items = this.GetPrivateField<List<CConUiPanel_Settings_Item>>("_items");
            var settingItemParent = this.GetPrivateField<RectTransform>("settingItemParent");

            items.Clear();
            settingItemParent.DestroyAllChildren();
            foreach (var (category, settings) in _customSettingsByCategory)
            {
                // add category label/header
                var txtCategory = new GameObject();
                txtCategory.transform.SetParent(settingItemParent, false);
                txtCategory.name = $"Category_{category}";
                var txtTmp = txtCategory.AddComponent<TextMeshProUGUI>();
                var conLocalize = txtCategory.AddComponent<ConLocalize_TMP>();
                var rectTransform = txtCategory.GetComponent<RectTransform>();

                var (table, entry) = category.GetCategoryLabel();
                var actualText = LocalizationSettings.StringDatabase.GetLocalizedString(table, entry);
                
                // setup localization component
                conLocalize.SetPrivateField("stringReference", new LocalizedString(table, entry));
                conLocalize.CallPrivateMethod("UpdateLocalizedText", actualText);
                
                // setup text component
                txtTmp.text = actualText;
                txtTmp.color = new Color(0.32f, 0.23f,0.4f);
                txtTmp.fontSize = 78;
                txtTmp.alignment = TextAlignmentOptions.Center;
                txtTmp.margin = new Vector4(20, 0, 0, 20);
                txtTmp.fontWeight = FontWeight.SemiBold;
                
                // final setup
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 100);
                
                foreach (IConGameSetting setting in settings)
                {
                    CConUiPanel_Settings_Item componentInChildren = Instantiate(setting.IsSubSetting()
                                ? this.GetPrivateField<GameObject>("settingItemSubPrefab")
                                : this.GetPrivateField<GameObject>("settingItemPrefab"),
                            settingItemParent.transform)
                        .GetComponentInChildren<CConUiPanel_Settings_Item>();
                    componentInChildren.gameObject.name = $"GameSettingItem-{setting.Label}";
                    componentInChildren.Init(setting);
                    componentInChildren.OnSettingChanged += () =>
                    {
                        this.GetPrivateField<CoroutineRefHolder>("_updateSettingsRoutineRef").KillCurrentRoutine();
                        this.GetPrivateField<CoroutineRefHolder>("_updateSettingsRoutineRef")
                            .Start(UpdateSettingsRoutine());
                    };
                    items.Add(componentInChildren);
                    componentInChildren.UpdateState(true);
                }
            }

            IEnumerator UpdateSettingsRoutine()
            {
                yield return null;
                foreach (CConUiPanel_Settings_Item panelSettingsItem in items)
                    panelSettingsItem.QueueUpdate();
                IConSceneListener_GameSettingsChanged.Notify();
                if ((bool)(Object)this.GetPrivateField<ConScrollRect>("scrollRect"))
                    this.GetPrivateField<ConScrollRect>("scrollRect").OnSizeChanged();
            }
        }
    }
}