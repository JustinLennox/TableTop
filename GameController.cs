using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class GameController : MonoBehaviour {

	public bool isMyTurn = false;
	public GameObject myPlayer;
	public GameObject currentPlayer;
	public GameObject endTurnButton;
	public GameObject fightablePanel;
	public GameObject targetGameObject;
	public GameObject characterBuilder;
	public int currentPlayerNumber;
	public Vector3 dragonDestination;
	public Queue<string> turnQueue = new Queue<string>();
	
	// Use this for initialization
	void Start () {
//		if(PlayerPrefs.GetInt("Host") == 1){
//			characterBuilder.SetActive(false);	//Player is the DM, don't need the char builder
//			endTurnButton.SetActive(false); //hide the end turn button because the DM isn't in the turn rotation
//		}else{
//			characterBuilder.SetActive(true);	//Player is a regular player, we need the char builder
//			
//		}
//		Input.location.Start();
//		Input.compass.enabled = true;
//		Input.compass.headingAccuracy
		currentPlayerNumber = 1;
		
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log("Magnetic heading: " + Input.compass.magneticHeading);
//		if(!SystemInfo.deviceModel.Contains("iPad")){
//			Camera.main.transform.rotation = Quaternion.Euler(90, Input.compass.magneticHeading, 0);
//		}

	}
	
	//Dequeues the last player who took a turn and sets the next player in queue to their turn
	[PunRPC]
	public void NextTurn(){
		Debug.Log("Next Turn");
//		string playerToBack = turnQueue.Dequeue();
//		turnQueue.Enqueue(playerToBack);
		currentPlayerNumber++;
		if(currentPlayerNumber == 5){
			currentPlayerNumber = 1;
		}
		CheckTurn();
	}
	
	//Checks the OrderTurnQueue for the top player and sets it as his turn, calling the StartNewTurn method on his
	//Player script and setting the current player as that player
	public void CheckTurn(){
		Debug.Log("Check turn");
		Debug.Log("Player turn: " + currentPlayerNumber);
		isMyTurn = false;
		int deadPlayerCount = 0;
		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
			player.GetComponent<Player>().myTurn = false;
			isMyTurn = false;
			if(player.GetComponent<Player>().dead == true){
				deadPlayerCount++;
			}
			if(deadPlayerCount == 4){
			
			}
		}
		GameObject playa = GameObject.Find ("player" + currentPlayerNumber);
			if(!playa.GetComponent<Player>().dead){
				playa.GetComponent<Player>().StartCoroutine("StartNewTurn");
//				playa.GetComponent<Player>().StartNewTurn();
				playa.GetComponent<Player>().myTurn = true;
				currentPlayer = playa;
			}else{
				NextTurn();
			}
//		Fightable[] fightableObjects = FindObjectsOfType(typeof(Fightable)) as Fightable[];
//		foreach(Object fightableObject in fightableObjects)
//		{
//		
//			
//			GameObject fightable = GameObject.Find(fightableObject.name);
//			fightable.GetComponent<Player>().myTurn = false;
//			
//			if(fightable.name == turnQueue.Peek()){
//				if(fightable.tag == "Player"){
//					Debug.Log("My Turn");
//					fightable.GetComponent<Player>().StartNewTurn();
//					fightable.GetComponent<Player>().myTurn = true;
//				}else if(fightable.tag == "Enemy"){
//					fightable.GetComponent<Enemy>().StartNewTurn();
//				}
//				currentPlayer = fightable;
//			}
//		}
		
	}
	
	//Reorders the turn queue, keeps track of the order of players' turns
	[PunRPC]
	public void OrderTurnQueue(){
		
		GetComponent<CameraManager>().enabled = false;
//		turnQueue.Clear();
//		Fightable[] fightableObjects = FindObjectsOfType(typeof(Fightable)) as Fightable[];
//		foreach(Object fightable in fightableObjects)
//		{
//			turnQueue.Enqueue(fightable.name);
//		}
//		CheckTurn();
	}
	
	//Called when endTurnButton is pressed
	public void endTurn(){
		GetComponent<PhotonView> ().RPC ("NextTurn", PhotonTargets.All);
//		HideFightablePanel();
	}
	
	//Shows attack options when a fightable character is touched
	public void ShowFightablePanel(){
		fightablePanel.SetActive(true);
	}
	
	//Hides attack options
	public void HideFightablePanel(){
		fightablePanel.SetActive(false);
	}
	
	//Attack target game object with the current player when the attack button is pressed
	public void AttackTarget(){
		Debug.Log("attack button pressed");
		if (isMyTurn && currentPlayer.GetComponent<Fightable>().attacksLeft > 0) {
			targetGameObject.GetComponent<Fightable>().Attack();
		}
	}
	
	public void ShowCharacterBuilder(){
		characterBuilder.SetActive(true);	//Player is a regular player, we need the char builder
		
	}
//	
//	IEnumerator DragonSpawner(){
//		for(;;){
//			yield return new WaitForSeconds(2.0f);
//			GetComponent<PhotonView>().RPC ("SpawnDragon", PhotonTargets.All);
//		}
//	}
//	

	[PunRPC]
	public void SetDragonDestination(int randomInt){
		GameObject dragon = GameObject.Find("Dragon");
		if(randomInt % 2 == 0){
			dragon.GetComponent<Dragon>().destination = new Vector3(-30, 0, 0);
		}else{
			dragon.GetComponent<Dragon>().destination = new Vector3(30, 0, 0);
		}
	}
	
	[PunRPC]
	void UpdateDragonHealth(float newHealth){
		GameObject dragon = GameObject.Find("Dragon");
		dragon.GetComponent<Dragon>().health = newHealth;
	}
	
}

