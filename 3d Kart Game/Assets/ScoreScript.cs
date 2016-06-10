using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	float gameScore = 0f;
	public UnityEngine.UI.Text scoreText; 
	public PlayerScript player;
	GameObject[] tokens;
	// Use this for initialization
	void Start () {
		tokens = GameObject.FindGameObjectsWithTag ("Token");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void updateScore(){
		scoreText.text = "" + gameScore;
	}

	public void AddScore(float amount){
		gameScore = gameScore + amount;
		updateScore ();
		player.changekart (gameScore);
	}

	public void resetScore(){
		gameScore = 0f;
		updateScore ();
	}

	public void resetTokens(){
		foreach (GameObject toke in tokens) {
			toke.SetActive(true);
		}
	}
}
