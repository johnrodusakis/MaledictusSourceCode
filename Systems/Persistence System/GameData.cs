using Ink.Parsed;
using Maledictus.Inventory;
using System.Collections.Generic;

namespace SomniaGames.Persistence
{
    [System.Serializable]
    public class CoinData
    {
        public int Coins = 0;

        public CoinData(int coins)
        {
            Coins = coins;
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public int Level = 1;
        public int Experience = 0;

        public LevelData(int level, int experience)
        {
            Level = level;
            Experience = experience;
        }
    }

    [System.Serializable]
    public class CodexCategoryData
    {
        public string Category;
        public List<CodexEnemyData> CodexEnemyData;

        public CodexCategoryData(string category, List<CodexEnemyData> codexEnemyData)
        {
            Category = category;
            CodexEnemyData = codexEnemyData;
        }
    }

    [System.Serializable]
    public class CodexEnemyData
    {
        public string EnemyName;
        public int Discovery = 0;
        public bool IsNew = false;

        public CodexEnemyData(string enemyName, int discovery, bool isNew)
        {
            EnemyName = enemyName;
            Discovery = discovery;
            IsNew = isNew;
        }
    }

    [System.Serializable]
    public class CodexData
    {
        public List<CodexCategoryData> CategoryData;

        public CodexData(List<CodexCategoryData> categoryData)
        {
            CategoryData = categoryData;
        }
    }

    [System.Serializable]
    public class ItemSlotData
    {
        public string Id;
        public double Order;
        public bool IsEmpty;
        public bool IsSelected;
        public bool IsNew;

        public ItemSlotData()
        {
            Id = string.Empty;
            Order = 0.0;
            IsSelected = false;
            IsNew = false;
            IsEmpty = true;
        }

        public ItemSlotData(string id, double order, bool isSelected, bool isNew)
        {
            Id = id;
            Order = order;
            IsSelected = isSelected;
            IsNew = isNew;
            IsEmpty = false;
        }
    }

    [System.Serializable]
    public class GearSlotData
    {
        public ItemSlotData ItemSlotData;
        public bool IsLocked;

        public GearSlotData(ItemSlotData itemSlotData, bool isLocked)
        {
            ItemSlotData = itemSlotData;
            IsLocked = isLocked;
        }
    }

    [System.Serializable]
    public class BaseInventoryData
    {
        public int SlotCapacity;
        public List<GearSlotData> GearSlots;
        public List<ItemSlotData> ItemSlots;

        public BaseInventoryData(int slotCapacity, List<GearSlotData> gearSlots, List<ItemSlotData> itemSlots)
        {
            SlotCapacity = slotCapacity;
            GearSlots = gearSlots;
            ItemSlots = itemSlots;
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        public BaseInventoryData WeaponInventoryData;
        public BaseInventoryData NecklaceInventoryData;
        public BaseInventoryData BraceletInventoryData;
        public BaseInventoryData RingInventoryData;

        public void SetWeaponInventoryData(BaseInventoryData data) => WeaponInventoryData = data;
        public void SetNecklaceInventoryData(BaseInventoryData data) => NecklaceInventoryData = data;
        public void SetBraceletInventoryData(BaseInventoryData data) => BraceletInventoryData = data;
        public void SetRingInventoryData(BaseInventoryData data) => RingInventoryData = data;

        public InventoryData()
        {
            var weaponGearSlot = new List<GearSlotData>
            {
                new (null, false),
                new (null, true)
            };

            WeaponInventoryData = new(10, weaponGearSlot, new List<ItemSlotData>());

            var necklaceGearSlot = new List<GearSlotData>
            {
                new (null, false),
            };

            NecklaceInventoryData = new(5, necklaceGearSlot, new List<ItemSlotData>());

            var braceletGearSlot = new List<GearSlotData>
            {
                new (null, false),
            };

            BraceletInventoryData = new(5, braceletGearSlot, new List<ItemSlotData>());

            var ringGearSlot = new List<GearSlotData>
            {
                new (null, false),
            };

            RingInventoryData = new(5, ringGearSlot, new List<ItemSlotData>());
        }

        public InventoryData(BaseInventoryData weaponInventoryData, BaseInventoryData necklaceInventoryData, BaseInventoryData braceletInventoryData, BaseInventoryData ringInventoryData)
        {
            WeaponInventoryData = weaponInventoryData;
            NecklaceInventoryData = necklaceInventoryData;
            BraceletInventoryData = braceletInventoryData;
            RingInventoryData = ringInventoryData;
        }
    }

    [System.Serializable]
    public class GameData
    {
        public CoinData CoinData;
        public LevelData LevelData;
        public CodexData CodexData;
        public InventoryData InventoryData;

        public GameData()
        {
            CoinData = new CoinData(0);
            LevelData = new LevelData(1, 0);
            CodexData = new CodexData(new List<CodexCategoryData>());
            InventoryData = new InventoryData();
        }

        public GameData(CoinData coinData, LevelData levelData, CodexData codexData, InventoryData inventoryData)
        {
            CoinData = coinData;
            LevelData = levelData;
            CodexData = codexData;
            InventoryData = inventoryData;
        }
    }
}