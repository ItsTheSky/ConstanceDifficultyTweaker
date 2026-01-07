using BepInEx.Logging;
using ConstanceDifficultyTweaker.Localizations.Langs;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Localizations;

/// <summary>
/// Class that handles loading of the different custom localizations.
/// </summary>
public static class LocalizationLoader
{
    private static ManualLogSource Logger =
        BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix + ".LocalizationLoader");
    
    private static ILangLoader[] Loaders =
    [
        new English(),
        new French()
    ];

    public static void Setup()
    {
        LocalizationSettings.SelectedLocaleChanged += AddCustomEntries;
        AddCustomEntries(null);
        Logger.LogInfo("LocalizationLoader setup complete.");
    }
    
    public static void AddCustomEntries([CanBeNull] Locale newLocale)
    {
        foreach (var loader in Loaders)
        {
            // only add entries for the new locale
            if (newLocale != null && newLocale.Identifier != loader.Language)
                continue;
            
            var locale = LocalizationSettings.AvailableLocales.GetLocale(loader.Language);
            var table = LocalizationSettings.StringDatabase.GetTable((TableReference) LangKeys.CDT_TABLE_NAME, locale);
            
            var previousEntries = table.Count;
            loader.Load(table);
            Logger.LogInfo($"Loaded localization for {loader.Language} with {table.Count - previousEntries} new entries. (total: {table.Count})");
        }
    }
    
}