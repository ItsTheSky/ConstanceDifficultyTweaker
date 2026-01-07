using System;
using ConstanceDifficultyTweaker.Localizations;
using ConstanceDifficultyTweaker.Patches.UI.Settings;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Utilities;

public static class SettingExtensions
{

    public static (TableReference tableReference, TableEntryReference entryReference) GetCategoryLabel(this SettingCategory category)
    {
        return category switch
        {
            SettingCategory.Main => (LangKeys.CDT_TABLE_NAME, LangKeys.SETTINGS_UI_SECTION_MAIN),
            SettingCategory.Abilities => (LangKeys.CDT_TABLE_NAME, LangKeys.SETTINGS_UI_SECTION_ABILITIES),
            SettingCategory.Experimental => (LangKeys.CDT_TABLE_NAME, LangKeys.SETTINGS_UI_SECTION_EXPERIMENTAL),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
    
}