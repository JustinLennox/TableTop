using UnityEngine;
using System.Collections;

public class FlyingObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x + 0.1f, 1.5f, transform.position.z);
	}

}
