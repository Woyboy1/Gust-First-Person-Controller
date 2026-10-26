using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// Stores references to all required dependencies for the Player to function properly.
    /// Absolutely leave this class alone. Do not remove this script and all the interfaces
    /// that workks with it. This is considered a core script, do not remove.
    /// </summary>
    public class PlayerDependencies : MonoBehaviour
    {
        [Header("Scripts")]
        [Space(5)]
        [SerializeField] private PlayerMovement movementController;
        [SerializeField] private PlayerCameraController cameraController;
        [SerializeField] private PlayerInteractionController interactionController;
        [SerializeField] private Headbob headBobController;
        [SerializeField] private PlayerInputHandler playerInputHandler;
        [SerializeField] private PlayerSoundController soundController;

        [Header("Other Components")]
        [Space(5)]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Camera playerCamera;

        public CharacterController CharacterController => characterController;
        public PlayerInputHandler PlayerInputHandler => playerInputHandler;
        public PlayerSoundController PlayerSoundController => soundController;
        public Transform CameraPivot => cameraPivot;
        public Transform CameraHolder => cameraHolder;
        public Camera PlayerCamera => playerCamera;

        public PlayerMovement MovementController => movementController;
        public PlayerCameraController CameraController => cameraController;
        public PlayerInteractionController InteractionController => interactionController;
        public Headbob HeadBobController => headBobController;
    }
}