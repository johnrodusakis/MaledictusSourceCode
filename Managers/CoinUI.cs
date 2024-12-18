using Maledictus.CustomUI;
using Obvious.Soap;
using UnityEngine;

namespace Maledictus.Coin
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private CustomText _coinValueText;
        [SerializeField] private CustomText _coinLabelText;

        [SerializeField] private ScriptableVariable<int> _coins;

        private void Awake() => UpdateCoins(_coins);

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
            _coinLabelText.SetText(_coins.Value == 1 ? "Leu" : "Lei");
            _coinValueText.SetText(AbbreviateNumbers.AbbreviateNumber(coins));
        }
    }
}