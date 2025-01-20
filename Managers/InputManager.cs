using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Maledictus
{
    public class InputManager : MonoBehaviour
    {
        public static event Action<Vector2> OnMove;
        public static event Action<Vector2> OnMoveStarted;
        public static event Action<Vector2> OnMoveEnded;

        public static event Action<bool> OnSprintStarted;
        public static event Action<bool> OnSprintEnded;

        public static event Action OnInteractionPerformed;
        public static event Action OnInventoryToggled;
        public static event Action OnMenuToggled;
        public static event Action OnDialogueToggled;

        public static event Action OnEquipItem;
        public static event Action OnUnequipItem;
        public static event Action OnDropItem;

        private PlayerInput _playerInput;

        private InputAction _moveAction;
        private InputAction _moveUpAction;
        private InputAction _moveDownAction;
        private InputAction _moveRightAction;
        private InputAction _moveLeftAction;

        private InputAction _sprintAction;
        private InputAction _interactionAction;
        private InputAction _inventoryAction;
        private InputAction _menuOpenCloseAction;
        private InputAction _dialogueAction;

        private InputAction _equipItemAction;
        private InputAction _unequipItemAction;
        private InputAction _dropItemAction;

        private void OnEnable() => RegisterEventInput();
        private void OnDisable() => DeregisterEventInput();

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            SetupInputActions();
        }

        private void SetupInputActions()
        {
            _moveAction = _playerInput.actions["Move"];

            _moveUpAction = _playerInput.actions["MoveUp"];
            _moveDownAction = _playerInput.actions["MoveDown"];
            _moveRightAction = _playerInput.actions["MoveRight"];
            _moveLeftAction = _playerInput.actions["MoveLeft"];

            _sprintAction = _playerInput.actions["Sprint"];
            _interactionAction = _playerInput.actions["Interaction"];
            _inventoryAction = _playerInput.actions["Inventory"];
            _menuOpenCloseAction = _playerInput.actions["Menu"];
            _dialogueAction = _playerInput.actions["Dialogue"];

            _equipItemAction = _playerInput.actions["Equip"];
            _unequipItemAction = _playerInput.actions["Unequip"];
            _dropItemAction = _playerInput.actions["Drop"];
        }

        private void RegisterEventInput()
        {
            _moveAction.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());


            _moveUpAction.started += ctx => OnMoveStarted?.Invoke(Vector2.up);
            _moveDownAction.started += ctx => OnMoveStarted?.Invoke(Vector2.down);
            _moveRightAction.started += ctx => OnMoveStarted?.Invoke(Vector2.right);
            _moveLeftAction.started += ctx => OnMoveStarted?.Invoke(Vector2.left);

            _moveUpAction.canceled += ctx => OnMoveEnded?.Invoke(Vector2.up);
            _moveDownAction.canceled += ctx => OnMoveEnded?.Invoke(Vector2.down);
            _moveRightAction.canceled += ctx => OnMoveEnded?.Invoke(Vector2.right);
            _moveLeftAction.canceled += ctx => OnMoveEnded?.Invoke(Vector2.left);

            _sprintAction.started += ctx => OnSprintStarted?.Invoke(ctx.ReadValueAsButton());
            _sprintAction.canceled += ctx => OnSprintEnded?.Invoke(ctx.ReadValueAsButton());

            _interactionAction.started += ctx => OnInteractionPerformed?.Invoke();
            _inventoryAction.started += ctx => OnInventoryToggled?.Invoke();
            _menuOpenCloseAction.started += ctx => OnMenuToggled?.Invoke();
            _dialogueAction.started += ctx => OnDialogueToggled?.Invoke();

            _equipItemAction.started += ctx => OnEquipItem?.Invoke();
            _unequipItemAction.started += ctx => OnUnequipItem?.Invoke();
            _dropItemAction.started += ctx => OnDropItem?.Invoke();
        }

        private void DeregisterEventInput()
        {
            _moveUpAction.started -= ctx => OnMoveStarted?.Invoke(Vector2.up);
            _moveDownAction.started -= ctx => OnMoveStarted?.Invoke(Vector2.down);
            _moveRightAction.started -= ctx => OnMoveStarted?.Invoke(Vector2.right);
            _moveLeftAction.started -= ctx => OnMoveStarted?.Invoke(Vector2.left);
            
            _moveUpAction.canceled -= ctx => OnMoveEnded?.Invoke(Vector2.up);
            _moveDownAction.canceled -= ctx => OnMoveEnded?.Invoke(Vector2.down);
            _moveRightAction.canceled -= ctx => OnMoveEnded?.Invoke(Vector2.right);
            _moveLeftAction.canceled -= ctx => OnMoveEnded?.Invoke(Vector2.left);
            
            _sprintAction.started -= ctx => OnSprintStarted?.Invoke(ctx.ReadValueAsButton());
            _sprintAction.canceled -= ctx => OnSprintEnded?.Invoke(ctx.ReadValueAsButton());
            
            _interactionAction.started -= ctx => OnInteractionPerformed?.Invoke();
            _inventoryAction.started -= ctx => OnInventoryToggled?.Invoke();
            _menuOpenCloseAction.started -= ctx => OnMenuToggled?.Invoke();
            _dialogueAction.started -= ctx => OnDialogueToggled?.Invoke();

            _equipItemAction.started -= ctx => OnEquipItem?.Invoke();
            _unequipItemAction.started -= ctx => OnUnequipItem?.Invoke();
            _dropItemAction.started -= ctx => OnDropItem?.Invoke();
        }
    }
}