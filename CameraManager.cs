using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	public bool connectionOpen = false;
	public Vector3 connectingOffset;
	public float screenWorldWidth;
	public float screenWorldHeight;
	public GameObject charBuilder;
	private float FOV;
	
	// Use this for initialization
	void Start () {		
	//Perspective Camera w/ FOV at 60
	
		FOV = 100.0f;
		#if UNITY_IOS
//		FOV = 300.0f;
		FOV = 250.0f;
		#endif
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Screen.height /FOV, Camera.main.transform.position.z);
		Debug.Log("Magnetic heading: " + Input.compass.magneticHeading);
		Vector3 viewPointPos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, Camera.main.transform.position.y));
		Debug.Log("FOV" + FOV);
		screenWorldHeight = viewPointPos.z * 2;
		screenWorldWidth = -viewPointPos.x * 2; 
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		//Ortho camera
		Camera.main.orthographicSize = Screen.height / FOV;
		screenWorldHeight = 2f * Camera.main.orthographicSize;
		screenWorldWidth = screenWorldHeight * Camera.main.aspect;
	}
	
	void OnGUI()
	{

		if(GUI.Button(new Rect(0, 0, Screen.width, Screen.height/4), "Top"))
		{
			ButtonPressed("Top");
		}
		
		if(GUI.Button(new Rect(0, Screen.height * 3.0f/4.0f, Screen.width, Screen.height/4), "Bottom"))
		{
			ButtonPressed("Bottom");
		}
		
		if(GUI.Button(new Rect(0, 0, Screen.width /4, Screen.height), "Left"))
		{
			ButtonPressed("Left");
		}
		
		if(GUI.Button(new Rect(Screen.width * 3.0f/4.0f, 0, Screen.width/4, Screen.height), "Right"))
		{
			ButtonPressed("Right");
		}
		
		if(GUI.Button (new Rect(Screen.width/2, Screen.height/2, Screen.width/6.0f, Screen.height/6.0f), "Done")){
//			GetComponent<PhotonView>().RPC ("FinishedCamSetup", PhotonTargets.AllBuffered);
			FinishedCamSetup();
			GetComponent<PhotonView>().RPC ("StartGame", PhotonTargets.All);
		}
		
	}
	
	void ButtonPressed(string buttonDirection){
		if(connectionOpen == false){
			if(buttonDirection == "Top"){
				GetComponent<PhotonView>().RPC("OpenConnection", PhotonTargets.AllBuffered, 
				new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + screenWorldHeight/2));
			}else if(buttonDirection == "Bottom"){
				GetComponent<PhotonView>().RPC("OpenConnection", PhotonTargets.AllBuffered, 
				new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - screenWorldHeight/2));
			}else if(buttonDirection == "Left"){
				GetComponent<PhotonView>().RPC("OpenConnection", PhotonTargets.AllBuffered, 
				new Vector3(Camera.main.transform.position.x - screenWorldWidth/2, Camera.main.transform.position.y, Camera.main.transform.position.z));
			}else if(buttonDirection == "Right"){
				GetComponent<PhotonView>().RPC("OpenConnection", PhotonTargets.AllBuffered, 
				new Vector3(Camera.main.transform.position.x + screenWorldWidth/2, Camera.main.transform.position.y, Camera.main.transform.position.z));	
				
			}
		}else if(connectionOpen == true){
			if(buttonDirection == "Top"){
				ConnectionMade(new Vector3(0, 0, -screenWorldHeight/2));
			}else if(buttonDirection == "Bottom"){
				ConnectionMade(new Vector3(0, 0, screenWorldHeight/2));	
			}else if(buttonDirection == "Left"){
				ConnectionMade(new Vector3(screenWorldWidth/2, 0, 0));	
			}else if(buttonDirection == "Right"){
				ConnectionMade(new Vector3(-screenWorldWidth/2, 0, 0));	
				
			}
		}
	
	}
	
	[PunRPC]
	void OpenConnection(Vector3 cOffset){
		connectionOpen = true;
		connectingOffset = cOffset;
	}
	
	void ConnectionMade(Vector3 offsetPosition){
		GameObject.Find ("Main Camera").transform.position = new Vector3(connectingOffset.x + offsetPosition.x, Camera.main.transform.position.y, connectingOffset.z + offsetPosition.z);
		GetComponent<PhotonView>().RPC("CloseConnection", PhotonTargets.AllBuffered);
	}
	
	[PunRPC]
	void CloseConnection(){
		connectionOpen = false;
	}
	
	[PunRPC]
	void StartGame(){
	}
	
	[PunRPC]
	void FinishedCamSetup(){
		GameObject player1 = (GameObject) PhotonNetwork.InstantiateSceneObject("Player1", new Vector3(0, 0.5f, 0), Quaternion.identity, 0, null);
		player1.transform.name = "player1";
		player1.GetComponent<Player>().playerMaxHealth = 5;
		player1.GetComponent<Player>().playerHealth = 5;
		player1.GetComponent<Player>().playerAttack = 1;
		player1.GetComponent<Player>().playerDefense = 1;
		player1.GetComponent<Player>().playerIntelligence = 1;
		player1.GetComponent<Player>().characterName = "player1";
		
		GameObject player2 = (GameObject) PhotonNetwork.InstantiateSceneObject("Player2", new Vector3(0, 0.5f, 1), Quaternion.identity, 0, null);
		player2.transform.name = "player2";
		player2.GetComponent<Player>().playerMaxHealth = 5;
		player2.GetComponent<Player>().playerHealth = 5;
		player2.GetComponent<Player>().playerAttack = 1;
		player2.GetComponent<Player>().playerDefense = 1;
		player2.GetComponent<Player>().playerIntelligence = 1;
		player2.GetComponent<Player>().characterName = "player2";
		
		GameObject player3 = (GameObject) PhotonNetwork.InstantiateSceneObject("Player3", new Vector3(1, 0.5f, 0), Quaternion.identity, 0, null);
		player3.transform.name = "player3";
		player3.GetComponent<Player>().playerMaxHealth = 5;
		player3.GetComponent<Player>().playerHealth = 5;
		player3.GetComponent<Player>().playerAttack = 1;
		player3.GetComponent<Player>().playerDefense = 1;
		player3.GetComponent<Player>().playerIntelligence = 1;
		player3.GetComponent<Player>().characterName = "player3";
		
		GameObject player4 = (GameObject) PhotonNetwork.InstantiateSceneObject("Player4", new Vector3(1, 0.5f, 1), Quaternion.identity, 0, null);
		player4.transform.name = "player4";
		player4.GetComponent<Player>().playerMaxHealth = 5;
		player4.GetComponent<Player>().playerHealth = 5;
		player4.GetComponent<Player>().playerAttack = 1;
		player4.GetComponent<Player>().playerDefense = 1;
		player4.GetComponent<Player>().playerIntelligence = 1;
		player4.GetComponent<Player>().characterName = "player4";
		
		
	}
}
