using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public int xPosition;
	public int zPosition;
	public bool isFilled;
	private float lastClickTime =0;
	float catchTime=0.5f;

	private GameObject gameController;
	// Use this for initialization
	void Start () {

		gameController = GameObject.Find ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//If user presses on the tile & the current user has moves left, move him to that tile over the network 
	//by calling the move method in the player script
	void OnMouseDown(){
	
		if(Time.time-lastClickTime<catchTime){
			//double click
			print("done:"+(Time.time-lastClickTime).ToString());
			if (!gameController.GetComponent<GameController>().currentPlayer.GetComponent<Player>().waiting) {
				gameController.GetComponent<GameController>().currentPlayer.GetComponent<PhotonView>().RPC ("MoveTo", PhotonTargets.All, transform.position);
				gameController.GetComponent<GameController>().currentPlayer.GetComponent<Player>().anim.SetFloat("Speed", 4.0f);
				
			}
		}else{
			//normal click
			print("miss:"+(Time.time-lastClickTime).ToString());
		}
		lastClickTime=Time.time;
	
	}
}
