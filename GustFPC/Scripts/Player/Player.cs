using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// Acts as the "Master" of the entire controller. Having access to methods to configuring
    /// the right things and controllers working on its own. This is considered a core script,
    /// do not remove.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Settings
        [SerializeField, Tooltip("Mandatory field, should always assigned " +
            "in order for the player to function")]
        private PlayerDependencies dependencies;
        #endregion

        #region Fields
        // Dependencies
        public PlayerDependencies Dependencies => dependencies;
        #endregion

        #region Locking
        private PlayerComponent[] playerComponents;
        private bool[] previousEnabledStates;
        private bool isPlayerLocked;

        public bool IsPlayerLocked => isPlayerLocked;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (dependencies == null)
                dependencies = GetComponent<PlayerDependencies>();

            playerComponents =
                GetComponentsInChildren<PlayerComponent>(true);

            previousEnabledStates =
                new bool[playerComponents.Length];

            foreach (PlayerComponent component in playerComponents)
            {
                if (component != null)
                    component.InitializeDependencies();
            }
        }

        private void Start()
        {
            SetCursorVisible(false);
        }
        #endregion

        /// <summary>
        /// Teleporting the player to a location. 
        /// </summary>
        /// <param name="targetLocation"></param>
        public void TeleportPlayer(Transform targetLocation)
        {
            if (targetLocation == null)
                return;

            CharacterController controller = dependencies.CharacterController;

            if (controller != null)
                controller.enabled = false;

            transform.position = targetLocation.position;

            if (controller != null)
                controller.enabled = true;
        }

        /// <summary>
        /// Locking the player, input is still recieved, but can not move
        /// move the camera, and interact with objects. 
        /// With an optional parameter of hiding/showing the mouse cursor.
        /// </summary>
        public void LockPlayer(bool showCursor = false)
        {
            if (!isPlayerLocked)
            {
                for (int i = 0; i < playerComponents.Length; i++)
                {
                    PlayerComponent component = playerComponents[i];

                    if (component == null)
                        continue;

                    previousEnabledStates[i] = component.enabled;
                    component.enabled = false;
                }

                isPlayerLocked = true;
            }

            SetCursorVisible(showCursor);
        }

        /// <summary>
        /// Undoing the LockPlayer()
        /// </summary>
        public void UnlockPlayer()
        {
            if (isPlayerLocked)
            {
                for (int i = 0; i < playerComponents.Length; i++)
                {
                    PlayerComponent component = playerComponents[i];

                    if (component == null)
                        continue;

                    component.enabled = previousEnabledStates[i];
                }

                isPlayerLocked = false;
            }
        }

        /// <summary>
        /// Setting cursor visibility. 
        /// </summary>
        /// <param name="visible"></param>
        private void SetCursorVisible(bool visible)
        {
            Cursor.visible = visible;

            Cursor.lockState = visible
                ? CursorLockMode.None
                : CursorLockMode.Locked;
        }
    }
}