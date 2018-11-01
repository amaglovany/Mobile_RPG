using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private readonly float ambientMusicSaveTime = 50f;

    private readonly float battleMusicSaveTime = 10f;
    private AudioSource[] allAudioSources;

    [SerializeField] private AudioClip ambientAudioClip;

    [Header("Exploration")] [SerializeField]
    private AudioSource ambientAudioSource;

    [SerializeField] private AudioSource ambientMusicAudioSource;

    [SerializeField] private AudioClip battleAudioClip;

    [Header("Battle")] [SerializeField] private AudioSource battleMusicAudioSource;

    [SerializeField] private AudioClip diabloDeathLaughAudioClip;

    [Header("Interface")] [SerializeField] private AudioSource interfaceAudioSource;

    [SerializeField] private AudioClip weaponChangeAudioClip;

    [SerializeField] private AudioClip windAudioClip;

    [SerializeField] private AudioClip windowAppearAudioClip;

    private void SetClip(AudioSource aSource, AudioClip newClip)
    {
        aSource.clip = newClip;
    }

    #region OnDeath

    public void OnDeathSound()
    {
        foreach (AudioSource audioSource in allAudioSources) audioSource.enabled = false;

        interfaceAudioSource.enabled = true;
        interfaceAudioSource.PlayOneShot(diabloDeathLaughAudioClip);
    }

    #endregion

    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;

        allAudioSources = GetComponentsInChildren<AudioSource>();
    }

    #endregion

    #region Battle

    public void BattleSoundChange(bool battleState)
    {
        if (battleState)
        {
            SmoothClipSwitch(ambientMusicAudioSource, battleMusicAudioSource, ambientMusicAudioSource.volume,
                battleMusicSaveTime);
        }
        else
        {
            SmoothClipSwitch(battleMusicAudioSource, ambientMusicAudioSource, battleMusicAudioSource.volume,
                ambientMusicSaveTime);
        }
    }

    public bool BattleVolumeIsOff()
    {
        return battleMusicAudioSource.volume == 0.0f;
    }

    public bool AmbientVolumeIsOff()
    {
        return ambientMusicAudioSource.volume == 0.0f;
    }

    #endregion

    #region Interface

    public void WeaponChangeSound()
    {
        interfaceAudioSource.PlayOneShot(weaponChangeAudioClip);
    }

    public void WindowAppearSound()
    {
        interfaceAudioSource.PlayOneShot(windowAppearAudioClip, 1f);
    }

    #endregion

    #region SmoothClipSwitch

    private Coroutine smoothClipSwitchCoroutine;

    public void SmoothClipSwitch(AudioSource oldSource, AudioSource newSource, float newSourceVolumeLevel,
        float saveNewClipDuration)
    {
        if (smoothClipSwitchCoroutine != null)
        {
            StopCoroutine(smoothClipSwitchCoroutine);
        }

        smoothClipSwitchCoroutine =
            StartCoroutine(SmoothClipSwitchRoutine(oldSource, newSource, newSourceVolumeLevel, saveNewClipDuration));
    }

    private float timeSinceNewPlaybackStarted;

    private IEnumerator SmoothClipSwitchRoutine(AudioSource oldSource, AudioSource newSource,
        float newSourceVolumeLevel, float saveNewClipDuration)
    {
        newSource.volume = 0;

        var oldSourceVolume = oldSource.volume;
        var newSourceVolume = newSource.volume;

        if (Time.timeSinceLevelLoad - timeSinceNewPlaybackStarted < saveNewClipDuration)
        {
            newSource.Play();
        }
        else
        {
            newSource.Stop();
            newSource.Play();
        }

        timeSinceNewPlaybackStarted = Time.timeSinceLevelLoad;

        // Fade in
        while (oldSource.volume > newSourceVolume && newSource.volume < newSourceVolumeLevel)
        {
            yield return new WaitForSeconds(0.25f);
            oldSource.volume -= 0.01f;
            newSource.volume += 0.01f;
        }

        oldSource.volume = 0;
        newSource.volume = oldSourceVolume;

        oldSource.Pause();
    }

    #endregion

    #region BattleStateMusicControl

    private bool oneSwitchBattleTrue;
    private bool oneSwitchBattleFalse;

    public void BattleMusicControl(bool inBattle)
    {
        if (inBattle)
        {
            if (!oneSwitchBattleTrue && BattleVolumeIsOff())
            {
                BattleSoundChange(true);
                oneSwitchBattleTrue = true;
                oneSwitchBattleFalse = false;
            }
        }
        else
        {
            if (!oneSwitchBattleFalse && AmbientVolumeIsOff())
            {
                BattleSoundChange(false);
                oneSwitchBattleFalse = true;
                oneSwitchBattleTrue = false;
            }
        }
    }

    #endregion
}