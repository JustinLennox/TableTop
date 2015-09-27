using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour {

	float lastTouch;
	//flag to check if the user has tapped / clicked.
	//Set to true on click. Reset to false on reaching destination
	private bool flag = false;
	//destination point
	private Vector3 endPoint;
	//alter this to change the speed of the movement of player / gameobject
	public float duration = 50.0f;
	//vertical position of the gameobject
	private float yAxis;
	
	GameObject gameController;
	// Use this for initialization
	void Start () {
	
		yAxis = 0.5f;
		gameController = GameObject.Find ("GameController");
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//check if the screen is touched / clicked
		if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0)))
		{
			if(Time.time - lastTouch < 0.2f){
				Debug.Log("double tap");
				//declare a variable of RaycastHit struct
				RaycastHit hit;
				//Create a Ray on the tapped / clicked position
				Ray ray;
				//for unity editor
				#if UNITY_EDITOR
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				//for touch device
				#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
				ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				#endif
				
				//Check if the ray hits any collider
				if(Physics.Raycast(ray,out hit))
				{
					//set a flag to indicate to move the gameobject
					flag = true;
					//save the click / tap position
					endPoint = hit.point;
					//as we do not want to change the y axis value based on touch position, reset it to original y axis value
					endPoint.y = yAxis;
					Debug.Log(endPoint);
					gameController.GetComponent<GameController>().currentPlayer.GetComponent<PhotonView>().RPC ("MoveTo", PhotonTargets.All, endPoint);
					
				}
				
			}  
			lastTouch = Time.time;
		}
	}
}
