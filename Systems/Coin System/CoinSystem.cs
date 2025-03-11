using Obvious.Soap;
using SomniaGames.Persistence;
using UnityEngine;

namespace Maledictus.Coin
{
    public class CoinSystem : MonoBehaviour
    {
        public static event System.Action<int> OnCoinsChange;

        [SerializeField] private ScriptableVariable<int> Coins;

        [Space(15f)]
        [SerializeField] private ScriptableEvent<int> _onGainCoins;

        private void OnEnable()
        {
            PersistenceSystem.OnLoadCompleted += HandleLoadGameData;
            _onGainCoins.OnRaised += UpdateCoins;
        }

        private void OnDisable()
        {
            PersistenceSystem.OnLoadCompleted -= HandleLoadGameData;
            _onGainCoins.OnRaised -= UpdateCoins;
        }

        private void UpdateCoins(int coins)
        {
            Coins.Value += coins;
            OnCoinsChange?.Invoke(Coins.Value);
        }

        private void HandleLoadGameData(GameData gameData)
        {
            Coins.Value = gameData.CoinData.Coins;
            OnCoinsChange?.Invoke(Coins.Value);
        }
    }
}