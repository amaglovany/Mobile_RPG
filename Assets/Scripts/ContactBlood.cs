using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBlood : MonoBehaviour
{
    public Transform root;
    public GameObject prefabBloodDecal;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Terrain") { return; }

        Vector3 fixPosition = transform.position;

        if (prefabBloodDecal)
        {
            fixPosition.y = root.position.y;
            Instantiate(prefabBloodDecal, fixPosition, root.rotation);
        }

        HealthEnemy getHealthEnemy = root.GetComponent<HealthEnemy>();

        if (getHealthEnemy)
        {
            getHealthEnemy.isContantGround = true;
            getHealthEnemy.CellParasite(fixPosition);
        }
    }
}
