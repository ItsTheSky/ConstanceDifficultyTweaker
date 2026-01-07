using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Localizations;

public interface ILangLoader
{

    void Load(StringTable table);

    SystemLanguage Language { get; }
    
}