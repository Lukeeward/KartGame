using UnityEngine.Advertisements;
using UnityEngine;

public class AdsScript : MonoBehaviour
{
	private MenuScript currentMenu;
	public void ShowRewardedAd(MenuScript menu)
	{
		currentMenu = menu;
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			currentMenu.closeUIAndContinue ();
			break;
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		}
	}
}