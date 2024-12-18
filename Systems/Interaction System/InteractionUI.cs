using DG.Tweening;
using Maledictus.CustomUI;
using Obvious.Soap;
using UnityEngine;

namespace Maledictus.Interaction
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private CustomImage _keyIcon;
        [SerializeField] private CustomText _interactionText;

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEvent<string> _onInteractableInRange;
        [SerializeField] private ScriptableEventNoParam _onInteractableOutOfRange;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            HideInteractionUI();
        }

        private void OnEnable()
        {
            _onInteractableInRange.OnRaised += DisplayInteractionUI;
            _onInteractableOutOfRange.OnRaised += HideInteractionUI;
        }

        private void OnDisable()
        {
            _onInteractableInRange.OnRaised -= DisplayInteractionUI;
            _onInteractableOutOfRange.OnRaised -= HideInteractionUI;
        }

        private void DisplayInteractionUI(string interactionText)
        {
            _canvasGroup.DOFade(1f, 0.1f).SetEase(Ease.Linear);

            _interactionText.SetText(interactionText);
        }

        private void HideInteractionUI()
        {
            _canvasGroup.DOFade(0f, 0.1f).SetEase(Ease.Linear);
        }
    }
}