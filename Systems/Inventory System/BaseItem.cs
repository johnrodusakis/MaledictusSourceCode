using Maledictus.CustomSoap;
using Maledictus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public class BaseItem : MonoBehaviour, IInteractable
    {
        public TestCollectWeaponItem _testWeaponCollection;
        public TestCollectAccessoryItem[] _testCosmeticCollection = new TestCollectAccessoryItem[3];


        [Button] public void AddRandomWeaponItem() => _testWeaponCollection.AddRandomItem();
        [Button] public void AddRandomNecklaceItem() => _testCosmeticCollection[0].AddRandomItem();
        [Button] public void AddRandomBraceletItem() => _testCosmeticCollection[1].AddRandomItem();
        [Button] public void AddRandomRingItem() => _testCosmeticCollection[2].AddRandomItem();

        public string InteractionMessage() => "Collect";
    }

    [System.Serializable]
    public class TestCollectWeaponItem
    {
        public WeaponInventorySO TestInventorySO;
        [SerializeField] private ScriptableEventWeaponItemSO _onWeaponItemCollected;

        private WeaponItemSO SelectRandomItem() => TestInventorySO.ItemDatabaseSO.ItemList[Random.Range(0, TestInventorySO.ItemDatabaseSO.ItemList.Count)];

        public void AddRandomItem()
        {
            var weapon = SelectRandomItem();

            TestInventorySO.AddItem(weapon);
            _onWeaponItemCollected.Raise(weapon);
        }
    }

    [System.Serializable]
    public class TestCollectAccessoryItem
    {
        public AccessoryInventorySO TestInventorySO;
        [SerializeField] private ScriptableEventAccessoryItemSO _onAccessoryItemCollected;

        private AccessoryItemSO SelectRandomItem() => TestInventorySO.ItemDatabaseSO.ItemList[Random.Range(0, TestInventorySO.ItemDatabaseSO.ItemList.Count)];

        public void AddRandomItem()
        {
            var accessory = SelectRandomItem();

            TestInventorySO.AddItem(accessory);
            _onAccessoryItemCollected.Raise(accessory);
        }
    }
}