using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : Fightable {
	
	public float enemyMaxHealth;
	public float enemyHealth;
	public float enemyAttack;
	public float enemyDefense;
	public float enemyIntelligence;
	private GameObject gameController;
	public string enemyName;
	public bool DM;
	
	void Start(){
		
		gameController = GameObject.Find ("GameController");
		xPosition = "0";
		zPosition = "0";
		enemyName = "Enemy" + Random.Range(-100000, 100000);
		enemyMaxHealth = 10.0f;
		enemyAttack = 2.0f;
		enemyDefense = 2.0f;
		enemyIntelligence = 2.0f;
		if (GetComponent<PhotonView> ().isMine) {
			GetComponent<PhotonView>().RPC ("SyncEnemyStats", PhotonTargets.AllBuffered,	//Sync the enemy's first stats from the character builder
			                                enemyMaxHealth,
			                                enemyAttack,
			                                enemyDefense,
			                                enemyIntelligence,
			                                enemyName);
			gameController.GetComponent<PhotonView>().RPC ("OrderTurnQueue", PhotonTargets.All); //Will make a call to insert this enemy into the turns queue
			
		}
		
		
	}
	
	void Update(){
	
			UpdateText();
		
	}
	
	public void UpdateText(){
	
		enemyHealth = health;
		transform.GetChild (0).GetComponent<TextMesh> ().text = "" + enemyHealth;
		
	}
	
	//Called from the GameController when the player's turn starts
	public void StartNewTurn(){
		movesLeft = 2;
		attacksLeft = 1;
		gameController.GetComponent<GameController>().currentPlayer = transform.gameObject;
	}
	
	//Syncs this player's stats over the network
	[PunRPC]
	void SyncEnemyStats(float h, float a, float d, float i, string charName){
		GetComponent<Enemy>().enemyMaxHealth = h;
		GetComponent<Enemy>().enemyHealth = h;
		health = h;
		GetComponent<Enemy>().enemyAttack = a;
		attack = a;
		GetComponent<Enemy>().enemyDefense = d;
		defense = d;
		GetComponent<Enemy>().enemyIntelligence = i;
		transform.GetChild (0).GetComponent<TextMesh> ().text = "" + enemyHealth;
		transform.name = charName;
		
	}
	
}