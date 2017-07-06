using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class HighscoreScript : MonoBehaviour {

	public LeaderboardScript leaderboard;

	float localHighscore = 0f;
	// Use this for initialization
	void Start () {
		localHighscore = ZPlayerPrefs.GetFloat("highscore");
	}

	public bool checkHighscore(float score){
		if(score > localHighscore){
			return true;
		}
		return false;
	}

	public void updateHighscore(float newHighscore){
		if(checkHighscore(newHighscore)){
			ZPlayerPrefs.SetFloat("highscore", newHighscore);
			localHighscore = newHighscore;
			GameAnalytics.NewDesignEvent ("High score", newHighscore);
			leaderboard.OnAddScoreToLeaderBoard (newHighscore);
		}
		leaderboard.checkForAchievement (newHighscore);
	}

	public float getHighscore(){
		float highscore = ZPlayerPrefs.GetFloat("highscore");
		return highscore;
	}

}
