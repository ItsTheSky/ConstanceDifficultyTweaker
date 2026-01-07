namespace ConstanceDifficultyTweaker.Localizations;

public static class LangKeys
{

    #region Main

    // for now we have to use Constance's table.
    // see https://discussions.unity.com/t/creating-stringtable-at-runtime/815428/8
    public const string CDT_TABLE_NAME = "UI";
    
    public const string SETTINGS_UI_TITLE = "settings.ui.title";
    public const string SETTINGS_UI_DEFAULT = "settings.ui.default";

    #endregion

    #region Settings - Main Settings 

    public const string SETTINGS_UI_SECTION_MAIN = "settings.ui.section.main";
    
    public const string SETTINGS_UI_SECTION_MAIN_TakenDamageMultiplier = "settings.ui.section.main.takenDamageMultiplier";
    public const string SETTINGS_UI_SECTION_MAIN_DealtDamageMultiplier = "settings.ui.section.main.dealtDamageMultiplier";
    public const string SETTINGS_UI_SECTION_MAIN_HealthPotionCount = "settings.ui.section.main.healthPotionCount";

    #endregion

    #region Settings - Abilities

    public const string SETTINGS_UI_SECTION_ABILITIES = "settings.ui.section.abilities";
    public const string SETTINGS_UI_SECTION_ABILITIES_DashThroughPenaltyDisabled = "settings.ui.section.main.dashThroughPenaltyDisabled";
    public const string SETTINGS_UI_SECTION_ABILITIES_CopyCatProjectilesCount = "settings.ui.section.main.copycatProjectilesCount";

    #endregion

    #region Settings - Experimental

    public const string SETTINGS_UI_SECTION_EXPERIMENTAL = "settings.ui.section.experimental";
    public const string SETTINGS_UI_SECTION_EXPERIMENTAL_GravityMultiplier = "settings.ui.section.experimental.gravityMultiplier";

    #endregion
    
}