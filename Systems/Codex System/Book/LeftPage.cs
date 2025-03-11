using Maledictus.CustomUI;
using Maledictus.Enemy;
using UnityEngine;

namespace Maledictus.Codex
{
    public class LeftPage : BasePage
    {
        [SerializeField] private CustomText _nameText;
        [SerializeField] private CustomText _descriptionText;

        public override void InitializeUnknown() => DisplayBasicStats(true);
        public override void InitializeEncountered() => DisplayBasicStats(false);
        public override void InitializeCaptured()
        {
            DisplayBasicStats(false);

        }

        private void DisplayBasicStats(bool isUnknown)
        {
            var name = isUnknown ? "???" : EnemySO.Name;
            var description = isUnknown ? "" : EnemySO.Description;

            _nameText.SetText(name);
            _descriptionText.SetText(description);
        }
    }
}