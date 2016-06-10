using UnityEngine;
using System.Collections;

public class DeathScript : MonoBehaviour {

	public PlatformController platformScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.gameObject.name == "hero") {
			 platformScript.setDeath (true);
		}
	}
}
