using System.Collections.Generic;
using UnityEngine;

namespace TDC.UI
{
    [AddComponentMenu("TDC/UI/WindowManager")]
    public class WindowManager : CoreUI
    {
        protected virtual void Update()
        {
            CoreUpdate();
        }

        #region Data

        [Header("Data")] [SerializeField] private List<WindowControl> listWindow;

        public List<WindowControl> ListWindow
        {
            get { return listWindow; }
        }

        #endregion

        #region Core

        public virtual void Initialization()
        {
            foreach (WindowControl window in ListWindow)
            {
                window.Initialization(this);
                window.ShowState(window.initState);
            }
        }

        public virtual void CoreUpdate()
        {
            foreach (WindowControl window in ListWindow)
                if (window.gameObject.activeSelf)
                {
                    window.CoreUpdate();
                }
        }

        public virtual void CellWindow(WindowControl.TTypeWindow _typeWindow, bool state = false)
        {
            WindowControl.TTypePriority Priority = GetWindow(_typeWindow).typePriority;

            foreach (WindowControl window in ListWindow)
                if (window.typeWindow == _typeWindow)
                {
                    window.Open();
                    window.ShowState(state);
                    Core.Instance.DebugLog("Cell window " + window.name + " " + state, DDebug.TColor.Default);
                }
                else if (Priority == WindowControl.TTypePriority.Singleton)
                {
                    window.Close();
                    window.ShowState(false);
                }
        }

        public virtual WindowControl GetWindow(WindowControl.TTypeWindow _typeWindow)
        {
            foreach (WindowControl window in ListWindow)
                if (window.typeWindow == _typeWindow)
                {
                    return window;
                }

            return null;
        }

        public virtual bool GetWindowState(WindowControl.TTypeWindow _typeWindow)
        {
            foreach (WindowControl window in ListWindow)
                if (window.typeWindow == _typeWindow)
                {
                    return window.GetState();
                }

            Core.Instance.DebugLog("Window not found", DDebug.TColor.Error);
            return false;
        }

        protected virtual WindowControl.TTypePriority PriotiryWindow(WindowControl.TTypeWindow _TypeWindow)
        {
            foreach (WindowControl window in ListWindow)
                if (window.typeWindow == _TypeWindow)
                {
                    return window.typePriority;
                }

            return WindowControl.TTypePriority.Singleton;
        }

        #endregion
    }
}