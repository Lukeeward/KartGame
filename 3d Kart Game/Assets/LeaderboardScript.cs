using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;
using GameAnalyticsSDK;

public class LeaderboardScript : MonoBehaviour {
	public string leaderboard;

	void Start ()
	{
		PlayGamesPlatform.DebugLogEnabled = true;
	}

	/// <summary>
	/// Login In Into Your Google+ Account
	/// </summary>
	public void LogIn (bool ShowLeaderboard = false)
	{
		try{
		Social.localUser.Authenticate ((bool success) =>
			{
				if (success) {
					if(ShowLeaderboard)
					{
						OnShowLeaderBoard();
					}
				} else {
				}
			});
		} catch(Exception ex) {
			GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, ex.Message);
		}
	}

	/// <summary>
	/// Shows All Available Leaderborad
	/// </summary>
	public void OnShowLeaderBoard ()
	{
		if (PlayGamesPlatform.Instance.IsAuthenticated()) {
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI (leaderboard);
		} else {
			LogIn (true);
		}
	}

	/// <summary>
	/// Adds Score To leader board
	/// </summary>
	public void OnAddScoreToLeaderBoard (float newScore)
	{
		if (PlayGamesPlatform.Instance.IsAuthenticated()) {
			Social.ReportScore ((int)newScore, leaderboard, (bool success) =>
				{
					if (success) {
						Debug.Log ("Update Score Success");
					} else {
						Debug.Log ("Update Score Fail");
						GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, "Update score failed");
					}
				});
		}
	}


	public void checkForAchievement(float newScore)
	{
		if (PlayGamesPlatform.Instance.IsAuthenticated ()) {
			if (newScore >= 50) {
				Social.ReportProgress ("CgkI7-2Us6EDEAIQAg", 100.0f, (bool success) => {
					Debug.Log(success);
				});
			}
			if (newScore >= 150) {
				Social.ReportProgress ("CgkI7-2Us6EDEAIQAw", 100.0f, (bool success) => {
					Debug.Log(success);
				});
			}
			if (newScore >= 300) {
				Social.ReportProgress ("CgkI7-2Us6EDEAIQBA", 100.0f, (bool success) => {
					Debug.Log(success);
				});
			}
			if (newScore >= 500) {
				Social.ReportProgress ("CgkI7-2Us6EDEAIQBQ", 100.0f, (bool success) => {
					Debug.Log(success);
				});
			}
			if (newScore >= 1000) {
				Social.ReportProgress ("CgkI7-2Us6EDEAIQBg", 100.0f, (bool success) => {
					Debug.Log(success);
				});
			}
		}
	}

}