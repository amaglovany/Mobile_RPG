using UnityEngine;
using System.Collections;

public class Teleportation : MonoBehaviour {

	public GameObject tagobj;
	public GameObject posion;

	void OnTriggerEnter(Collider collider) {
		posion.transform.position = tagobj.transform.position;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
