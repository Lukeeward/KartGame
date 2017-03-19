using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

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
			Debug.Log (ex);
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
					}
				});
		}
	}
}