using Maledictus.CustomUI;
using Obvious.Soap;
using UnityEngine;

namespace Maledictus.Coin
{
    public class CoinSystemUI : MonoBehaviour
    {
        [SerializeField] private CustomText _coinValueText;

        [SerializeField] private ScriptableVariable<int> _coins;

        private void OnEnable()
        {
            _coins.OnValueChanged += UpdateCoins;
        }

        private void OnDisable()
        {
            _coins.OnValueChanged -= UpdateCoins;
        }

        private void UpdateCoins(int coins)
        {
            var label = coins <= 1 ? "Leu" : "Lei";
            _coinValueText.SetText($"{AbbreviateNumbers.FormattedNumber(coins)}  {label}");
        }
    }
}