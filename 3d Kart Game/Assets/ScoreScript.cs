using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	float gameScore = 0f;
	float distanceScore = 0f;
	public UnityEngine.UI.Text scoreText; 
	public UnityEngine.UI.Text distanceText;
	public UnityEngine.UI.Text finalScoreText; 
	public UnityEngine.UI.Text finalDistanceText;
	public UnityEngine.UI.Text finalHighscore;
	public PlayerScript player;
	public MenuScript menu;
	private float lastUpdatedPosition = 0f;
	GameObject[] tokens;
	// Use this for initialization
	void Start () {
		tokens = GameObject.FindGameObjectsWithTag ("Token");
	}

	public void updateScore(){
		scoreText.text = "" + gameScore;
	}

	public void updateDistance(){
		distanceText.text = "" + distanceScore + "m";
	}

	public void AddScore(float amount){
		gameScore = gameScore + amount;
		updateScore ();
		player.changekart (gameScore);
	}

	public void addDistanceScore(float amount){
		distanceScore = amount;
		updateDistance ();
		if((distanceScore != 0)&&(distanceScore % 20 == 0)) {
			if(lastUpdatedPosition != distanceScore) {
				player.increaseSpeed ();
				lastUpdatedPosition = distanceScore;
			}
		}
	}

	public float getDistanceScore(){
		return distanceScore;
	}

	public float getGoldScore(){
		return gameScore;
	}

	public void buySave(){
		if (canAffordSave ()) {
			gameScore = gameScore - 25;
			updateScore ();
		}
	}

	public bool canAffordSave(){
		if (gameScore >= 25) {
			return true;
		} else {
			return false;
		}
	}

	public void resetScore(){
		gameScore = 0f;
		distanceScore = 0f;
		updateScore ();
		player.resetSpeed ();
		lastUpdatedPosition = 0;
		if (distanceScore != 0) {
			updateDistance ();
		}
	}

	public void resetTokens(){
		foreach (GameObject toke in tokens) {
			toke.SetActive(true);
		}
	}

	public void showFinalScore(float highscore){
		finalDistanceText.text = distanceScore.ToString() + "m";
		finalScoreText.text = gameScore.ToString();
		finalHighscore.text = highscore.ToString () + "m";
	}
}
