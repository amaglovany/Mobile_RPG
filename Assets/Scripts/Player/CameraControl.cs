using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region Data

    [Header("Data Position")] [Range(0.1f, 15f)]
    public float height = 1.8f;

    [Range(0.1f, 15f)] public float distance = 3.5f;
    [Range(0.01f, 1f)] public float smoothPosition = 0.05f;

    public Vector3 pivotPosition;

    [Header("Data Rotation")] [Range(0.01f, 5f)]
    public float rotateSensitivity = 2.2f; // 1.1f

    [Range(0.01f, 1f)] public float turnSensitivity = 0.4f; // 0.2f
    [Range(0.01f, 1f)] public float smoothRotation = 0.1f; // 0.05f

    public Vector3 rotateDirection;

    [Header("Link")] public Transform parentCamera;

    public Transform pivotCamera;
    public Transform targetCamera;

    public Transform target;

    private Transform localTransform;

    #endregion

    #region Core

    public void Start()
    {
        pivotPosition.y = height;
        pivotPosition.z = -distance;
    }

    public void Update()
    {
        PositionControl();
        RotationControl();
    }

    private void PositionControl()
    {
        pivotCamera.transform.localPosition =
            Vector3.Lerp(pivotCamera.transform.localPosition, pivotPosition, smoothPosition);
    }

    private void RotationControl()
    {
        if (!target)
        {
            Quaternion fixRotationParent = Quaternion.Euler(0, rotateDirection.x, 0);

            parentCamera.transform.rotation =
                Quaternion.Lerp(parentCamera.transform.rotation, fixRotationParent, smoothRotation);

            Quaternion fixTarget =
                Quaternion.LookRotation((parentCamera.position - targetCamera.position).normalized, Vector3.up);
            fixTarget.y = 0;
            fixTarget.z = 0;

            pivotCamera.transform.localRotation =
                Quaternion.Lerp(pivotCamera.transform.localRotation, fixTarget, smoothRotation);
            targetCamera.localRotation = Quaternion.Lerp(targetCamera.localRotation, Quaternion.identity, 0.05f);
        }
        else
        {
            Quaternion fixTarget =
                Quaternion.LookRotation((target.position - targetCamera.position).normalized, Vector3.up);
            Quaternion fixRotationParent = fixTarget;
            fixRotationParent.x = 0;
            fixRotationParent.z = 0;

            Quaternion fixRotationPivot = fixTarget;
            fixRotationPivot.y = 0;
            fixRotationPivot.z = 0;

            parentCamera.transform.rotation =
                Quaternion.Lerp(parentCamera.transform.rotation, fixRotationParent, 0.03f);

            pivotCamera.transform.rotation = Quaternion.Lerp(pivotCamera.transform.rotation, fixRotationPivot, 0.02f);

            targetCamera.rotation = Quaternion.Lerp(targetCamera.rotation, fixTarget, 0.05f);
        }
    }

    #endregion
}