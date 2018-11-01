using UnityEngine;

namespace TDC
{
    [AddComponentMenu("TDC/Physics/Trigger")]
    public class CoreTrigger : CoreVisibleTrigger
    {
        #region Unity

        protected virtual void OnTriggerEnter(Collider col)
        {
            OnEnter(col.transform);
        }

        protected virtual void OnTriggerExit(Collider col)
        {
            OnExit(col.transform);
        }

        #endregion
    }
}