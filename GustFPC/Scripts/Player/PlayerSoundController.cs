using System;
using System.Collections.Generic;
using UnityEngine;

namespace GustFPC
{
    public class PlayerSoundController : PlayerComponent
    {
        [Serializable]
        [HideInInspector]
        private class GroundSoundEntry
        {
            public string groundLayerName;
            public AudioClip footstepSFX;
        }


        [Header("Audiosources")]
        [Space(5)]
        [SerializeField] [Tooltip("Assign your footstep source. This should be assigned" +
            "by default, but if not please assign it now.")] private AudioSource footstepSource;

        [Header("Settings")]
        [Space(5)]
        [SerializeField, Range(0f, 1f), Tooltip("Random Pitch Variance")] private float pitchVariance = 0.5f;
        [SerializeField, Range(0f, 1f), Tooltip("Random Volume Variance")] private float volumeVariance = 0.5f;

        [Header("Ground Sounds")]
        [Space(5)]
        [SerializeField] private List<GroundSoundEntry> groundSounds = new();

        private float defaultPitch;
        private float defaultVolume;

        private void Awake()
        {
            if (footstepSource == null)
            {
                Debug.LogWarning(
                    "PlayerSoundController: Footstep AudioSource is missing.",
                    this);
                return;
            }

            defaultPitch = footstepSource.pitch;
            defaultVolume = footstepSource.volume;
        }

        /// <summary>
        /// Use for playing the footsteps, randomly changing pitch
        /// and volume is active by default;
        /// </summary>
        /// <param name="groundLayerName"></param>
        /// <param name="randomPitch"></param>
        public void PlayFootstep(
            string groundLayerName, 
            bool randomPitch = true, 
            bool randomVolume = true)
        {
            if (string.IsNullOrWhiteSpace(groundLayerName))
            {
                Debug.LogWarning(
                    "PlayerSoundController: Ground layer name is empty.",
                    this);
                return;
            }

            AudioClip clip = GetSoundLayer(groundLayerName);

            if (clip == null)
            {
                Debug.LogWarning(
                    $"PlayerSoundController: No footstep clip assigned for layer '{groundLayerName}'.",
                    this);
                return;
            }

            footstepSource.pitch = randomPitch
                ? defaultPitch * UnityEngine.Random.Range(
                    1f - pitchVariance, 1f + pitchVariance)
                : defaultPitch;

            footstepSource.volume = randomVolume
                ? defaultVolume * UnityEngine.Random.Range(
                    1f - volumeVariance, 1f + volumeVariance)
                : defaultVolume;

            footstepSource.clip = clip;
            footstepSource.Play();
        }

        /// <summary>
        /// Returning audioclip for the ground layer.
        /// </summary>
        /// <param name="groundLayerName"></param>
        /// <returns></returns>
        private AudioClip GetSoundLayer(string groundLayerName)
        {
            foreach (GroundSoundEntry entry in groundSounds)
            {
                if (entry.groundLayerName == groundLayerName)
                    return entry.footstepSFX;
            }

            return null;
        }
    }
}