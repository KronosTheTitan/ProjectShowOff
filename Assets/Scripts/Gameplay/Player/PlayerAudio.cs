using System;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private PlayerCombat combat;

        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip frogKickSfx;
        [SerializeField] private AudioClip tongueHookSfx;
        [SerializeField] private AudioClip vocalSackSfx;
        private void Start()
        {
            combat.OnKick += KickAudio;
            combat.OnTonguePull += TongueHookAudio;
            combat.OnVocalSack += VocalSackAudio;
        }

        private void KickAudio()
        {
            audioSource.clip = frogKickSfx;
            audioSource.Play();
        }

        private void TongueHookAudio()
        {
            audioSource.clip = vocalSackSfx;
            audioSource.Play();
        }

        private void VocalSackAudio()
        {
            audioSource.clip = tongueHookSfx;
            audioSource.Play();
        }
    }
}