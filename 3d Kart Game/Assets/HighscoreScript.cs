using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class HighscoreScript : MonoBehaviour {

	EncryptionScript encryptionScript = new EncryptionScript();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool checkHighscore(float score){
		float oldHighscore = encryptionScript.getPlayerPref("highscore");
		if(score > oldHighscore){
			return true;
		}
		return false;
	}

	public void updateHighscore(float newHighscore){
		if(checkHighscore(newHighscore)){
			encryptionScript.savePlayerPref("highscore", newHighscore);
			GameAnalytics.NewDesignEvent ("High score", newHighscore);
		}
	}

	public float getHighscore(){
		return (encryptionScript.getPlayerPref("highscore"));
	}

}
