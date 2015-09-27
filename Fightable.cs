using UnityEngine;
using System.Collections;

public class Fightable : Moveable {

	public int attacksLeft;
	public float health;
	public float attack;
	public float defense;
	
	//GAMECONTROLLER IS NOT A GLOBAL VARIABLE BECAUSE THIS CODE ISN'T CALLED ON START??

	
	// Use this for initialization
	void Start () {
	
	}
	
	//We tapped on this player, meaning we're attacking it
//	void OnMouseDown(){
//	
//		GameObject gameController = GameObject.Find ("GameController");
//		
//		if (!GetComponent<PhotonView>().isMine &&
//		    gameController.GetComponent<GameController> ().isMyTurn &&
//		    gameController.GetComponent<GameController>().currentPlayer.GetComponent<Fightable>().attacksLeft > 0) {
//		    	gameController.GetComponent<GameController>().targetGameObject = transform.gameObject;
//		    	gameController.GetComponent<GameController>().ShowFightablePanel();
//		    }
//		
//		
//	}
	
	public void Attack(){
	
		
		GameObject gameController = GameObject.Find ("GameController");
		
		if (!GetComponent<PhotonView>().isMine &&
		    gameController.GetComponent<GameController> ().isMyTurn &&
		    gameController.GetComponent<GameController>().currentPlayer.GetComponent<Fightable>().attacksLeft > 0
		    && gameController.GetComponent<GameController>().currentPlayer.GetComponent<Fightable>().GetDistance(xPosition, zPosition) <= 1
		    ) {
			
			gameController.GetComponent<GameController>().currentPlayer.GetComponent<Fightable>().attacksLeft--;
			float damage = gameController.GetComponent<GameController>().currentPlayer.GetComponent<Fightable>().attack - defense;
			GetComponent<PhotonView>().RPC ("AttackFor", PhotonTargets.All, damage);
			gameController.GetComponent<GameController>().HideFightablePanel();
			
			
		}
		
	}
	
	[PunRPC]
	public void AttackFor(float damageDone){
	
		GameObject gameController = GameObject.Find ("GameController");
		if(damageDone > 0){
			Debug.Log("Damage done: " + damageDone);
			health -= damageDone;
			Debug.Log("current health: " + health);
		}else{
			Debug.Log("Whiff!");
			
		}
	}
	
	private int GetDistance(string targetXPos, string targetZPos){
		int returnValue = Mathf.Abs(int.Parse(xPosition) - int.Parse(targetXPos)) + Mathf.Abs(int.Parse (zPosition) - int.Parse (targetZPos));
		Debug.Log("Return value: " + returnValue);
		return returnValue;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
