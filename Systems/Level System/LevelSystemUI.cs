using Ink.Parsed;
using Maledictus.CustomUI;
using Obvious.Soap;
using System.Globalization;
using UnityEngine;
using VInspector;

namespace Maledictus.Level
{
    public class LevelSystemUI : MonoBehaviour
    {
        [SerializeField] private CustomText _levelValueText;
        [SerializeField] private CustomText _experienceValueText;
        [SerializeField] private CustomUIBar _experienceUIBar;

        [Space(15f)]
        [SerializeField] private ScriptableVariable<int> _level;
        [SerializeField] private ScriptableVariable<int> _experience;
        [SerializeField] private ScriptableVariable<int> _experienceToNextLevel;

        public float ExperiencePercent => (float)_experience.Value / _experienceToNextLevel.Value;

        private void OnEnable()
        {
            _level.OnValueChanged += UpdateLevel;
            _experience.OnValueChanged += UpdateExperience;
            _experienceToNextLevel.OnValueChanged += UpdateExperience;
        }

        private void OnDisable()
        {
            _level.OnValueChanged -= UpdateLevel;
            _experience.OnValueChanged -= UpdateExperience;
            _experienceToNextLevel.OnValueChanged -= UpdateExperience;
        }

        private void UpdateLevel(int level) => _levelValueText.SetText(level.ToString());
        private void UpdateExperience(int experience)
        {
            var exp = AbbreviateNumbers.FormattedNumber(_experience.Value);
            var maxExp = AbbreviateNumbers.FormattedNumber(_experienceToNextLevel.Value);

            _experienceValueText.SetText($"{exp} / {maxExp}");
            UpdateLevelBar(ExperiencePercent);
        }

        private void UpdateLevelBar(float expBar)
        {
            ResetLevelBar();

            _experienceUIBar.SetBarFill(expBar);
            _experienceUIBar.SetForegroundBarFill(expBar);
        }

        private void ResetLevelBar()
        {
            _experienceUIBar.SetBarFill(0, 0f);
            _experienceUIBar.SetForegroundBarFill(0, 0f);
        }
    }
}