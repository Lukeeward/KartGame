using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class TrackGeneration : MonoBehaviour {


	private GameObject kartObject;
	private List<GameObject> track;
	private GameObject player;
	private bool playerDead;
	// Use this for initialization
	void Start () {
		track = new List<GameObject> ();
		kartObject = GameObject.Find ("kart");
		player = GameObject.Find ("Player");
		playerDead = false;
		for (int i = 0; i < 10; i++) {
			if(i == 0){
				GameObject tester = Instantiate(Resources.Load("shorttrack") as GameObject);
				tester.transform.position = kartObject.transform.position;
				track.Add (tester);
			} else {
				track.Add(Instantiate(Resources.Load("shorttrack") as GameObject));
				track.ElementAt(i).transform.position = new Vector3 (track.ElementAt(i-1).transform.position.x + track.ElementAt(i-1).GetComponent<Renderer>().bounds.size.x, track.ElementAt(i-1).transform.position.y, track.ElementAt(i-1).transform.position.z);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (!playerDead) {
			for (int i = 0; i < 10; i++) {
				if ((!track.ElementAt (i).GetComponent<Renderer> ().isVisible) && !(track.ElementAt (i).transform.position.x > player.transform.position.x)) {
					int trackAction = Random.Range (1, 10);
					GameObject tester5 = track.ElementAt (i);
					track.RemoveAt (i);
					tester5.SetActive (true);
					tester5.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
					tester5.transform.position = new Vector3 (track.LastOrDefault ().transform.position.x + track.LastOrDefault ().GetComponent<Renderer> ().bounds.size.x, track.LastOrDefault ().transform.position.y, track.LastOrDefault ().transform.position.z);
					if (trackAction <= 3) {
						if (!(track.LastOrDefault ().activeSelf) && !((track [track.Count - 2]).activeSelf)) {
						} else {
							if ((track.LastOrDefault ().activeSelf) && !((track [track.Count - 2]).activeSelf)) {
						
							} else {
								tester5.SetActive (false);
							}
						}
					}
					GameObject tt = (track.LastOrDefault ());
					GameObject kk = track [track.Count - 2];
					if (trackAction >= 8 && !(!tt.activeSelf && !kk.activeSelf)) {
						tester5.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 16));
						tester5.transform.position = new Vector3 (tester5.transform.position.x, tester5.transform.position.y + 1.2f, tester5.transform.position.z);
					}
					if (tt.transform.eulerAngles == new Vector3 (0, 0, 16) && tt.activeSelf) {
						tester5.transform.position = new Vector3 (tester5.transform.position.x - 0.27f, tester5.transform.position.y + (1f), tester5.transform.position.z);
					}
					if (tt.transform.eulerAngles == new Vector3 (0, 0, 16) && tt.activeSelf && !tester5.activeSelf) {
						tester5.SetActive (true);
					}
					print (trackAction);
					track.Add (tester5);
				}
			}
		}
	}

	public void resetGame(){
		playerDead = false;
		for (int i = 0; i < track.Count; i++) {
			track.ElementAt(i).transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			track.ElementAt (i).SetActive (true);
			if (i == 0) {
				track.ElementAt (i).transform.position = kartObject.transform.position;
			} else {
				track.ElementAt(i).transform.position = new Vector3 (track.ElementAt(i-1).transform.position.x + track.ElementAt(i-1).GetComponent<Renderer>().bounds.size.x, track.ElementAt(i-1).transform.position.y, track.ElementAt(i-1).transform.position.z);
			}
		}
	}

	public void deadPlayer(){
		playerDead = true;
	}
}
