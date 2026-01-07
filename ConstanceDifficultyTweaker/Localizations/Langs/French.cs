using UnityEngine;
using UnityEngine.Localization.Tables;

namespace ConstanceDifficultyTweaker.Localizations.Langs;

public class French : ILangLoader
{
    public void Load(StringTable table)
    {
        table.AddEntry(LangKeys.SETTINGS_UI_TITLE, "Ajusteur de Difficulté");
        table.AddEntry(LangKeys.SETTINGS_UI_DEFAULT, "défaut");

        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN, "Général");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN_TakenDamageMultiplier, "Mult. de Dégâts Reçus");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN_DealtDamageMultiplier, "Mult. de Dégâts Infligés");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_MAIN_HealthPotionCount, "Quantité de Potions/Vials de Santé");
        
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_ABILITIES, "Capacités");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_ABILITIES_CopyCatProjectilesCount, "Nombre de Projectiles de Copycat");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_ABILITIES_DashThroughPenaltyDisabled, "Pénalité de la poupée");
        
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_EXPERIMENTAL, "Expérimental");
        table.AddEntry(LangKeys.SETTINGS_UI_SECTION_EXPERIMENTAL_GravityMultiplier, "Mult. de Gravité");
    }

    public SystemLanguage Language => SystemLanguage.French;
}