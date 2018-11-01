using System.Collections;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject hitBlockEffect;

    private Coroutine effectCoroutine;

    private void ActivateEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, float duration)
    {
        effectCoroutine = StartCoroutine(ActivateEffectRoutine(effectPrefab, position, rotation, duration));
    }

    private IEnumerator ActivateEffectRoutine(GameObject effectPrefab, Vector3 position, Quaternion rotation,
        float duration)
    {
        GameObject instancedEffect = Instantiate(effectPrefab, position, rotation);
        yield return new WaitForSeconds(duration);
        Destroy(instancedEffect);
    }

    public void ActivateBloodEffect(Transform parentTransform)
    {
        ActivateEffect(bloodEffect, parentTransform.position, parentTransform.rotation, 3f);
    }

    public void ActivateHitBlockEffect(Transform parentTransform)
    {
        ActivateEffect(hitBlockEffect, parentTransform.position, parentTransform.rotation, 3f);
    }

    #region Singleton

    public static EffectsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
}