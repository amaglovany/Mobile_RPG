using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAudio : MonoBehaviour
{
    public AudioClip footstepsAudioClip;
    public AudioSource footstepsAudioSource;

    public List<AudioClip> hitAudioClipList;
    public AudioSource hitAudioSource;

    public List<AudioClip> deathAudioClipList;
    public AudioSource deathAduioSource;

    public void PlayFootstep()
    {
        footstepsAudioSource.PlayOneShot(footstepsAudioClip);
    }

    public void PlayHit()
    {
        if (!hitAudioSource.isPlaying) { hitAudioSource.PlayOneShot(hitAudioClipList[Random.Range(0, hitAudioClipList.Count)]); }
    }

    public void PlayDeath()
    {
        deathAduioSource.PlayOneShot(deathAudioClipList[Random.Range(0, deathAudioClipList.Count)]);
    }
}