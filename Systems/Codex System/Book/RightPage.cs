using Maledictus.CustomUI;
using Maledictus.Enemy;
using UnityEngine;

namespace Maledictus.Codex
{
    public class RightPage : BasePage
    {
        [SerializeField] private CustomImage _figureImage;

        public override void InitializeUnknown() => DisplayFigure(true);

        public override void InitializeEncountered() => DisplayFigure(false);

        public override void InitializeCaptured() => DisplayFigure(false);

        private void DisplayFigure(bool isUnknown)
        {
            _figureImage.EnableImage(true);
            _figureImage.SetColor(isUnknown ? Color.black : Color.white);
            _figureImage.SetImage(EnemySO.Figure);
        }
    }
}