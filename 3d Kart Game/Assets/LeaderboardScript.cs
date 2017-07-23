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
				AGSLeaderboardsClient.SubmitScore("leaderboard", Convert.ToInt64(newScore));
			} catch (Exception) {
				GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, "Update score failed");
			}
		}
	}


	public void checkForAchievement(float newScore)
	{
		if (AGSClient.IsServiceReady()) {
			if (newScore >= 50) {
				AGSAchievementsClient.UpdateAchievementProgress ("50", 100);
			}
			if (newScore >= 150) {
				AGSAchievementsClient.UpdateAchievementProgress ("150", 100);
			}
			if (newScore >= 300) {
				AGSAchievementsClient.UpdateAchievementProgress ("300", 100);
			}
			if (newScore >= 500) {
				AGSAchievementsClient.UpdateAchievementProgress ("500", 100);
			}
			if (newScore >= 1000) {
				AGSAchievementsClient.UpdateAchievementProgress ("1000", 100);
			}
		}
	}

}