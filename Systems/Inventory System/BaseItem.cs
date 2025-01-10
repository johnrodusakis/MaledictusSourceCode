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


        [SerializeField] private List<WeaponItemSO> _itemSOs;

        private WeaponItemSO SelectRandomItem() => _itemSOs[Random.Range(0, _itemSOs.Count)];

        public void AddRandomItem() => TestInventorySO.AddItem(SelectRandomItem());
    }

    [System.Serializable]
    public class TestCollectAccessoryItem
    {
        public AccessoryInventorySO TestInventorySO;


        [SerializeField] private List<AccessoryItemSO> _itemSOs;

        private AccessoryItemSO SelectRandomItem() => _itemSOs[Random.Range(0, _itemSOs.Count)];

        public void AddRandomItem() => TestInventorySO.AddItem(SelectRandomItem());
    }
}