using System.Collections;
using TDC.Audio;
using UnityEngine;

namespace TDC.UI
{
    [AddComponentMenu("TDC/UI/WindowControl")]
    public class WindowControl : CoreUI
    {
        private const string ANIMATOR_STATE_ENABLE = "Enable";

        #region Data

        public enum TTypeWindow
        {
            MainMenu,
            Levels,
            Settings,
            Ads,
            Shop,
            Gameplay,
            EndGame,
            NextLevel,
            Respawn,
            Promo,
            BuyUI,
            Bank,
            FreeKnife
        }

        public enum TTypePriority
        {
            Singleton,
            Multiple
        }

        [Header("Data Window")] public TTypeWindow typeWindow;

        public TTypePriority typePriority;
        public bool initState;
        public bool findCameraToCanvas;

        public WindowManager windowManager { get; private set; }
        public GameObject main { get; private set; }
        public Canvas canvas { get; private set; }
        public Animator animControl { get; private set; }
        public bool soundActives = true;

        #endregion

        #region Core

        public virtual void Initialization(WindowManager _windowManager)
        {
            main = gameObject;
            canvas = GetComponent<Canvas>();
            animControl = GetComponent<Animator>();

            if (findCameraToCanvas && canvas)
            {
                canvas.worldCamera = Camera.main;
                canvas.planeDistance = 1;
            }

            windowManager = _windowManager;
        }

        public virtual void CoreUpdate()
        {
        }

        public virtual void ShowState(bool state)
        {
            if (animControl)
            {
                if (state)
                {
                    SetActive(main, true);
                }
                else if (!state && main.activeSelf)
                {
                    StartCoroutine(DelayClose(state, animControl.GetCurrentAnimatorStateInfo(0).length));
                }

                animControl.SetBool(ANIMATOR_STATE_ENABLE, state);
            }
            else
            {
                SetActive(main, state);
            }
        }

        public virtual bool GetState()
        {
            if (animControl)
            {
                return animControl.GetBool(ANIMATOR_STATE_ENABLE);
            }

            return main.activeSelf;
        }

        public virtual void Open()
        {
            Core.Instance.CoreAudio.PlayOneShot(CoreAudio.DataSound.TType.SwitchWindow);
        }

        public virtual void Close()
        {
            Core.Instance.CoreAudio.PlayOneShot(CoreAudio.DataSound.TType.SwitchWindow);
        }

        public IEnumerator DelayClose(bool state, float time)
        {
            yield return new WaitForSeconds(time);

            SetActive(main, state);
        }

        private void OnDestroy()
        {
            if (windowManager)
            {
                windowManager.ListWindow.Remove(this);
            }
        }

        #endregion
    }
}