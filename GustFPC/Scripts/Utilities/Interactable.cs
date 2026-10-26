using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace GustFPC
{
    /// <summary>
    /// A base script for interactable incase you want to have
    /// shared features. Recommended if you find yourself using
    /// the same features with a bunch of other scripts such as
    /// playing a sound at the press of a button, etc.
    /// </summary>
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [SerializeField] private bool canInteract = true;
        [SerializeField] private bool disableAfterInteraction;

        [Header("Debug Settings")]
        [SerializeField] private bool logInteraction;
        [SerializeField] private string debugMessage = "Interacted";

        [Header("Audio Settings")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip interactionSound;

        [Header("Cooldown Settings")]
        [SerializeField] private bool useCooldown;
        [SerializeField, Min(0f)] private float interactionCooldown = 1f;

        [Header("Events")]
        [SerializeField] private UnityEvent onInteract;

        public bool CanInteract => canInteract;

        private float nextInteractionTime;

        public void Interact()
        {
            if (!canInteract)
                return;

            if (useCooldown && Time.time < nextInteractionTime)
                return;

            if (useCooldown)
                nextInteractionTime = Time.time + interactionCooldown;

            if (disableAfterInteraction)
                canInteract = false;

            if (logInteraction)
                Debug.Log(debugMessage, this);

            if (audioSource != null && interactionSound != null)
                audioSource.PlayOneShot(interactionSound);

            onInteract?.Invoke();

            OnInteract();
        }

        /// <summary>
        /// Called when the object is interacted with.
        /// Override this method to add custom behavior.
        /// </summary>
        protected virtual void OnInteract()
        {

        }
    }
}