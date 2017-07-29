using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameAnalyticsSDK;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

public class MenuScript : MonoBehaviour {

	public PlayerScript player;
	public CanvasGroup gameUI;
	public CanvasGroup deathUI;
	public CanvasGroup pauseButtonCanvasGroup;
	public CanvasGroup startButtonCanvasGroup;
	public CanvasGroup pausedVolumeButtonCanvasGroup;
	public CanvasGroup saveMePopupCanvas;
	public CanvasGroup newHighscoreTextCanvasGroup;
	public ScoreScript goldscore;
	public Button muteButton;
	public Button pausedMuteButton;
	public Button saveMeButton;
	public Button freeSaveButton;
	public Sprite muteSprite;
	public InputField feedbackForm;
	public LeaderboardScript leaderboard;
	public Text remainingFreeSaveText;

	public CanvasGroup enjoyingGameCanvas;
	public CanvasGroup sendFeedbackCanvas;
	public CanvasGroup rateGameCanvas;
	public AdsScript ads;

	private int localIsMute = 0;
	public bool debug;
	private bool pauseToggle = false;
	private bool muteToggle = false;
	private int timesPlayed;
	private float timeLeft = 0;
	private bool saveMeIsVisible = false;

	void Start() {
		if(PlayerPrefs.HasKey("mute")) {
			localIsMute = PlayerPrefs.GetInt ("mute");
		}
		bool isServiceReady = AGSClient.IsServiceReady();
		if(!isServiceReady)
		{
			AGSClient.ServiceReadyEvent += serviceReadyHandler;
			AGSClient.ServiceNotReadyEvent += serviceNotReadyHandler;
			bool usesLeaderboards = true;
			bool usesAchievements = true;
			bool usesWhispersync = true;
			AGSClient.Init (usesLeaderboards, usesAchievements, usesWhispersync);
		}
		timeLeft = getRemainingFreeSaveTime ();
	}

	private void serviceNotReadyHandler (string error)    {
		Debug.Log("Service is not ready");
	}

	private void serviceReadyHandler ()    {
		Debug.Log("Service is ready");
	}

	void Update()
	{
		if (saveMeIsVisible) 
		{
			//Check that the menu is visible before doing this stuff
			timeLeft = timeLeft - Time.deltaTime;
			setLastUseCountDown (timeLeft);
		}
	}

	public void hideMenuAndStartGame(){
		CanvasGroup menuCanvasGroup = GetComponent<CanvasGroup> ();
		menuCanvasGroup.alpha = 0.0f;
		menuCanvasGroup.blocksRaycasts = false;
		showGameUI ();
		hideDeathMenu ();
		hideNewHighscoreText ();
		player.startGame ();
	}

	public void showMenu(){
		CanvasGroup menuCanvasGroup = GetComponent<CanvasGroup> ();
		menuCanvasGroup.alpha = 1f;
		menuCanvasGroup.blocksRaycasts = true;
		//Get vol preferences
		if(localIsMute == 1) {
			muteButton.image.overrideSprite = muteSprite;
			pausedMuteButton.image.overrideSprite = muteSprite;
		} else {
			muteButton.image.overrideSprite = null;
			pausedMuteButton.image.overrideSprite = null;
		}
		hideGameUI ();
		hideDeathMenu ();
		trackGamesPlayed ();

		hideNewHighscoreText ();
	}

	public void showDeathMenu(){
		if (deathUI.alpha != 1.0f) {
			CanvasGroup menuCanvasGroup = GetComponent<CanvasGroup> ();
			menuCanvasGroup.alpha = 0.0f;
			menuCanvasGroup.blocksRaycasts = false;
			hideGameUI ();
			if (debug || goldscore.canAffordSave ()) {
				saveMeButton.interactable = true;
			} else {
				saveMeButton.interactable = false;
			}
			deathUI.alpha = 1f;
			deathUI.blocksRaycasts = true;
		}
	}

	public void showGameUI(){
		gameUI.alpha = 1f;
		gameUI.blocksRaycasts = true;
		//Get vol preferences
		if(localIsMute == 1) {
			muteButton.image.overrideSprite = muteSprite;
			pausedMuteButton.image.overrideSprite = muteSprite;
		} else {
			muteButton.image.overrideSprite = null;
			pausedMuteButton.image.overrideSprite = null;
		}
	}

	public void hideGameUI(){
		gameUI.alpha = 0.0f;
		gameUI.blocksRaycasts = false;
	}

