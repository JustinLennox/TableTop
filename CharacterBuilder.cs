using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterBuilder : MonoBehaviour {

	private int totalPoints = 14;
	private int currentPoints =  14;
	private int health = 1;
	private int attack = 1;
	private int defense = 1;
	private int intelligence = 1;
	private int speed = 0;
	private int luck = 0;
	private int donePress = 0;
	public string characterName;
	public GameObject gameCanvas;
	
	// Use this for initialization
	void Start () {

		GameObject.Find ("HealthPoints").gameObject.GetComponent<Text> ().text = "" + health;
		GameObject.Find ("AttackPoints").gameObject.GetComponent<Text> ().text = "" + attack;
		GameObject.Find ("DefensePoints").gameObject.GetComponent<Text> ().text = "" + defense;
		GameObject.Find ("IntelligencePoints").gameObject.GetComponent<Text> ().text = "" + intelligence;

		
	}
	
	// Update is called once per frame
	void Update () {
		updateCurrentPoints ();
	}
	
	public void healthUp(){
		if (currentPoints > 0) {
			health++;
			currentPoints--;
			GameObject.Find ("HealthPoints").gameObject.GetComponent<Text> ().text = "" + health;
		}
	}

	public void healthDown(){
		if (health > 1) {
			health--;
			currentPoints++;
			GameObject.Find ("HealthPoints").gameObject.GetComponent<Text> ().text = "" + health;
		}
	}

	public void attackUp(){
		if (currentPoints > 0) {
			attack++;
			currentPoints--;
			GameObject.Find ("AttackPoints").gameObject.GetComponent<Text> ().text = "" + attack;
		}
	}

	public void attackDown(){
		if (attack > 1) {
			attack--;
			currentPoints++;
			GameObject.Find ("AttackPoints").gameObject.GetComponent<Text> ().text = "" + attack;
		}
	}

	public void defenseUp(){
		if (currentPoints > 0) {
			defense++;
			currentPoints--;
			GameObject.Find ("DefensePoints").gameObject.GetComponent<Text> ().text = "" + defense;
		}
	}
	
	public void defenseDown(){
		if (defense > 1) {
			defense--;
			currentPoints++;
			GameObject.Find ("DefensePoints").gameObject.GetComponent<Text> ().text = "" + defense;
		}
	}

	public void intelligenceUp(){
		if (currentPoints > 0) {
			intelligence++;
			currentPoints--;
			GameObject.Find ("IntelligencePoints").gameObject.GetComponent<Text> ().text = "" + intelligence;
		}
	}
	
	public void intelligenceDown(){
		if (intelligence > 1) {
			intelligence--;
			currentPoints++;
			GameObject.Find ("IntelligencePoints").gameObject.GetComponent<Text> ().text = "" + intelligence;
		}
	}

	public void updateCurrentPoints(){
		GameObject.Find ("PointsFill").gameObject.GetComponent<Image> ().fillAmount = (float)currentPoints / (float)totalPoints;
		GameObject.Find ("CurrentPoints").gameObject.GetComponent<Text> ().text = "" + currentPoints;

	}

	public void doneButtonPressed(){
		donePress++;
		if (donePress >= 3) {
			GameObject player = (GameObject) PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity, 0);
			player.GetComponent<Player>().playerMaxHealth = health;
			player.GetComponent<Player>().playerHealth = health;
			player.GetComponent<Player>().playerAttack = attack;
			player.GetComponent<Player>().playerDefense = defense;
			player.GetComponent<Player>().playerIntelligence = intelligence;
			player.GetComponent<Player>().characterName = GameObject.Find ("CharacterName").GetComponent<InputField>().text;
			transform.gameObject.SetActive (false);
			gameCanvas.SetActive(true);
		}
	}

}
