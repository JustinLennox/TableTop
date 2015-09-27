using UnityEngine;
using System.Collections;

public class Moveable : MonoBehaviour {

	public string xPosition;
	public string zPosition;
	public int movesLeft;
	public Vector3 destination;

	// Use this for initialization
	void Awake () {
		destination = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(transform.position.x != destination.x || transform.position.z != destination.z){
			transform.position = Vector3.MoveTowards(transform.position, destination, 2.0f * Time.deltaTime);
		}
		
		if(GetComponent<Player>().myTurn){
			foreach(Touch t in Input.touches){
				var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
				var offset = new Vector2(t.position.x - screenPoint.x, t.position.y - screenPoint.y);
				var angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0, angle, 0);
			}
		}
		
	}
	
	//Moves the object to a new position over the network and decreases the moves he has left
	[PunRPC]
	public void MoveTo(Vector3 newPosition){
//		Debug.Log("move to");
//		Debug.Log("old" + xPosition + zPosition + "new" + newTileName);
//		GameObject oldTile = GameObject.Find (xPosition + zPosition);	//Empty the old tile
//		oldTile.GetComponent<TileScript>().isFilled = false;
//		GameObject tile = GameObject.Find(""+ newX + newZ);		
//		tile.GetComponent<TileScript>().isFilled = true; //Fill the new tile
		
		destination = new Vector3(newPosition.x, 0.5f, newPosition.z);	//Move the object
		Debug.Log("Destination: " + destination);
		movesLeft--;
		
		xPosition = "" + xPosition;	//Set the new object position strings
		zPosition = "" + zPosition;
	}
}