	public void hideDeathMenu(){
		deathUI.alpha = 0.0f;
		deathUI.blocksRaycasts = false;
	}


	public void skipToMenu(){
		showMenu ();
		player.respawnPlayer ();
	}
		
	public void ChangeToEndless(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("3dkartgameProcGen");
	}
		
	public void togglePause(){
		if (pauseToggle == false) {
			pauseGame ();
			pauseToggle = true;
			startButtonCanvasGroup.alpha = 1;
			startButtonCanvasGroup.blocksRaycasts = true;
			pausedVolumeButtonCanvasGroup.alpha = 1;
			pausedVolumeButtonCanvasGroup.blocksRaycasts = true;
			pauseButtonCanvasGroup.alpha = 0;
			pauseButtonCanvasGroup.blocksRaycasts = false;
		} else {
			resumeGame ();
			pauseToggle = false;
			startButtonCanvasGroup.alpha = 0;
			startButtonCanvasGroup.blocksRaycasts = false;
			pausedVolumeButtonCanvasGroup.alpha = 0;
			pausedVolumeButtonCanvasGroup.blocksRaycasts = false;
			pauseButtonCanvasGroup.alpha = 1;
			pauseButtonCanvasGroup.blocksRaycasts = true;
		}
	}

	public void saveMeButtonClick(){
		deathUI.alpha = 0f;
		deathUI.blocksRaycasts = false;
		hideDeathMenu ();
		saveMeIsVisible = true;
		saveMePopupCanvas.alpha = 1;
		saveMePopupCanvas.blocksRaycasts = true;
		saveMePopupCanvas.interactable = true;
		if (goldscore.canAffordSave ()) {
			saveMeButton.interactable = true;
		} else {
			saveMeButton.interactable = false;
		}
		if (canUseFreeSave ()) {
			freeSaveButton.interactable = true;
		} else {
			freeSaveButton.interactable = false;
			timeLeft = getRemainingFreeSaveTime ();
		}
	}

	private void hideSaveMePopup() {
		saveMePopupCanvas.alpha = 0;
		saveMePopupCanvas.blocksRaycasts = false;
		saveMePopupCanvas.interactable = false;
		saveMeIsVisible = false;
	}

	public void savePlayer() {
		deathUI.alpha = 0f;
		deathUI.blocksRaycasts = false;
		if (goldscore.canAffordSave()) {
			goldscore.buySave ();
			closeUIAndContinue ();
		} else {
			player.respawnPlayer ();
		}
	}

	public void closeUIAndContinue()
	{
		player.savePlayer ();
		hideSaveMePopup ();
		player.startGame ();
		showGameUI ();
	}

	public void freeSave()
	{
		if (canUseFreeSave ()) {
			ads.ShowRewardedAd (this);
			ZPlayerPrefs.SetString ("freeSysString", DateTime.Now.ToString ());
		}
	}

	private bool canUseFreeSave()
	{
		DateTime lastUse = DateTime.Now;
		if (ZPlayerPrefs.HasKey ("freeSysString")) {
			string lastUseString = ZPlayerPrefs.GetString ("freeSysString");
			lastUse = DateTime.Parse (lastUseString);
			var currentTime = DateTime.Now.Ticks;
			var currentTimess = DateTime.Now;
			var limitTime = lastUse.AddMinutes (15d).Ticks;
			if (limitTime < currentTime) {
				return true;
			} else {
				return false;
			}
		} else {
			return true;
		}
	}

	private void setLastUseCountDown(float timeRemaining)
	{
		if (timeRemaining > 0) {
			remainingFreeSaveText.text = "Next save in: " + (Mathf.Floor(timeRemaining / 60).ToString("00")  + ":" + Mathf.RoundToInt (timeRemaining % 60).ToString ("00"));
		} else {
			remainingFreeSaveText.text = "";
			if (canUseFreeSave ())
			{
				freeSaveButton.interactable = true;
			}
		}
	}

	private float getRemainingFreeSaveTime()
	{
		if (ZPlayerPrefs.HasKey ("freeSysString")) {
			DateTime lastUse = DateTime.Now;
			string lastUseString = ZPlayerPrefs.GetString ("freeSysString");
			lastUse = DateTime.Parse (lastUseString);
			var currentTime = DateTime.Now;
			if (lastUse.CompareTo (currentTime) <= 0) {
				var limitTime = lastUse.AddMinutes (15d);
				TimeSpan span = limitTime - currentTime;
				return (float)span.TotalSeconds;
			} else {
				return 0;
			}
		} else {
			return 0;
		}
	}

