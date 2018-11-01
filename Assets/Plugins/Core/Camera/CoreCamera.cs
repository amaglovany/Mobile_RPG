using TDC.CameraEngine.Effect;
using UnityEngine;

namespace TDC.CameraEngine
{
    [AddComponentMenu("TDC/Camera/CameraEngine")]
    public class CoreCamera : MonoBehaviour
    {
        #region Data

        [Header("Data")] public Vector3 offset;

        public float lerp = 0.1f;
        public bool freezeY;

        [Header("VelocityMode")] public bool velocityMode;

        [Range(0f, 1f)] public float velocityPower;
        private float velocityFactor;

        [Header("Link")] public Transform rootCamera;

        public Transform pivotCamera;
        public CameraShaker cameraShake;
        public RippleEffect rippleEffect;
        private Transform target;
        private Rigidbody2D targetRigidbody;

        private Vector3 fixCamPosition;
        private Vector3 awakeCameraPosition;

        #endregion

        #region Core

        public void Initialization(Transform targetPlayer)
        {
            target = targetPlayer;

            if (awakeCameraPosition == Vector3.zero)
            {
                awakeCameraPosition = rootCamera.position;
            }

            targetRigidbody = target.GetComponent<Rigidbody2D>();
            fixCamPosition = rootCamera.position;

            if (rippleEffect)
            {
                rippleEffect.Initialization();
            }
        }

        private void FixedUpdate()
        {
            if (!target)
            {
                return;
            }

            Movement();
            VelocityMode();
        }

        public void CoreUpdate()
        {
            if (!target)
            {
                return;
            }

            rootCamera.position = Vector3.Lerp(rootCamera.position, fixCamPosition,
                0.15f + targetRigidbody.velocity.magnitude / 80);
        }

        private void Movement()
        {
            if (velocityMode)
            {
                fixCamPosition.x = Mathf.Lerp(fixCamPosition.x, target.transform.position.x,
                    0.4f + targetRigidbody.velocity.magnitude / 80);
            }
            else
            {
                fixCamPosition.x = Mathf.Lerp(fixCamPosition.x, target.transform.position.x, 0.4f);
            }

            if (!freezeY)
            {
                fixCamPosition.y = Mathf.Lerp(fixCamPosition.y, target.transform.position.y * 0.9f, lerp);
            }
            else
            {
                fixCamPosition.y = awakeCameraPosition.y;
            }

            fixCamPosition += offset;
        }

        private void VelocityMode()
        {
            if (!velocityMode)
            {
                return;
            }

            if (!targetRigidbody)
            {
                velocityMode = false;
                return;
            }

            velocityFactor = targetRigidbody.velocity.magnitude * velocityPower;
            velocityFactor = Mathf.Clamp(velocityFactor, 0, 5);


            fixCamPosition.z = Mathf.Lerp(fixCamPosition.z, awakeCameraPosition.z - velocityFactor, lerp);
        }

        public void CoreReset()
        {
            rootCamera.position = awakeCameraPosition;
        }

        #endregion
    }
}