using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.CustomUI
{
    public abstract class CustomUIComponent : MonoBehaviour
    {
        private void Awake() => Initialize();
        private void OnValidate() => Initialize();

        [Button]
        private void Initialize()
        {
            Setup();
            Configure();
        }

        protected virtual void Setup() { }
        protected abstract void Configure();
    }
}