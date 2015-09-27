using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Fightable {

	public float playerMaxHealth;
	public float playerHealth;
	public float playerAttack;
	public float playerDefense;
	public float playerIntelligence;
	public float playerExp;
	private GameObject gameController;
	public string characterName;
	public float secondsLeft = 0.0f;
	public Animator anim;
	private GameObject turnTimer;
	private GameObject turnLabel;
	public bool DM;
	private Color darkBlue = new Color(0.0f/255.0f, 6.0f/255.0f, 104.0f/255.0f, 0.5f);
	private Color darkRed = new Color(190.0f/255.0f, 0.0f/255.0f, 27.0f/255.0f, 0.7f);
	private Color darkYellow = new Color(255.0f/255.0f, 166.0f/255.0f, 0.0f/255.0f, 0.5f);
	private Color darkGreen = new Color(0.0f/255.0f, 69.0f/255.0f, 10.0f/255.0f, 0.58f);
	public bool myTurn = false;
	public bool gameOver = false;
	public bool dead = false;
	public bool waiting = false;
	private GameObject healthText;
	private float touchStart = 0;

	void Start(){

		gameController = GameObject.Find ("GameController");
		anim = GetComponent<Animator>();
		anim.SetBool("Dead", false);
		anim.SetFloat("Speed", 0.0f);
		xPosition = "0";
		zPosition = "0";
		dead = false;
		gameOver = false;
		secondsLeft = 0.0f;
		myTurn = false;
		playerExp = 0.0f;
//		rigid = GetComponent<Rigidbody>();
		turnTimer = GameObject.Find("TurnTimer");
		turnLabel = GameObject.Find ("TurnLabel");
		healthText = transform.GetChild(0).gameObject;
		if (GetComponent<PhotonView> ().isMine) { //This is for the DM and its Minions. This will always be the player
			GetComponent<PhotonView>().RPC ("SyncPlayerStats", PhotonTargets.AllBuffered,	//Sync the player's first stats from the character builder
			                                playerMaxHealth,
			                                playerAttack,
			                                playerDefense,
			                                playerIntelligence,
			                                characterName);
			
						gameController.GetComponent<PhotonView>().RPC ("OrderTurnQueue", PhotonTargets.All); //Will make a call to insert this player into the turns queue
						if(transform.name == "player1"){
							secondsLeft = 5.0f;
							gameController.GetComponent<GameController>().CheckTurn();
						}
			//Set if the player is the DM
			if(PlayerPrefs.GetInt("Host") == 1){
			
				GetComponent<PhotonView>().RPC ("SetIsDM", PhotonTargets.AllBuffered);
			}else{
				SetIsntDM();
			}
		}
		

	}
	
	void Update(){
		healthText.GetComponent<TextMesh>().text = "" + playerHealth.ToString("F2");
		if(!waiting && !gameOver){
			if(myTurn && secondsLeft >= 0){
				secondsLeft -= Time.deltaTime;
				turnTimer.GetComponent<Image>().fillAmount = secondsLeft/5.0f;
				if(Input.GetMouseButtonUp(1) && !waiting){
					ShootArrow(1.0f);
				
				}
				foreach(Touch t in Input.touches){
					if(t.phase == TouchPhase.Began){
						touchStart = Time.time;
					}
					if(t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled){
						if((Time.time - touchStart) > 0.1f && !waiting){
							float arrowDamage = Time.time - touchStart;
							if(arrowDamage > 2.0f){
								arrowDamage = 2.0f;
							}
							ShootArrow(arrowDamage);
						}
					}
				}
				
				
			}else if(myTurn){
				Debug.Log("ending turn");
				myTurn = false;
				destination = transform.position;
//				rigid.isKinematic = true;
				anim.SetFloat("Speed", 0.0f);
				if(GetComponent<PhotonView>().isMine){
					gameController.GetComponent<GameController>().endTurn();
				}
				
			}
		}
		
	}

	//Called from the GameController when the player's turn starts
	public IEnumerator StartNewTurn(){
		if(!gameOver){
					if(transform.name == "player1"){
						turnLabel.GetComponent<Text>().text = "Red!";
						turnLabel.GetComponent<Text>().color = darkRed;
						turnTimer.GetComponent<Image>().color = darkRed;
					
				}else if(transform.name == "player2"){
						turnLabel.GetComponent<Text>().text = "Blue!";
					turnLabel.GetComponent<Text>().color = darkBlue;
					turnTimer.GetComponent<Image>().color = darkBlue;
					
				}else if(transform.name == "player3"){
						turnLabel.GetComponent<Text>().text = "Green!";
					turnLabel.GetComponent<Text>().color = darkGreen;
					turnTimer.GetComponent<Image>().color = darkGreen;
					
					
				}else if(transform.name == "player4"){
						turnLabel.GetComponent<Text>().text = "Yellow!";
						turnLabel.GetComponent<Text>().color = darkYellow;
					turnTimer.GetComponent<Image>().color = darkYellow;
					
					
				}
					movesLeft = 2;
					attacksLeft = 1;
					waiting = true;
					Debug.Log("Start Wait for " + transform.name);
					yield return new WaitForSeconds(1.5f);
					turnLabel.GetComponent<Text>().text = "Go!";
					yield return new WaitForSeconds(0.5f);
					turnLabel.GetComponent<Text>().text = "";
					secondsLeft = 5.0f;
					myTurn = true;
					waiting = false;
			//		rigid.isKinematic = false;
					anim.SetFloat("Speed", 1.0f);
					StopCoroutine("StartNewTurn");
			}
		
	}
	
	//Syncs this player's stats over the network
	[PunRPC]
	void SyncPlayerStats(float h, float a, float d, float i, string charName){
		GetComponent<Player>().playerMaxHealth = h;
		GetComponent<Player>().playerHealth = h;
		health = h;
		GetComponent<Player>().playerAttack = a;
		attack = a;
		GetComponent<Player>().playerDefense = d;
		defense = d;
		GetComponent<Player>().playerIntelligence = i;
		GetComponent<Player>().characterName = charName;
		transform.name = charName;
		
	}
	
	//Called when the Player is the Dungeon Master
	[PunRPC]
	void SetIsDM(){
	
		DM = true;
		GetComponent<Collider>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		transform.GetChild(0).GetComponent<Renderer>().enabled = false;
		
	
	}
	
	void OnTriggerEnter(Collider other){
		Debug.Log("TRIGGER");
		if(other.gameObject.tag == "Arrow"){
			if(other.GetComponent<PhotonView>().isMine){
				PhotonNetwork.Destroy(other.gameObject);
			}
			playerHealth -= other.GetComponent<Arrow>().arrowDamage;
			GetComponent<PhotonView>().RPC("UpdateHealth", PhotonTargets.All, playerHealth);
			Debug.Log("Current Player Health : " + playerHealth);
			if(playerHealth <= 0.0f){
				PlayerDie();
			}
		}
		if(other.gameObject.tag == "Fire"){
			Debug.Log("Collision");
			playerHealth -= 0.7f;
			GetComponent<PhotonView>().RPC("UpdateHealth", PhotonTargets.All, playerHealth);
			Debug.Log("Current Player Health : " + playerHealth);
			if(playerHealth <= 1){
				PlayerDie();
			}
		}
		if(other.gameObject.tag == "Health"){
			GameObject.Destroy(other.gameObject);
			playerHealth += 0.5f;
			GetComponent<PhotonView>().RPC("UpdateHealth", PhotonTargets.All, playerHealth);
		}
		if(other.gameObject.tag == "Gold"){
			gameOver = true;
			if(transform.name == "player1"){
				turnLabel.GetComponent<Text>().text = "Red Wins!";
				turnLabel.GetComponent<Text>().color = darkRed;
				turnTimer.GetComponent<Image>().color = darkRed;
				
			}else if(transform.name == "player2"){
				turnLabel.GetComponent<Text>().text = "Blue Wins!";
				turnLabel.GetComponent<Text>().color = darkBlue;
				turnTimer.GetComponent<Image>().color = darkBlue;
				
			}else if(transform.name == "player3"){
				turnLabel.GetComponent<Text>().text = "Green Wins!";
				turnLabel.GetComponent<Text>().color = darkGreen;
				turnTimer.GetComponent<Image>().color = darkGreen;
				
				
			}else if(transform.name == "player4"){
				turnLabel.GetComponent<Text>().text = "Yellow Wins!";
				turnLabel.GetComponent<Text>().color = darkYellow;
				turnTimer.GetComponent<Image>().color = darkYellow;
				
				
			}
			foreach(GameObject play in GameObject.FindGameObjectsWithTag("Player")){
				play.GetComponent<Player>().StartCoroutine("StartNewGame");
			}
		}
	}
	
	[PunRPC]
	void UpdateHealth(float newHealth){
		playerHealth = newHealth;
	}
	
	void ShootArrow(float arrowDamage){
		anim.SetTrigger("Shooting");
		GetComponent<AudioSource>().Play();
		GameObject arrow = PhotonNetwork.Instantiate("Arrow", new Vector3(transform.position.x, 2.0f, transform.position.z) + transform.forward, 
		Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0) , 0);
		arrow.GetComponent<PhotonView>().RequestOwnership();
		arrow.GetComponent<PhotonView>().RPC ("FlyStraight", PhotonTargets.All, transform.forward, Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0), arrowDamage);
	}
	
	void PlayerDie(){
		dead = true;
		Debug.Log("Player die");
		anim.SetBool("Dead", true);
		GetComponent<PhotonView>().RPC ("CheckWon", PhotonTargets.All);
	}
	
	[PunRPC]
	void CheckWon(){
		Debug.Log("Check won");
		dead = true;
		int deadPlayerCount = 0;
		foreach(GameObject playa in GameObject.FindGameObjectsWithTag("Player")){
			if(playa.GetComponent<Player>().dead == true){
				deadPlayerCount++;
			}
		}
		Debug.Log("Dead player count: " + deadPlayerCount);
		if(deadPlayerCount == 3){
			foreach(GameObject playa in GameObject.FindGameObjectsWithTag("Player")){
				if(playa.GetComponent<Player>().dead == false){
					Debug.Log("You win!!!");
					gameOver = true;
					if(playa.name == "player1"){
						turnLabel.GetComponent<Text>().text = "Red Wins!";
						turnLabel.GetComponent<Text>().color = darkRed;
						turnTimer.GetComponent<Image>().color = darkRed;
						
					}else if(playa.name == "player2"){
						turnLabel.GetComponent<Text>().text = "Blue Wins!";
						turnLabel.GetComponent<Text>().color = darkBlue;
						turnTimer.GetComponent<Image>().color = darkBlue;
						
					}else if(playa.name == "player3"){
						turnLabel.GetComponent<Text>().text = "Green Wins!";
						turnLabel.GetComponent<Text>().color = darkGreen;
						turnTimer.GetComponent<Image>().color = darkGreen;
						
						
					}else if(playa.name == "player4"){
						turnLabel.GetComponent<Text>().text = "Yellow Wins!";
						turnLabel.GetComponent<Text>().color = darkYellow;
						turnTimer.GetComponent<Image>().color = darkYellow;
						
						
					}
					foreach(GameObject play in GameObject.FindGameObjectsWithTag("Player")){
						play.GetComponent<Player>().StartCoroutine("StartNewGame");
					}
				}
			}
		}
	}
	
	IEnumerator StartNewGame(){
		yield return new WaitForSeconds(0.5f);
		turnLabel.GetComponent<Text>().text = "New Game!";
		yield return new WaitForSeconds(1.0f);
		anim.SetBool("Dead", false);
		anim.SetFloat("Speed", 0.0f);
		dead = false;
		gameOver = false;
		secondsLeft = 0.0f;
		playerHealth = 5.0f;
		GetComponent<PhotonView>().RPC("UpdateHealth", PhotonTargets.All, playerHealth);
		myTurn = false;
		playerExp = 0.0f;
		if (GetComponent<PhotonView> ().isMine) { //This is for the DM and its Minions. This will always be the player
			if(transform.name == "player1"){
				secondsLeft = 5.0f;
				gameController.GetComponent<GameController>().CheckTurn();
			}
		}
		if(transform.name == "player1"){
			transform.position = new Vector3(0, 0.5f, 0);
		}else if(transform.name == "player2"){
			transform.position = new Vector3(0, 0.5f, 1);
		}else if(transform.name == "player3"){
			transform.position = new Vector3(1, 0.5f, 0);
		}else if(transform.name == "player4"){
			transform.position = new Vector3(1, 0.5f, 1);
		}
	}
		
	//Called when the player isn't the Dungeon Master
	void SetIsntDM(){
	
//		DM = false;
//		GameObject.Find ("DMController").SetActive(false);
//		GameObject.Find ("OpenDMPanelButton").SetActive(false);
		
	}

}
