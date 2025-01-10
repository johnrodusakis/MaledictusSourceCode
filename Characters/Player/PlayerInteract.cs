using Obvious.Soap;
using UnityEditor;
using UnityEngine;

namespace Maledictus.Player
{
    using Maledictus.Interaction;
    using Maledictus.Inventory;

    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private float _interactionRange = 1.5f;

        [Header("Events")]
        [SerializeField] private ScriptableEvent<string> _onInteractableInRange;
        [SerializeField] private ScriptableEventNoParam _onInteractableOutOfRange;

        private IInteractable _currentInteractable;
        //private BaseInventory _playerInventory;

        private void Awake()
        {
            //_playerInventory = GetComponent<BaseInventory>();
        }

        private void OnEnable()
        {
            InputManager.OnInteractionPerformed += HandleInteract;
        }

        private void OnDisable()
        {
            InputManager.OnInteractionPerformed -= HandleInteract;
        }


        void Update()
        {
            CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            var interactable = _currentInteractable;
            var interactableInRange = false;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _interactionRange);

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out interactable))
                {
                    interactableInRange = true;
                    break;
                }
            }

            if (interactableInRange && _currentInteractable == null)
            {
                _currentInteractable = interactable;
                var message = _currentInteractable.InteractionMessage();
                _onInteractableInRange.Raise(message);
            }
            else if (!interactableInRange && _currentInteractable != null)
            {
                _onInteractableOutOfRange.Raise();
                _currentInteractable = null;
            }
        }

        private void HandleInteract()
        {
            //_currentInteractable?.Interact(_playerInventory);
        }


        void OnDrawGizmosSelected()
        {
            Handles.color = new Color(0.5f, 0.5f, 0.25f, 0.5f);
            Handles.DrawWireDisc(transform.position, Vector3.forward, _interactionRange, 5f);
        }
    }
}