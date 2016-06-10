using UnityEngine;
using System.Collections;

public class DeathZon : MonoBehaviour {

	public PlayerScript player;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter (Collider col){
		if (col.gameObject.name == "Player") {
			player.setDeath (true);
		}
	}
}
