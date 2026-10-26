
using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// The controller for handling all interactions. Serves a purpose
    /// of creating a raycast searching for interactables, and the input
    /// is checked to see if the object has been interacted with.
    /// </summary>
    public class PlayerInteractionController : PlayerComponent
    {
        #region Settings
        [Header("Interaction")]
        [Space(5)]
        [SerializeField, Tooltip("The maximum distance the interaction can reach")] private float interactionDistance = 3f;
        [SerializeField, Tooltip("The layers the raycast can hit")] private LayerMask interactionLayers = ~0;
        #endregion

        #region Variables
        private IInteractable currentInteractable;

        // Dependencies
        private PlayerInputHandler InputHandler => Dependencies.PlayerInputHandler;
        private Transform CameraPivot => Dependencies.CameraPivot;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (Dependencies == null)
                return;

            HandleInteractionDetection();
            HandleInteractionInput();
        }
        #endregion

        #region Interaction
        /// <summary>
        /// Raycasting for interactables containing the Interface IInteractable.
        /// </summary>
        private void HandleInteractionDetection()
        {
            currentInteractable = null;

            Ray ray = new Ray(CameraPivot.position, CameraPivot.forward);

            if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance,
                interactionLayers))
            {
                currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
            }
        }

        /// <summary>
        /// Interaction input based on the raycast being casted by 
        /// HandleInteractionDetection()
        /// </summary>
        private void HandleInteractionInput()
        {
            if (currentInteractable == null)
                return;

            if (InputHandler.InteractPressed)
            {
                currentInteractable.Interact();
            }
        }
        #endregion
    }
}
