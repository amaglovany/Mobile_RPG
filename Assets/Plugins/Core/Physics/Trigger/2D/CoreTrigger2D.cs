using UnityEngine;

namespace TDC
{
    [AddComponentMenu("TDC/Physics/Trigger2D")]
    public class CoreTrigger2D : CoreVisibleTrigger
    {
        #region Unity

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            OnEnter(col.transform);
        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            OnExit(col.transform);
        }

        #endregion
    }
}