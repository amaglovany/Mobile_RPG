using UnityEngine;

namespace TDC.UI
{
    [AddComponentMenu("TDC/UI/InitContent")]
    public class InitContent : SwapManager
    {
        [Header("Init")] public GameObject examplePrefab;

        public GameObject exampleToolBox;

        [Header("LootBox")] public ToggleManagerUI toolBoxManager;

        public override void Initialization()
        {
            base.Initialization();

            if (listContents.Count > 0)
            {
                for (var i = 0; i < listContents.Count; i++) InitToolBox();
            }
        }

        public override void CoreUpdate()
        {
            base.CoreUpdate();

            ToolBoxControl();
        }

        public GameObject GetLoad()
        {
            GameObject newPrefab = Instantiate(examplePrefab, content);

            Vector3 fixPosition = newPrefab.transform.localPosition;
            fixPosition.x = Screen.width * 1.5f * (listContents.Count - 1);

            newPrefab.transform.localPosition = fixPosition;

            listContents.Add(newPrefab);

            InitToolBox();

            return newPrefab;
        }

        private void InitToolBox()
        {
            if (!toolBoxManager)
            {
                return;
            }

            if (!exampleToolBox)
            {
                return;
            }

            GameObject newPrefab = Instantiate(exampleToolBox, toolBoxManager.transform);

            var dataToggle = newPrefab.GetComponent<ToggleUI>();

            if (dataToggle)
            {
                dataToggle.Initialization(toolBoxManager);
                toolBoxManager.listToggle.Add(dataToggle);
            }
        }

        private void ToolBoxControl()
        {
            if (!toolBoxManager)
            {
                return;
            }

            if (step > listContents.Count - 1 || step < 0)
            {
                return;
            }

            toolBoxManager.Selected(step);
        }
    }
}