using UnityEngine;
using System.Collections;

public class DeathZon : MonoBehaviour {

	public PlayerScript player;
	private audioScript audioscript;
	// Use this for initialization
	void Start () {
		audioscript = GameObject.Find ("Player").GetComponent<audioScript> ();
	}

	void OnTriggerEnter (Collider col){
		if (col.gameObject.name == "Player") {
			audioscript.playDeath ();
			player.setDeath (true);
		}
	}
}
