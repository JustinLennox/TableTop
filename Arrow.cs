using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	public Vector3 forwardVector;
	public Quaternion rotation;
	private float startTime;
	public float arrowDamage;
		
	// Use this for initialization
	void Start () {
		startTime = Time.time;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(forwardVector != null){
			transform.position = Vector3.MoveTowards(transform.position, transform.position + forwardVector, Time.deltaTime * 10.0f);
			transform.rotation = rotation;
		}
		
		if(Time.time - startTime > 5.0f){
			if(GetComponent<PhotonView>().isMine){
				PhotonNetwork.Destroy(transform.gameObject);
			}
		}
	
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag != "Player" && GetComponent<PhotonView>().isMine){
			PhotonNetwork.Destroy(transform.gameObject);
		}
	}
	
	[PunRPC]
	void FlyStraight(Vector3 fVector, Quaternion rotat, float damage){
		Debug.Log("FVector: " + fVector);
		forwardVector = fVector;
		rotation = rotat;
		arrowDamage = damage;
	}
}
