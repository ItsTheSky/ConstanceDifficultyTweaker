using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using Constance;
using ConstanceDifficultyTweaker.Localizations;
using Leo;
using QFSW.QC.Utilities;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Patches.UI.Settings;

/// <summary>
/// Represents a custom game setting for Constance Difficulty Tweaker (CDT).
/// This specific class was made to manage <see cref="SharedConfigValues"/>-related settings.
/// </summary>
public class CDTConstantSetting : AConGameSetting
{

    private static readonly ConSettingsOption Default = ConSettingsOption.Localized(LangKeys.SETTINGS_UI_DEFAULT);
    private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(SharedConfigValues.PluginLogPrefix);
    
    #region Factory

    public static List<CDTConstantSetting> CreateAllFromClass(Type sharedClassType)
    {
        var settings = new List<CDTConstantSetting>();
        var properties = sharedClassType.GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (var property in properties)
        {
            if (!property.HasAttribute<SettingAttribute>()) continue; // we skip those (not supported yet)
            var labelAttr = property.GetCustomAttribute<SettingAttribute>();
            if (labelAttr == null) continue;
            
            var category = labelAttr.Category;
            var label = labelAttr.Label;
            
            CDTconstantSettingInfo settingInfoAttr = null;
            if (property.HasAttribute<SettingSliderAttribute>())
            {
                var attr = property.GetCustomAttribute<SettingSliderAttribute>();
                settingInfoAttr = new CDTconstantSettingInfo(CDTConstantSettingType.MinMax, category, attr.Min, attr.Max, attr.Step, attr.AllowDefault);
            }
            else
            {
                if (property.PropertyType == typeof(bool)) 
                    settingInfoAttr = new CDTconstantSettingInfo(CDTConstantSettingType.ToggleBoolean, category);
                else if (property.PropertyType == typeof(bool?)) 
                    settingInfoAttr = new CDTconstantSettingInfo(CDTConstantSettingType.ToggleKleenean, category);
            }
            
            if (settingInfoAttr != null)
            {
                var setting = new CDTConstantSetting(sharedClassType, property.Name, settingInfoAttr, label);
                settings.Add(setting);
            }
        }

        return settings;
    } 

    #endregion
    
    
    private IPlayerPrefEntry.Int _pref;
    
    private Type _sharedClassType;
    private PropertyInfo _propertyInfo;
    private CDTconstantSettingInfo _settingInfo;
    
    private ConLocalizationRef _label;
    private IEnumerable<ConSettingsOption> _availableOptions;
    
    public SettingCategory Category => _settingInfo.Category;
    
    public CDTConstantSetting(Type sharedClassType, string propertyName, 
        CDTconstantSettingInfo settingInfo, 
        ConLocalizationRef label)
    {
        _pref = new IPlayerPrefEntry.Int("gameSettings.cdt_constants." + propertyName);
        
        _sharedClassType = sharedClassType;
        _settingInfo = settingInfo;
        _label = label;
        _propertyInfo = sharedClassType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public);

        _availableOptions = new List<ConSettingsOption>();
        switch (settingInfo.Type)
        {
            case CDTConstantSettingType.ToggleBoolean:
                _availableOptions = [ConSettingsOption.On, ConSettingsOption.Off];
                break;
            case CDTConstantSettingType.ToggleKleenean:
                _availableOptions = [ConSettingsOption.On, ConSettingsOption.Raw("default"), ConSettingsOption.Off];
                break;
            case CDTConstantSettingType.MinMax:
                
                var options = new List<ConSettingsOption>();
                if (settingInfo.AllowDefault)
                    options.Add(Default);
                
                var format = "0.0";
                if (Mathf.Approximately(settingInfo.Step % 1, 0))
                    format = "0";
                else if (Mathf.Approximately(settingInfo.Step * 10 % 1, 0))
                    format = "0.0";
                else if (Mathf.Approximately(settingInfo.Step * 100 % 1, 0))
                    format = "0.00";
                
                var starting = settingInfo.Min;;
                while (starting <= settingInfo.Max)
                {
                    options.Add(ConSettingsOption.Raw(starting.ToString(format)));
                    starting += settingInfo.Step;
                }
                _availableOptions = options;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public override void ApplyOption()
    {
        var index = _pref.Value;
        switch (_settingInfo.Type)
        {
            case CDTConstantSettingType.MinMax:
                if (index == 0 && _settingInfo.AllowDefault)
                {
                    _propertyInfo.SetValue(null, null);
                    Logger.LogInfo($"Applied CDT constant setting '{_propertyInfo.Name}' with value: null (default)");
                }
                else
                {
                    var actualIndex = _settingInfo.AllowDefault ? index - 1 : index;
                    var value = _settingInfo.Min + actualIndex * _settingInfo.Step;
                    _propertyInfo.SetValue(null, value); 
                    Logger.LogInfo($"Applied CDT constant setting '{_propertyInfo.Name}' with value: {value}");
                }
                break;
            case CDTConstantSettingType.ToggleBoolean:
                var boolValue = index == 0;
                _propertyInfo.SetValue(null, boolValue);
                Logger.LogInfo($"Applied CDT constant setting '{_propertyInfo.Name}' with value: {boolValue}");
                break;
            case CDTConstantSettingType.ToggleKleenean:
                bool? kleeneanValue = index switch
                {
                    0 => true,
                    1 => null,
                    _ => false
                };
                
                _propertyInfo.SetValue(null, kleeneanValue);
                Logger.LogInfo($"Applied CDT constant setting '{_propertyInfo.Name}' with value: {kleeneanValue}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override ConLocalizationRef Label => _label;
    public override ConSettingsOption[] GetAvailableOptions() => _availableOptions.ToArray();
    protected override IPlayerPrefEntry.Int GetPref() => _pref;

    public override float CurrentFillAmount
    {
        get
        {
            var index = _pref.Value;
            return _settingInfo.Type switch
            {
                CDTConstantSettingType.MinMax =>
                    _settingInfo.AllowDefault
                        ? index / _availableOptions.Count() 
                        : ((float) _propertyInfo.GetValue(null) - _settingInfo.Min) / (_settingInfo.Max - _settingInfo.Min),
                CDTConstantSettingType.ToggleBoolean => index,
                CDTConstantSettingType.ToggleKleenean => index / 2f,
                _ => 0f
            };
        }
    }

    public override void OnLevelLoaded()
    {
        ApplyOption();
    }
}

public enum CDTConstantSettingType
{
    ToggleBoolean, // on/off
    ToggleKleenean, // yes/no/"maybe" = default
    MinMax, // float range
}

public record CDTconstantSettingInfo(CDTConstantSettingType Type, SettingCategory Category, float Min = 0f, float Max = 1f, float Step = 0.1f, bool AllowDefault = false)
{
    public CDTConstantSettingType Type { get; } = Type;
    public SettingCategory Category { get; } = Category;
    
    // For slider
    public float Min { get; } = Min;
    public float Max { get; } = Max;
    public float Step { get; } = Step;
    public bool AllowDefault { get; } = AllowDefault;
};

public class SettingAttribute(SettingCategory category, string key) : Attribute
{
    public ConLocalizationRef Label { get; } = new((TableReference) LangKeys.CDT_TABLE_NAME, (TableEntryReference) key);
    public SettingCategory Category { get; } = category;
}

public enum SettingCategory
{
    Main,
    Abilities,
    Experimental
}

public class SettingSliderAttribute(float min, float max, float step, bool allowDefault = false) : Attribute
{
    public float Min { get; } = min;
    public float Max { get; } = max;
    public float Step { get; } = step;
    public bool AllowDefault { get; } = allowDefault;
}