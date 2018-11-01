using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpSize : MonoBehaviour
{
    private GameObject currentDecal;
    private Vector3 saveSize;

    public float speed = 0.1f;

    private void Awake()
    {
        currentDecal = gameObject;
        saveSize = currentDecal.transform.localScale;
        currentDecal.transform.localScale = new Vector3(0.01f, saveSize.y, 0.01f);
    }

    private void Update()
    {
        if (currentDecal)
        {
            currentDecal.transform.localScale = Vector3.Lerp(currentDecal.transform.localScale, saveSize, speed);
        }
    }
}
    