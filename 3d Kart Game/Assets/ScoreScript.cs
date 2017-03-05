using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	float goldScore = 0f;
	float distanceScore = 0f;
	public UnityEngine.UI.Text scoreText; 
	public UnityEngine.UI.Text distanceText;
	public UnityEngine.UI.Text finalScoreText; 
	public UnityEngine.UI.Text finalDistanceText;
	public UnityEngine.UI.Text finalHighscore;
	public PlayerScript player;
	public MenuScript menu;
	public HighscoreScript highscoreScript;
	private float lastUpdatedPosition = 0f;
	GameObject[] tokens;
	// Use this for initialization
	void Start () {
		tokens = GameObject.FindGameObjectsWithTag ("Token");
	}

	public void updateScore(){
		scoreText.text = "" + goldScore;
	}

	public void updateDistance(){
		distanceText.text = "" + distanceScore + "m";
		checkIfNewHighscore ();
	}

	public void AddScore(float amount){
		goldScore = goldScore + amount;
		updateScore ();
		player.changekart (goldScore);
	}

	public void checkIfNewHighscore() {
		if (highscoreScript.checkHighscore (distanceScore)) {
			menu.displayNewHighscoreText ();
		}
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

	public float getPersistantGoldScore() {
		return (ZPlayerPrefs.GetFloat("goldscore"));
	}

	public void savePersistantGoldScore(float newScore) {
		ZPlayerPrefs.SetFloat("goldscore", newScore);
	}

	public float getOverallGoldScore() {
		return (getPersistantGoldScore() + goldScore);
	}

	public float getGoldScore(){
		return goldScore;
	}

	public void buySave(){
		if (canAffordSave ()) {
			var remainingGold = getOverallGoldScore() - 25;
			goldScore = 0;
			savePersistantGoldScore (remainingGold);
			updateScore ();
		}
	}

	public bool canAffordSave(){
		if (getOverallGoldScore() >= 25) {
			return true;
		} else {
			return false;
		}
	}

	public void resetScore(){
		goldScore = 0f;
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
		finalScoreText.text = getOverallGoldScore().ToString();
		finalHighscore.text = highscore.ToString () + "m";
		savePersistantGoldScore (getOverallGoldScore ());
	}
}
