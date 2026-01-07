using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Localizations.Langs;

public class English : ILangLoader
{
    public void Load(StringTable table)
    {
        table.AddEntry(LangKeys.SETTINGS_UI_TITLE, "Difficulty Tweaker");
        table.AddEntry(LangKeys.SETTINGS_UI_DEFAULT, "default");
        
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN, "General");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN_TakenDamageMultiplier, "Taken Damage Multiplier");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN_DealtDamageMultiplier, "Dealt Damage Multiplier");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN_HealthPotionCount, "Health Potion/Vial Count");
        
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_ABILITIES, "Abilities");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_ABILITIES_CopyCatProjectilesCount, "Copycat Projectiles Count");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_ABILITIES_DashThroughPenaltyDisabled, "Dash Through Penalty Disabled");
        
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_EXPERIMENTAL, "Experimental");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_EXPERIMENTAL_GravityMultiplier, "Gravity Multiplier");
    }

    public SystemLanguage Language => SystemLanguage.English;
}