using Maledictus.CustomUI;
using UnityEngine;

namespace Maledictus.Codex
{
    public class CodexCategoryUI : MonoBehaviour 
    {
        public System.Action<bool> OnNewNotification { get; set; }

        private CustomUIFoldout _customFoldout;
        private GameObject _enemyListObject;

        private void Awake()
        {
            _customFoldout = GetComponent<CustomUIFoldout>();
        }

        public void InitializeCategoryUI(string categoryName, GameObject enemyListObject)
        {
            _enemyListObject = enemyListObject;

            _customFoldout.ButtonText.SetText(categoryName);

            OnNewNotification += HandleNewNotification;
            _customFoldout.OnClick += ToggleCategory;
        }

        private void OnDestroy()
        {
            OnNewNotification -= HandleNewNotification;
            _customFoldout.OnClick -= ToggleCategory;
        }

        private void ToggleCategory() => _customFoldout.Toggle(_enemyListObject);
        private void HandleNewNotification(bool isNew) => _customFoldout.NotificationImage.EnableImage(isNew);
    }
}