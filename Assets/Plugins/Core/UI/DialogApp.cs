using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TDC.UI
{
    public class DialogApp : WindowManager
    {
        [SerializeField] private List<WindowControl> listActiveWindow = new List<WindowControl>();

        public override void CoreUpdate()
        {
            base.CoreUpdate();
        }

        public override void CellWindow(WindowControl.TTypeWindow _typeWindow, bool State = false)
        {
            if (PriotiryWindow(_typeWindow) == WindowControl.TTypePriority.Singleton)
            {
                for (var i = listActiveWindow.Count - 1; i >= 0; i--)
                    if (listActiveWindow[i].typePriority == WindowControl.TTypePriority.Singleton)
                    {
                        if (listActiveWindow[i].soundActives)
                        {
                            listActiveWindow[i].Close();
                        }

                        DestroyWindow(listActiveWindow[i]);
                        listActiveWindow.RemoveAt(i);
                    }

                SpawnWindow(_typeWindow);
            }
            else
            {
                for (var i = listActiveWindow.Count - 1; i >= 0; i--)
                    if (listActiveWindow[i].typeWindow == _typeWindow)
                    {
                        if (listActiveWindow[i].soundActives)
                        {
                            listActiveWindow[i].Close();
                        }

                        DestroyWindow(listActiveWindow[i]);
                        listActiveWindow.RemoveAt(i);
                    }

                SpawnWindow(_typeWindow);
            }

            //base.CellWindow(_typeWindow, State);
        }

        public void DisableAll(WindowControl.TTypePriority priority)
        {
            for (var i = listActiveWindow.Count - 1; i >= 0; i--)
                if (listActiveWindow[i].typePriority == priority)
                {
                    DestroyWindow(listActiveWindow[i]);
                    listActiveWindow.RemoveAt(i);
                }
        }

        public void StateRaycast(bool state)
        {
            foreach (WindowControl window in listActiveWindow)
                if (window.GetComponent<GraphicRaycaster>())
                {
                    window.GetComponent<GraphicRaycaster>().enabled = state;
                }
        }

        public override WindowControl GetWindow(WindowControl.TTypeWindow _TypeWindow)
        {
            foreach (WindowControl window in listActiveWindow)
                if (window.typeWindow == _TypeWindow)
                {
                    return window;
                }

            return base.GetWindow(_TypeWindow);
        }

        private void SpawnWindow(WindowControl.TTypeWindow _TypeWindow)
        {
            GameObject newWindow = Instantiate(GetWindow(_TypeWindow).gameObject);

            var windowControll = newWindow.GetComponent<WindowControl>();
            windowControll.Initialization(this);
            windowControll.ShowState(true);

            if (windowControll.soundActives)
            {
                windowControll.Open();
            }

            listActiveWindow.Add(windowControll);
        }

        private void DestroyWindow(WindowControl _window)
        {
            if (_window)
            {
                Destroy(_window.gameObject);
            }
        }
    }
}