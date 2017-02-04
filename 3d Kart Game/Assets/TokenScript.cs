using UnityEngine;
using System.Collections;

public class TokenScript : MonoBehaviour {

	public ScoreScript scoreScript;
	private audioScript audioscript;
	// Use this for initialization
	void Start () {
		audioscript = GameObject.Find ("Player").GetComponent<audioScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.transform.Rotate (0, -5, 0);
	}
	void OnTriggerEnter(Collider col){
		audioscript.playToken ();
		this.gameObject.SetActive(false);
		scoreScript.AddScore (1);
	}
}
