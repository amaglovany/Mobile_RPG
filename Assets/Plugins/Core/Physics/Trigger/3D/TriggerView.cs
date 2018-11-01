using UnityEngine;

namespace TDC
{
    [AddComponentMenu("TDC/Physics/TriggerView")]
    public class TriggerView : CoreVisibleTrigger
    {
        #region Unity

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnEnter(collision.transform);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            OnEnter(collision.collider.transform);
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            OnExit(collision.collider.transform);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            OnExit(collision.transform);
        }

        #endregion
    }
}