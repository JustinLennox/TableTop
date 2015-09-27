using UnityEngine;
using System.Collections;

public class Lobby : Photon.PunBehaviour {
	
	public GameObject joinButton;
	public GameObject hostButton;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}
	
	void OnJoinedRoom(){
			PhotonNetwork.LoadLevel ("GameScene");			
	}


	public void HostGame(){
		hostButton.SetActive(false);
		joinButton.SetActive(false);
		Debug.Log ("Host");
		PlayerPrefs.SetInt("Host", 1);
		PhotonNetwork.ConnectUsingSettings ("0.1");

	}

	public void JoinGame(){
		hostButton.SetActive(false);
		joinButton.SetActive(false);
		Debug.Log ("Join");
		PlayerPrefs.SetInt("Host", 0);
		PhotonNetwork.ConnectUsingSettings ("0.1");

	}
}
