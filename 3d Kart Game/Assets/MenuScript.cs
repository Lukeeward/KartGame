using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameAnalyticsSDK;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

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
	public Sprite muteSprite;
	public InputField feedbackForm;
	public LeaderboardScript leaderboard;

	public CanvasGroup enjoyingGameCanvas;
	public CanvasGroup sendFeedbackCanvas;
	public CanvasGroup rateGameCanvas;


	public UnityEngine.UI.Button saveMeButton;
	private int localIsMute = 0;
	public bool debug;
	private bool pauseToggle = false;
	private bool muteToggle = false;
	private int timesPlayed;

	void Start() {
		if(PlayerPrefs.HasKey("mute")) {
			localIsMute = PlayerPrefs.GetInt ("mute");
		}
		PlayGamesPlatform.Activate ();
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

	public void changeToLevel(){
		//UnityEngine.SceneManagement.SceneManager.LoadScene("3dkartgame");
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

		saveMePopupCanvas.alpha = 1;
		saveMePopupCanvas.blocksRaycasts = true;
		saveMePopupCanvas.interactable = true;
		if (goldscore.canAffordSave ()) {
			saveMeButton.interactable = true;
		} else {
			saveMeButton.interactable = false;
		}
	}

	private void hideSaveMePopup() {
		saveMePopupCanvas.alpha = 0;
		saveMePopupCanvas.blocksRaycasts = false;
		saveMePopupCanvas.interactable = false;
	}

	public void savePlayer() {
		deathUI.alpha = 0f;
		deathUI.blocksRaycasts = false;
		if (goldscore.canAffordSave()) {
			goldscore.buySave ();
			player.savePlayer ();
			hideSaveMePopup ();
			player.startGame ();
			showGameUI ();
		} else {
			player.respawnPlayer ();
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
		Application.OpenURL("market://details?id=" + "APPID");
		#endif
	}

	public void sendFeedback() {
		if(feedbackForm.text != "") {
			GameAnalytics.NewErrorEvent (GAErrorSeverity.Info,feedbackForm.text);
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

	public void hideNewHighscoreText() {
		newHighscoreTextCanvasGroup.alpha = 0;
	}

	public void showHighscores()
	{
		leaderboard.OnShowLeaderBoard ();
	}

}
