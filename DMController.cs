using UnityEngine;
using System.Collections;

public class DMController : MonoBehaviour {
	public GameObject gameController;
	public GameObject DMPanel;
	
	// Use this for initialization
	void Start () {
		CloseDMPanel();		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Called when the DM presses the Enemy 1 Button in the DM Panel
	public void SpawnEnemyOne(){
		CloseDMPanel();
		PhotonNetwork.Instantiate("Enemy1", new Vector3(0, 1, 0), Quaternion.identity, 0);
	}
	
	//Opens the DMPanel
	public void OpenDMPanel(){
		DMPanel.SetActive(true);
	}
	
	//Close the DMPanel
	public void CloseDMPanel(){
		DMPanel.SetActive(false);		
		
	}
	
}
