using BepInEx.Logging;
using Constance;
using Leo;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Patches.UI.Settings
{
    public class TestSetting : AConGameSetting
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);
        public static readonly IPlayerPrefEntry.Int Pref = new("gameSettings.test.testSetting");
        
        private readonly ConSettingsOption[] _availableOptions = new ConSettingsOption[2]
        {
            ConSettingsOption.On,
            ConSettingsOption.Off,
        };
        
        public override void ApplyOption()
        {
            Logger.LogInfo($"Applying TestSetting with value: {GetPref().Value}");
        }
        
        public static bool Enabled => Pref.Value == 0;

        public override ConLocalizationRef Label => new ConLocalizationRef((TableReference) "UI", 
            (TableEntryReference) "Hello there!");
        public override float CurrentFillAmount => Enabled.BoolFloat();

        public override ConSettingsOption[] GetAvailableOptions() => _availableOptions;
        protected override IPlayerPrefEntry.Int GetPref() => Pref;
    }
}