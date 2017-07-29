using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using GameAnalyticsSDK;

public class LeaderboardScript : MonoBehaviour {
	public string leaderboard;

	void Start ()
	{
	}

	/// <summary>
	/// Shows All Available Leaderborad
	/// </summary>
	public void OnShowLeaderBoard ()
	{
		if (AGSClient.IsServiceReady()) {
			AGSLeaderboardsClient.ShowLeaderboardsOverlay();
		}
	}

	/// <summary>
	/// Adds Score To leader board
	/// </summary>
	public void OnAddScoreToLeaderBoard (float newScore)
	{
		if (AGSClient.IsServiceReady()) {
			try {
				AGSLeaderboardsClient.SubmitScoreFailedEvent += onLeaderboardFailure;
				AGSLeaderboardsClient.SubmitScore("Highscores", Convert.ToInt64(newScore));
			} catch (Exception) {
				GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, "Update score failed");
			}
		}
	}

	private void onLeaderboardFailure(string leaderboardid, string error)
	{
		GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, "Update leaderboard for " + leaderboardid + " : " + error);
	}

	public void checkForAchievement(float newScore)
	{
		if (AGSClient.IsServiceReady()) {
			if (newScore >= 50) {
				AGSAchievementsClient.UpdateAchievementProgress ("50m", 100);
			}
			if (newScore >= 150) {
				AGSAchievementsClient.UpdateAchievementProgress ("150m", 100);
			}
			if (newScore >= 300) {
				AGSAchievementsClient.UpdateAchievementProgress ("300m", 100);
			}
			if (newScore >= 500) {
				AGSAchievementsClient.UpdateAchievementProgress ("500m", 100);
			}
			if (newScore >= 1000) {
				AGSAchievementsClient.UpdateAchievementProgress ("1000m", 100);
			}
		}
	}

}