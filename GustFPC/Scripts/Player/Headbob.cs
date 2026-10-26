using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// Script that will manage headbobbing. If you do not like this way of
    /// headbobbing your camera, you are free to change what you want. This
    /// is not a Core script so you are free to remove it.
    /// </summary>
    public class Headbob : PlayerComponent
    {
        #region Settings
        [Header("Headbob Settings")]
        [Space(5)]
        [SerializeField, Tooltip("Controls how quickly each bob repeats when moving")] private float bobFrequency = 10f;
        [SerializeField, Tooltip("high amplitude = the player's steps feel heavier, low amplitude" +
            " = more stable footsteps")] private float bobAmplitude = 0.05f;
        [SerializeField, Tooltip("Head bob smoothing, how quickly the camera follows the bob")] private float smoothness = 10f;
        #endregion

        #region Variables
        private Vector3 startingPosition;
        private float bobTimer;

        // Dependencies
        private PlayerInputHandler InputHandler => Dependencies.PlayerInputHandler;
        private CharacterController CharacterController => Dependencies.CharacterController;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            startingPosition = transform.localPosition;
        }

        private void Update()
        {
            HandleHeadbob();
        }
        #endregion

        #region Headbob
        private void HandleHeadbob()
        {
            if (Dependencies == null)
                return;

            bool isMoving = InputHandler.MoveInput.magnitude > 0.1f;
            bool isGrounded = CharacterController.isGrounded;

            if (isMoving && isGrounded)
            {
                bobTimer += Time.deltaTime * bobFrequency;

                Vector3 targetPosition = startingPosition;
                targetPosition.y += Mathf.Sin(bobTimer) * bobAmplitude;

                transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    targetPosition,
                    smoothness * Time.deltaTime);
            }
            else
            {
                // Smoothly return to the starting position.
                bobTimer = 0f;

                transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    startingPosition,
                    smoothness * Time.deltaTime);
            }
        }
        #endregion
    }
}