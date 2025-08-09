using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    // Audio Clips configured in Unity Editor
    [SerializeField] private List<AudioClip> jumpSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> specialJumpSounds = new List<AudioClip>();
    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;

    void Awake()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Play a random jump sound
    public void Jump()
    {
        if (jumpSounds.Count > 0)
        {
            AudioClip clip = jumpSounds[Random.Range(0, jumpSounds.Count)];
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("No jump sounds assigned!");
        }
    }

    // Play a random special jump sound
    public void SpecialJump()
    {
        if (specialJumpSounds.Count > 0)
        {
            AudioClip clip = specialJumpSounds[Random.Range(0, specialJumpSounds.Count)];
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("No special jump sounds assigned!");
        }
    }

    // Play death sound
    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("No death sound assigned!");
        }
    }
}