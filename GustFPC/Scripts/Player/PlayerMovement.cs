
using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// Controller of the player's movement using the resources of the player dependencies.
    /// This is considered a core script, do not remove.
    /// </summary>
    public class PlayerMovement : PlayerComponent
    {
        #region Settings
        [Header("Movement")]
        [SerializeField, Tooltip("Base movement speed")] private float moveSpeed = 5f;
        [SerializeField, Tooltip("Sprinting speed")] private float sprintSpeed = 8f;

        [Header("Jumping")]
        [SerializeField, Tooltip("How high you can jump. Duhh")] private float jumpHeight = 1.5f;
        [SerializeField, Tooltip("Custom gravity, gravity fall speed")] private float gravity = -20f;

        [Header("Step Interval")]
        [SerializeField, Tooltip("Determine each step interval")] private float footstepInterval = 0.5f;
        #endregion

        #region Variables
        private Vector3 verticalVelocity;
        private float footstepTimer;
        private bool IsSprinting => InputHandler.SprintHeld;

        // Dependencies
        private CharacterController CharacterController => Dependencies.CharacterController;
        private PlayerInputHandler InputHandler => Dependencies.PlayerInputHandler;
        private PlayerSoundController SoundController => Dependencies.PlayerSoundController;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (Dependencies == null)
                return;

            HandleMovement();
            HandleGravity();
            HandleJump();
            HandleFootsteps();
        }
        #endregion

        #region Movement
        /// <summary>
        /// Base movement controls.
        /// </summary>
        private void HandleMovement()
        {
            Vector2 input = InputHandler.MoveInput;

            float currentSpeed = IsSprinting ? sprintSpeed : moveSpeed;

            Vector3 moveDirection =
                transform.right * input.x +
                transform.forward * input.y;

            CharacterController.Move(
                moveDirection * currentSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Custom gravity to make it a realistic first person
        /// controller rather than using a rigidbody. 
        /// </summary>
        private void HandleGravity()
        {
            if (CharacterController.isGrounded && verticalVelocity.y < 0f)
            {
                verticalVelocity.y = -2f;
            }

            verticalVelocity.y += gravity * Time.deltaTime;

            CharacterController.Move(
                verticalVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Simple Jump Input.
        /// </summary>
        private void HandleJump()
        {
            if (CharacterController.isGrounded && InputHandler.JumpPressed)
            {
                SoundController.PlayFootstep(GetGroundLayerName());
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        /// <summary>
        /// Step interval is managed here, with a purpose of
        /// playing sounds. 
        /// </summary>
        private void HandleFootsteps()
        {
            if (!CharacterController.isGrounded)
            {
                footstepTimer = 0f;
                return;
            }

            Vector2 input = InputHandler.MoveInput;

            if (input.sqrMagnitude < 0.01f)
            {
                footstepTimer = 0f;
                return;
            }

            float currentInterval = IsSprinting
                ? footstepInterval * (moveSpeed / sprintSpeed)
                : footstepInterval;

            footstepTimer += Time.deltaTime;

            if (footstepTimer >= currentInterval)
            {
                string groundLayerName = GetGroundLayerName();

                if (!string.IsNullOrEmpty(groundLayerName))
                {
                    SoundController.PlayFootstep(groundLayerName);
                }

                footstepTimer = 0f;
            }
        }

        /// <summary>
        /// Returns the name of the Unity layer the player is standing on.
        /// Returns null if no ground layer is detected.
        /// </summary>
        private string GetGroundLayerName()
        {
            Vector3 origin = CharacterController.bounds.center;
            float distance = CharacterController.bounds.extents.y + 0.2f;

            if (Physics.Raycast(
                origin,
                Vector3.down,
                out RaycastHit hit,
                distance,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore))
            {
                return LayerMask.LayerToName(hit.collider.gameObject.layer);
            }

            return null;
        }
        #endregion
    }
}