	public void pauseGame(){
		Time.timeScale = 0;
	}
	public void resumeGame(){
		Time.timeScale = 1;
	}

	public void closeSaveUI() {
		saveMePopupCanvas.alpha = 0;
		saveMePopupCanvas.blocksRaycasts = false;
		saveMeIsVisible = false;
		skipToMenu ();
	}

	public void toggleVolumeMute() {
		if (!muteToggle) {
			AudioListener.pause = true;
			muteToggle = true;
			muteButton.image.overrideSprite = muteSprite;
			pausedMuteButton.image.overrideSprite = muteSprite;
			PlayerPrefs.SetInt("mute", 1);
			localIsMute = 1;
		} else {
			muteToggle = false;
			AudioListener.pause = false;
			muteButton.image.overrideSprite = null;
			pausedMuteButton.image.overrideSprite = null;
			PlayerPrefs.SetInt("mute", 0);
			localIsMute = 0;
		}
	}

	private void trackGamesPlayed() {
		timesPlayed = PlayerPrefs.HasKey ("gamesPlayed") ? PlayerPrefs.GetInt ("gamesPlayed") : 0;
		timesPlayed = timesPlayed + 1;
		int hasAskedForRating = PlayerPrefs.HasKey("askedForRating") ? PlayerPrefs.GetInt ("askedForRating") : 0;
		if (timesPlayed >= 3 && hasAskedForRating == 0) {
			askForRating();
		} else {
			PlayerPrefs.SetInt ("gamesPlayed",timesPlayed);
			if(timesPlayed > 10 && timesPlayed < 30) {
				GameAnalytics.NewDesignEvent ("TimesPlayed > 10", timesPlayed);
			}
			if(timesPlayed > 30 && timesPlayed < 60) {
				GameAnalytics.NewDesignEvent ("TimesPlayed > 30", timesPlayed);
			}
		}
	}

	private void askForRating() {
		hideGameUI ();
		hideDeathMenu ();
		CanvasGroup menuCanvasGroup = GetComponent<CanvasGroup> ();
		menuCanvasGroup.alpha = 0f;
		menuCanvasGroup.blocksRaycasts = false;
		enjoyingGameCanvas.alpha = 1;
		enjoyingGameCanvas.blocksRaycasts = true;
	}

	public void displayFeedbackPanel() {
		sendFeedbackCanvas.alpha = 1;
		sendFeedbackCanvas.blocksRaycasts = true;
		enjoyingGameCanvas.alpha = 0;
		enjoyingGameCanvas.blocksRaycasts = false;
		PlayerPrefs.SetInt ("askedForRating", 1);
	}

	public void displayRatePanel() {
		sendFeedbackCanvas.alpha = 0;
		sendFeedbackCanvas.blocksRaycasts = false;
		enjoyingGameCanvas.alpha = 0;
		enjoyingGameCanvas.blocksRaycasts = false;
		rateGameCanvas.alpha = 1;
		rateGameCanvas.blocksRaycasts = true;
		PlayerPrefs.SetInt ("askedForRating", 1);
	}

	public void redirectToPlaystore() {
		hideAllRatePopups ();
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=" + "com.InnovationWard.CavernRider");
		#endif
	}

	public void sendFeedback() {
		var feedbackText = feedbackForm.text;
		if(feedbackText != "") {
			FeedbackScript.sendEmail (feedbackText);
			GameAnalytics.NewErrorEvent (GAErrorSeverity.Info,feedbackText);
		}
		hideAllRatePopups ();
	}

	public void hideAllRatePopups() {
		sendFeedbackCanvas.alpha = 0;
		sendFeedbackCanvas.blocksRaycasts = false;
		enjoyingGameCanvas.alpha = 0;
		enjoyingGameCanvas.blocksRaycasts = false;
		rateGameCanvas.alpha = 0;
		rateGameCanvas.blocksRaycasts = false;
		showMenu ();
	}

	public void displayNewHighscoreText() {
		newHighscoreTextCanvasGroup.alpha = 1;
	}

	public bool newHighScoreTextIsVisible()
	{
		return newHighscoreTextCanvasGroup.alpha == 1;
	}

	public void hideNewHighscoreText() {
		newHighscoreTextCanvasGroup.alpha = 0;
	}

	public void showHighscores()
	{
		leaderboard.OnShowLeaderBoard ();
	}

}
