using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    public GameObject activeObj;
    private Transform localTransform;

    private void Awake()
    {
        localTransform = transform;
    }

    private void Update()
    {
        localTransform.Rotate(Vector3.up * 55 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activeObj.activeSelf) { return; }

        if(other.transform.tag == "Player")
        {
            HealthPlayer hPlayer = other.transform.GetComponent<HealthPlayer>();

            if(hPlayer.currentHealth < hPlayer.maxHealth)
            {
                Activate(hPlayer);
            }
        }
    }

    private void Activate(HealthPlayer targetHealthSystem)
    {
        targetHealthSystem.Heal(25);
        activeObj.SetActive(false);
    }

    public void ResetPoint()
    {
        activeObj.SetActive(true);
    }
}
