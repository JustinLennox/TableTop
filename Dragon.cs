using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour {

	public GameObject gameController;
	public Vector3 destination;
	public float health;
	public bool dead;
	public int currentPos;
	
	// Use this for initialization
	void Start () {
		StartCoroutine("MoveToRandomPosition");
		gameController = GameObject.Find ("GameController");
		transform.name = "Dragon";
		destination = new Vector3(-20, 0, 0);
		health = 2.0f;
		dead = false;
		currentPos = 0;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 5.0f);
		transform.rotation = Quaternion.LookRotation(destination);
	}
	
	IEnumerator MoveToRandomPosition(){
		for(;;){
			yield return new WaitForSeconds(15.0f);
			if(gameController.GetComponent<PhotonView>().isMine){
				currentPos++;
				gameController.GetComponent<PhotonView>().RPC ("SetDragonDestination", PhotonTargets.All, currentPos);
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Arrow"){
			if(other.GetComponent<PhotonView>().isMine){
				PhotonNetwork.Destroy(other.gameObject);
			}
			health -= other.GetComponent<Arrow>().arrowDamage;
			gameController.GetComponent<PhotonView>().RPC("UpdateHealth", PhotonTargets.All, health);
			if(health <= 0.0f){
				DragonDie();
				health = 2.0f;
			}
		}
	}
	
	void DragonDie(){
		GameObject.Instantiate(Resources.Load("Health"), new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
	}
}
