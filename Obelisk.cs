using UnityEngine;
using System.Collections;

public class Obelisk : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("SpawnHealth");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator SpawnHealth(){
		for(;;){
			yield return new WaitForSeconds(15.0f);
			int randomInt = Random.Range(1, 3);
			foreach(GameObject health in GameObject.FindGameObjectsWithTag("Health")){
				GameObject.Destroy(health);
			}
			GameObject.Instantiate(Resources.Load("Health"), new Vector3(transform.position.x + randomInt, 0.5f, transform.position.z + randomInt), Quaternion.identity);
		}
	}
}
