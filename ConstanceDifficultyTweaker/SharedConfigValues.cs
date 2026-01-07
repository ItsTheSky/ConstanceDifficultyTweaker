using ConstanceDifficultyTweaker.Localizations;
using ConstanceDifficultyTweaker.Patches.Player;
using ConstanceDifficultyTweaker.Patches.UI.Settings;

namespace ConstanceDifficultyTweaker
{
    public static class SharedConfigValues
    {
        public static string PluginLogPrefix = "CDT";

        [Setting(SettingCategory.Main, 
            LangKeys.SETTINGS_UI_SECTION_MAIN_TakenDamageMultiplier)]
        [SettingSlider(0.0f, 10.0f, 0.1f)]
        public static float TakenDamageMultiplier { get; set; } = 1f;

        [Setting(SettingCategory.Main, 
            LangKeys.SETTINGS_UI_SECTION_MAIN_DealtDamageMultiplier)]
        [SettingSlider(0.0f, 10.0f, 0.1f)]
        public static float DealtDamageMultiplier { get; set; } = 1f;

        [Setting(SettingCategory.Main,
            LangKeys.SETTINGS_UI_SECTION_MAIN_HealthPotionCount)]
        [SettingSlider(0f, 5f, 1f, true)]
        public static float? HealthPotionCount
        {
            get;
            set
            {
                field = value;
                PlayerPotionPatches.ForceUpdatePotionCount();
            }
        } = null;

        [Setting(SettingCategory.Abilities, 
            LangKeys.SETTINGS_UI_SECTION_ABILITIES_CopyCatProjectilesCount)]
        [SettingSlider(1f, 16f, 1f)]
        public static float CopycatProjectilesCount { get; set; } = 4f;

        [Setting(SettingCategory.Abilities, 
            LangKeys.SETTINGS_UI_SECTION_ABILITIES_DashThroughPenaltyDisabled)]
        public static bool DashThroughPenaltyDisabled { get; set; } = true;

        [Setting(SettingCategory.Experimental, 
            LangKeys.SETTINGS_UI_SECTION_EXPERIMENTAL_GravityMultiplier)]
        [SettingSlider(0.1f, 5.0f, 0.1f)]
        public static float GravityMultiplier { get; set; } = 1.0f;
    }

    public static class JugglerConfigValues
    {
        public static bool ModifyJugglerProjectiles = true;
        public static int JugglerSmallProjectilesCount = 40;
    }
}