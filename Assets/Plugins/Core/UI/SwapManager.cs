using System.Collections.Generic;
using TDC.InputSystem;
using UnityEngine;

namespace TDC.UI
{
    [AddComponentMenu("TDC/UI/Mobile/SwapManager")]
    public class SwapManager : CoreUI
    {
        public Transform content;

        [Header("Data")] [Range(0, 1)] public float lerp = 0.15f;

        private Vector3 lerpPosition;
        public List<GameObject> listContents = new List<GameObject>();
        public int step;

        #region Core

        public virtual void Initialization()
        {
            InputManager.Instance.swap += Swap;
        }

        public virtual void CoreUpdate()
        {
            SwapControl();
        }

        private void SwapControl()
        {
            if (!content)
            {
                return;
            }

            if (listContents.Count == 0)
            {
                return;
            }

            if (Mathf.Abs(-listContents[step].transform.localPosition.x - content.localPosition.x) <= 2)
            {
                return;
            }

            if (step > listContents.Count - 1 || step < 0)
            {
                return;
            }

            lerpPosition = new Vector3(-listContents[step].transform.localPosition.x, 0, 0);

            content.localPosition = Vector3.Lerp(content.localPosition, lerpPosition, lerp);
        }

        public void SwapForest(int _step)
        {
            if (!content)
            {
                return;
            }

            if (listContents.Count == 0)
            {
                return;
            }

            if (step > listContents.Count - 1 || step < 0)
            {
                return;
            }

            step = _step;
            content.localPosition = new Vector3(-listContents[step].transform.localPosition.x, 0, 0);
        }

        public void SwapLerp(int _step)
        {
            if (!content)
            {
                return;
            }

            if (listContents.Count == 0)
            {
                return;
            }

            if (step > listContents.Count - 1 || step < 0)
            {
                return;
            }

            step = _step;
        }

        private void Swap()
        {
            var direction = InputManager.Instance.updatePosition.x;

            if (direction > 0 && step < listContents.Count - 1)
            {
                step++;
            }
            else if (direction < 0 && step > 0)
            {
                step--;
            }
        }

        #endregion
    }
}