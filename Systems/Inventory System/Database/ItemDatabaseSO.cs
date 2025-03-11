using Maledictus.Codex;
using Maledictus.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Inventory
{
    public class ItemDatabaseSO<T> : ScriptableObject where T : ItemSO
    {
        public string Path;
        public List<T> ItemList;

        public T FindItemSO(string id) => ItemList.Find(x => x.Id == id);

        private void OnValidate()
        {
            ItemList.Clear();

            var data = Resources.LoadAll($"Maledictus/Items/{Path}");
            foreach (var item in data)
            {
                var itemSO = (T)item;
                ItemList.Add(itemSO);
            }
        }
    }
}