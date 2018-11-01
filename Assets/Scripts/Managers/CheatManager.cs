using System.Security.Policy;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public bool FPS_SHOW;

    public bool GOD_MODE;
    public bool LIFESTEAL;

    public bool FAST_TESTING;

    private void InitializePlayerPrefs()
    {
        FPS_SHOW = PlayerPrefs.GetInt("FPS_SHOW") == 1;
        GOD_MODE = PlayerPrefs.GetInt("GOD_MODE") == 1;
        LIFESTEAL = PlayerPrefs.GetInt("LIFESTEAL") == 1;
        FAST_TESTING = PlayerPrefs.GetInt("FAST_TESTING") == 1;
    }

    public void SetGOD_MODE(bool value)
    {
        GOD_MODE = value;

        PlayerPrefs.SetInt("GOD_MODE", value ? 1 : 0);
    }

    public void SetFPS_SHOW(bool value)
    {
        FPS_SHOW = value;

        PlayerPrefs.SetInt("FPS_SHOW", value ? 1 : 0);
    }

    public void SetLIFESTEAL(bool value)
    {
        LIFESTEAL = value;

        PlayerPrefs.SetInt("LIFESTEAL", value ? 1 : 0);
    }

    public void SetFAST_TESTING(bool value)
    {
        FAST_TESTING = value;
        SpawnManager.Instance.SetDeadBodyDeleteDuration(FAST_TESTING ? (uint) 0 : (uint) 5);

        PlayerPrefs.SetInt("FAST_TESTING", value ? 1 : 0);
    }

    #region Singleton

    public static CheatManager Instance;

    private void Awake()
    {
        Instance = this;

        InitializePlayerPrefs();
    }

    #endregion
}