using Maledictus.Codex;
using Maledictus.Coin;
using Maledictus.Inventory;
using Maledictus.Level;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace SomniaGames.Persistence
{
    public class PersistenceSystem : MonoBehaviour
    {
        public static event System.Action OnSaveCompleted;
        public static event System.Action<GameData> OnLoadCompleted;

        [SerializeField] private bool _useEncryption = false;

        private JsonFilePersistence _repository;

        [Space(15f)]
        [SerializeField] private InventoryDatabaseSO _inventoryDatabaseSO;

        private CoinData _coinData;
        private LevelData _levelData;
        private CodexData _codexData;
        private InventoryData _inventoryData = new InventoryData();

        private GameData _gameData;

        private void Awake()
        {
            _repository = new("GameData.save", _useEncryption);
        }

        private void Start() => LoadGameData();
        private void OnDestroy() => SaveGameData();

        private void OnEnable()
        {
            CoinSystem.OnCoinsChange += HandleCoinData;
            LevelSystem.OnLevelUpdate += HandleLevelData;
            CodexController.OnCodexUpdate += HandleCodexData;

            _inventoryDatabaseSO.WeaponInventorySO.OnInventoryUpdate += HandleWeaponInventoryData;
            _inventoryDatabaseSO.NecklaceInventorySO.OnInventoryUpdate += HandleNecklaceInventoryData;
            _inventoryDatabaseSO.BraceletInventorySO.OnInventoryUpdate += HandleBraceletInventoryData;
            _inventoryDatabaseSO.RingInventorySO.OnInventoryUpdate += HandleRingInventoryData;
        }

        private void OnDisable()
        {
            CoinSystem.OnCoinsChange -= HandleCoinData;
            LevelSystem.OnLevelUpdate -= HandleLevelData;
            CodexController.OnCodexUpdate -= HandleCodexData;

            _inventoryDatabaseSO.WeaponInventorySO.OnInventoryUpdate -= HandleWeaponInventoryData;
            _inventoryDatabaseSO.NecklaceInventorySO.OnInventoryUpdate -= HandleNecklaceInventoryData;
            _inventoryDatabaseSO.BraceletInventorySO.OnInventoryUpdate -= HandleBraceletInventoryData;
            _inventoryDatabaseSO.RingInventorySO.OnInventoryUpdate -= HandleRingInventoryData;
        }

        private void HandleCoinData(int coins) => _coinData = new CoinData(coins);
        private void HandleLevelData(int level, int experience) => _levelData = new LevelData(level, experience);
        private void HandleCodexData(List<CodexCategoryData> data) => _codexData = new CodexData(data);

        private void HandleWeaponInventoryData(BaseInventoryData data) => _inventoryData.SetWeaponInventoryData(data);
        private void HandleNecklaceInventoryData(BaseInventoryData data) => _inventoryData.SetNecklaceInventoryData(data);
        private void HandleBraceletInventoryData(BaseInventoryData data) => _inventoryData.SetBraceletInventoryData(data);
        private void HandleRingInventoryData(BaseInventoryData data) => _inventoryData.SetRingInventoryData(data);

        [Button]
        public async void SaveGameData()
        {
            _gameData = new GameData(_coinData, _levelData, _codexData, _inventoryData);

            await _repository.SaveAsync(_gameData);
            OnSaveCompleted?.Invoke();
        }

        [Button]
        public async void LoadGameData()
        {
            _gameData = await _repository.LoadAsync();

            OnLoadCompleted.Invoke(_gameData);
        }
    }
}