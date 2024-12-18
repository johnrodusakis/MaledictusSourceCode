using Obvious.Soap;
using UnityEngine;
using VInspector;

namespace Maledictus.Coin
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private ScriptableVariable<int> Coins;

        [Button] private void AddCoins() => UpdateCoins(5);
        [Button] private void RemoveCoins() => UpdateCoins(-5);

        private void UpdateCoins(int coins) => Coins.Value += coins;
    }
}