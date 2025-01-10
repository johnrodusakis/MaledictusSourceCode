using Maledictus.CustomUI;
using Obvious.Soap;
using UnityEngine;
using VInspector;

namespace Maledictus.Inventory
{
    public class ItemSortUI : MonoBehaviour
    {
        [Tab("Custom UI")]
        [SerializeField] private CustomText _sortByText;

        [Tab("Events")]
        [SerializeField] private ScriptableEventString _onSortItems;

        private void OnEnable()
        {
            _onSortItems.OnRaised += UpdateSortByText;
        }

        private void OnDisable()
        {
            _onSortItems.OnRaised -= UpdateSortByText;
        }

        private void UpdateSortByText(string sortType) => _sortByText.SetText($"Sort By: {sortType}");
    }
}