using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponData : MonoBehaviour
{
    public Weapon weaponData;
    public List<AudioClip> listSounds = new List<AudioClip>();

    private AudioSource aSource;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        aSource.clip = listSounds[Random.Range(0, listSounds.Count)];
        aSource.Play();
    }
}