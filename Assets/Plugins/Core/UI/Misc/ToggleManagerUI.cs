using System.Collections.Generic;

namespace TDC.UI
{
    public class ToggleManagerUI : CoreUI
    {
        #region Unity

        private void Start()
        {
            Initialization();

            if (initState)
            {
                for (var i = 0; i < listToggle.Count; i++)
                    if (i == 0)
                    {
                        listToggle[i].SwitchState(true);
                    }
                    else
                    {
                        listToggle[i].SwitchState(false);
                    }
            }
            else
            {
                for (var i = 0; i < listToggle.Count; i++) listToggle[i].SwitchState(false);
            }
        }

        #endregion

        #region Data

        public bool initState;
        public List<ToggleUI> listToggle = new List<ToggleUI>();

        #endregion

        #region Core

        public void Initialization()
        {
            foreach (ToggleUI item in GetComponentsInChildren<ToggleUI>())
            {
                item.Initialization(this);
                listToggle.Add(item);
            }
        }

        public void Selected(ToggleUI target)
        {
            CoreSelected(target);
        }

        public void Selected(int index)
        {
            if (index > listToggle.Count - 1)
            {
                Core.Instance.DebugLog("ToogleManager Array error", DDebug.TColor.Error);
                return;
            }

            CoreSelected(listToggle[index]);
        }

        private void CoreSelected(ToggleUI target)
        {
            if (listToggle.Count == 0)
            {
                return;
            }

            foreach (ToggleUI Item in listToggle)
                if (Item == target)
                {
                    target.SwitchState(true);
                }
                else
                {
                    Item.SwitchState(false);
                }
        }

        #endregion
    }
}