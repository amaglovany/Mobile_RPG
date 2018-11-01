using System.Collections.Generic;
using UnityEngine;

namespace TDC
{
    public class CoreVisibleTrigger : MonoBehaviour
    {
        #region Unity

        private void OnEnable()
        {
            listObject = new List<Transform>();
        }

        #endregion

        #region Data

        [Header("Data")] public List<string> listTag = new List<string>();

        public List<Transform> listObject;

        #endregion

        #region Core

        public void CoreUpdate()
        {
            ListControl();
        }

        private void ListControl()
        {
            if (listObject.Count == 0)
            {
                return;
            }

            for (var i = listObject.Count - 1; i >= 0; i--)
                if (!listObject[i])
                {
                    listObject.RemoveAt(i);
                }
        }

        protected virtual void OnEnter(Transform target)
        {
            if (!listObject.Contains(target) && CheckTag(target.tag))
            {
                listObject.Add(target);
            }
        }

        protected virtual void OnExit(Transform target)
        {
            if (listObject.Contains(target))
            {
                listObject.Remove(target);
            }
        }

        protected bool CheckTag(string tag)
        {
            if (listTag.Contains(tag))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}