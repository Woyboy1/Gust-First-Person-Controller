using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// The main camera controller function handling the first person
    /// controller POV. This is considered a core script, do not remove.
    /// </summary>
    public class PlayerCameraController : PlayerComponent
    {
        #region Settings
        [Header("Look Settings")]
        [Space(5)]
        [SerializeField, Tooltip("Mouse sensitivity field, control the speed of the POV")] private float sensitivity = 0.1f;
        [SerializeField, Tooltip("Minimum pitch of how low the player can look down")] private float minPitch = -90f;
        [SerializeField, Tooltip("Maximum pitch of how high the player can look up")] private float maxPitch = 90f;

        [Header("Camera Tilt")]
        [Space(5)]
        [SerializeField, Tooltip("The maximum angle the camera's Z-Axis tilts to")] private float tiltAngle = 5f;
        [SerializeField, Tooltip("The speed it takes to tilt the camera and back")] private float tiltSpeed = 5f;
        #endregion

        #region Variables
        private float pitch;

        // Dependencies
        private PlayerInputHandler InputHandler => Dependencies.PlayerInputHandler;
        private Transform CameraPivot => Dependencies.CameraPivot;
        private Transform CameraHolder => Dependencies.CameraHolder;
        #endregion

        #region Unity Methods
        private void Start()
        {

        }

        private void Update()
        {
            if (Dependencies == null)
                return;

            HandleLook();
            HandleCameraTilt();
        }
        #endregion

        #region Camera
        /// <summary>
        /// Handling the Player POV look of the camera. Does
        /// not handle the head tilt.
        /// </summary>
        private void HandleLook()
        {
            Vector2 lookInput = InputHandler.LookInput;

            float mouseX = lookInput.x * sensitivity;
            float mouseY = lookInput.y * sensitivity;

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            CameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        /// <summary>
        /// Handling the head tilting when the player mov es to the
        /// sides, shifting the camera's local Z axis. 
        /// </summary>
        private void HandleCameraTilt()
        {
            float horizontalInput = InputHandler.MoveInput.x;

            float targetTilt = -horizontalInput * tiltAngle;

            Quaternion targetRotation = Quaternion.Euler(
                0f,
                0f,
                targetTilt
            );

            CameraHolder.localRotation = Quaternion.Slerp(
                CameraHolder.localRotation,
                targetRotation,
                tiltSpeed * Time.deltaTime
            );
        }
        #endregion
    }
}