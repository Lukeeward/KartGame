using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public PlayerScript player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hideMenu(){
		CanvasGroup menuCanvasGroup = GetComponent<CanvasGroup> ();
		menuCanvasGroup.alpha = 0.0f;
		menuCanvasGroup.blocksRaycasts = false;
		player.startGame ();
	}

	public void showMenu(){
		CanvasGroup menuCanvasGroup = GetComponent<CanvasGroup> ();
		menuCanvasGroup.alpha = 1f;
		menuCanvasGroup.blocksRaycasts = true;
	}

	public void ChangeToEndless(){
		Application.LoadLevel ("3dkartgameProcGen");
	}

	public void changeToLevel(){
		Application.LoadLevel ("3dkartgame");
	}
}
