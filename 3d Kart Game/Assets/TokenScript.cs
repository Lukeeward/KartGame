using UnityEngine;
using System.Collections;

public class TokenScript : MonoBehaviour {

	public ScoreScript scoreScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.transform.Rotate (0, -5, 0);
	}
	void OnTriggerEnter(Collider col){
		this.gameObject.SetActive(false);
		scoreScript.AddScore (1);
	}
}
