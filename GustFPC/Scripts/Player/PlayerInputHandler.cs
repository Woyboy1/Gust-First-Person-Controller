using UnityEngine;
using UnityEngine.InputSystem;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// Reads and exposes the input actions of the Player, utilizing the 
    /// new Input System. This is considered a core script, do not remove.
    /// </summary>
    public class PlayerInputHandler : PlayerComponent
    {
        [Header("Movement")]
        [Space(5)]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference sprintAction;

        [Header("Interaction")]
        [Space(5)]
        [SerializeField] private InputActionReference interactAction;

        public Vector2 MoveInput => moveAction.action.ReadValue<Vector2>();
        public Vector2 LookInput => lookAction.action.ReadValue<Vector2>();
        public bool JumpPressed => jumpAction.action.WasPressedThisFrame();
        public bool SprintHeld => sprintAction.action.IsPressed();
        public bool InteractPressed => interactAction.action.IsPressed();

        private void OnEnable()
        {
            EnableAction(moveAction);
            EnableAction(lookAction);
            EnableAction(jumpAction);
            EnableAction(sprintAction);
            EnableAction(interactAction);
        }

        private void OnDisable()
        {
            DisableAction(moveAction);
            DisableAction(lookAction);
            DisableAction(jumpAction);
            DisableAction(sprintAction);
            DisableAction(interactAction);
        }

        private void EnableAction(InputActionReference actionRef)
        {
            if (actionRef != null && actionRef.action != null)
                actionRef.action.Enable();
            else
                Debug.LogWarning("PlayerInputHandler.cs: " + actionRef + " was null");
        }

        private void DisableAction(InputActionReference actionRef)
        {
            if (actionRef != null && actionRef.action != null)
                actionRef.action.Disable();
            else
                Debug.LogWarning("PlayerInputHandler.cs: " + actionRef + " was null");
        }
    }
}