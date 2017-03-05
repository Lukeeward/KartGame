using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class TrackGeneration : MonoBehaviour {


	private GameObject kartObject;
	private List<GameObject> track;
	private List<GameObject> backgrounds;
	private List<GameObject> gold;
	private List<GameObject> obstacles;
	private List<GameObject> enemies;
	private List<GameObject> pretrackpeices;
	public GameObject player;
	public bool floorDebug = true; //DEBUG: STOPS HOLES APPEARING
	public bool obsDebug = true; //DEBUG: STOPS OBSTICLES APPEARING
	private bool playerDead;
	public ScoreScript scoreUI;
	private Vector3 startPosition;
	float trackSize;
	// Use this for initialization
	void Start () {
		track = new List<GameObject> ();
		backgrounds = new List<GameObject> ();
		obstacles = new List<GameObject> ();
		gold = new List<GameObject> ();
		pretrackpeices = new List<GameObject> ();
		kartObject = GameObject.Find ("kart1");
		playerDead = false;
		enemies = new List<GameObject> ();
		startPosition = player.transform.position;
		//Initiate Track
		for (int i = 0; i < 10; i++) {
			if(i == 0){
				GameObject tester = Instantiate(Resources.Load("trackwithend") as GameObject);
				tester.transform.position = kartObject.transform.position;
				track.Add (tester);
			} else {
				track.Add(Instantiate(Resources.Load("trackwithend") as GameObject));
				track.ElementAt(i).transform.position = new Vector3 (track.ElementAt(i-1).transform.position.x + track.ElementAt(i-1).GetComponent<Renderer>().bounds.size.x, track.ElementAt(i-1).transform.position.y, track.ElementAt(i-1).transform.position.z);
			}
			track.ElementAt (i).transform.FindChild ("support").gameObject.SetActive (false);
			track.ElementAt (i).transform.FindChild ("endLeft").gameObject.SetActive (false);
			track.ElementAt (i).transform.FindChild ("endRight").gameObject.SetActive (false);
		}
		//pretrack peices
		for(int i = 0; i < 3; i++){
			if (i == 0) {
				GameObject pretrack = Instantiate (Resources.Load ("trackwithsupport") as GameObject);
				pretrack.transform.position = new Vector3 (track.First().transform.position.x - track.First().GetComponent<Renderer> ().bounds.size.x, track.First().transform.position.y, track.First().transform.position.z);
				pretrackpeices.Add (pretrack);
			} else {
				pretrackpeices.Add (Instantiate (Resources.Load ("trackwithsupport") as GameObject));
				pretrackpeices.ElementAt(i).transform.position = new Vector3 (pretrackpeices.ElementAt(i-1).transform.position.x - pretrackpeices.ElementAt(i-1).GetComponent<Renderer>().bounds.size.x, pretrackpeices.ElementAt(i-1).transform.position.y, pretrackpeices.ElementAt(i-1).transform.position.z);
			}
			pretrackpeices.ElementAt (i).transform.FindChild ("support").gameObject.SetActive (false);
		}
		track.ElementAt (0).transform.FindChild ("support").gameObject.SetActive (true);
		track.ElementAt (4).transform.FindChild ("support").gameObject.SetActive (true);
		track.ElementAt (7).transform.FindChild ("support").gameObject.SetActive (true);
		track.ElementAt (9).transform.FindChild ("support").gameObject.SetActive (true);
		//Initiate Backgrounds
		for (int i = 0; i < 2; i++) {
			if(i == 0){
				GameObject tester = Instantiate(Resources.Load("backgroundPrefab(clone)") as GameObject);
				tester.transform.position = new Vector3(kartObject.transform.position.x,kartObject.transform.position.y,kartObject.transform.position.z- 20);
				backgrounds.Add (tester);
			} else {
				backgrounds.Add(Instantiate(Resources.Load("backgroundPrefab(clone)") as GameObject));
				backgrounds.ElementAt(i).transform.position = new Vector3 (backgrounds.ElementAt(i-1).transform.position.x + (backgrounds.ElementAt(i-1).GetComponentInChildren<Renderer>().bounds.size.x *2.6f + 85.18f), backgrounds.ElementAt(i-1).transform.position.y, backgrounds.ElementAt(i-1).transform.position.z);
			}
		}
		//Initate Gold
		for(int i = 0; i < 10; i++){
			GameObject goldPiece = Instantiate (Resources.Load ("GoldPoint") as GameObject);
			ScoreScript ttttt =  GameObject.Find ("Score").GetComponent<ScoreScript>();
			goldPiece.GetComponent<TokenScript> ().scoreScript = ttttt;
			goldPiece.SetActive (false);
			gold.Add (goldPiece);
		}
		//Initiate KartObsticles
		for(int i = 0; i < 5; i++){
			int obsrand = Random.Range (0, 10);
			GameObject kartObstacle;
			if (obsrand > 5) {
				kartObstacle = Instantiate (Resources.Load ("trackObs") as GameObject);	
			} else {
				kartObstacle = Instantiate (Resources.Load ("obsticleKart") as GameObject);
			}
			kartObstacle.SetActive (false);
			obstacles.Add (kartObstacle);
		}

		for (int i = 0; i < 2; i++) {
			GameObject enemy;
			switch (i) {
			case 1:
				enemy = Instantiate (Resources.Load ("fullkartEnemy2") as GameObject);
				break;
			case 2:
				enemy = Instantiate (Resources.Load ("fullkartEnemy3") as GameObject);
				break;
			default:
				enemy = Instantiate (Resources.Load ("fullkartEnemy") as GameObject);
				break;
			}
			enemy.SetActive (false);
			enemies.Add (enemy);
		}

		foreach (GameObject game in enemies) {
			game.SetActive (false);
		}

		trackSize = track.ElementAt(1).GetComponent<Renderer>().bounds.size.x;
	}

	// Update is called once per frame
	void Update () {
		//player is alive
		if (!playerDead) {

			//move background
			for (int i = 0; i < 2; i++) {
				if(!(backgrounds.ElementAt(i).GetComponentInChildren<Renderer>().isVisible) && !(backgrounds.ElementAt(i).transform.position.x > player.transform.position.x)){
					GameObject tester5 = backgrounds.ElementAt (i);
					backgrounds.RemoveAt (i);
					tester5.transform.position = new Vector3 (backgrounds.LastOrDefault ().transform.position.x + (backgrounds.LastOrDefault ().GetComponentInChildren<Renderer> ().bounds.size.x * 3 - 15.9f), backgrounds.LastOrDefault ().transform.position.y, backgrounds.LastOrDefault ().transform.position.z);
					backgrounds.Add (tester5);
				}
			}
				
			for (int i = 0; i < 10; i++) {
				//if track peice is behind player 
				if ((!track.ElementAt (i).GetComponent<Renderer> ().isVisible) && !(track.ElementAt (i).transform.position.x > player.transform.position.x)) {
					//randomly get track action number
					int trackAction = Random.Range (1, 10);
					int goldAction = Random.Range (1, 10);

					GameObject currentTrack = track.ElementAt (i);
					track.RemoveAt (i);
					currentTrack.SetActive (true);
					currentTrack.transform.FindChild ("endRight").gameObject.SetActive (false);
					currentTrack.transform.FindChild ("endLeft").gameObject.SetActive (false);
					currentTrack.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
					currentTrack.transform.position = new Vector3 (track.LastOrDefault ().transform.position.x + track.LastOrDefault ().GetComponent<Renderer> ().bounds.size.x, track.LastOrDefault ().transform.position.y, track.LastOrDefault ().transform.position.z);
					currentTrack.transform.FindChild ("support").gameObject.SetActive (true);
					//if less than 3 set active to false

					if (trackAction <= 3) {
						//Previous track is false and so is track before that
						if (!(track.LastOrDefault ().activeSelf) && !((track [track.Count - 2]).activeSelf)) {
						} else {
							//Previous track is true but one previous is not 
							if ((track.LastOrDefault ().activeSelf) && !((track [track.Count - 2]).activeSelf)) {
							//Maybe keep me having added end peices
							} else {
								//stops obsticle being followed by two holes
								if (!(track.LastOrDefault ().activeSelf) && (obsNearBy (track.LastOrDefault()))) {
									//print ("way too close");
								} else {
									if((track.LastOrDefault ().activeSelf) && (obsNearBy (track.LastOrDefault()))){
										//print ("et tu brute?");
									} else {
										if (!floorDebug) {
											currentTrack.SetActive (false);
											if(track.LastOrDefault ().activeSelf){
												track.LastOrDefault ().transform.FindChild ("endRight").gameObject.SetActive (true);
											}
											//Set the end peices to appear
										}
									}
								}
							}
						}
					}
					GameObject previousTrack = (track.LastOrDefault ());
					GameObject previousTrack2 = track [track.Count - 2];

					//If previous track is false but current is true then add left end peice
					if((!previousTrack.activeSelf) && currentTrack.activeSelf){
						currentTrack.transform.FindChild ("endLeft").gameObject.SetActive (true);
					}


					//if track action 8 rotate the track
					if (trackAction == 8 && !(!previousTrack.activeSelf && !previousTrack2.activeSelf)) {
						if(currentTrack.transform.position.y >= 14.6f){

						} else {
							currentTrack.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 16));
							currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y + 1.2f, currentTrack.transform.position.z);
						}
					}

					if (previousTrack.transform.eulerAngles == new Vector3 (0, 0, 16) && previousTrack.activeSelf) {
						currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x - 0.27f, currentTrack.transform.position.y + (1f), currentTrack.transform.position.z);
					}
					if (previousTrack.transform.eulerAngles == new Vector3 (0, 0, 16) && previousTrack.activeSelf && !currentTrack.activeSelf) {
						currentTrack.SetActive (true);
					}



					//if track action 9 rotate the track
					if (trackAction == 9 && !(!previousTrack.activeSelf && !previousTrack2.activeSelf)) {
						if(currentTrack.transform.position.y <= -14.6f){

						} else {
							currentTrack.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 344));
							currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x - 0.4f, currentTrack.transform.position.y - 1.08f, currentTrack.transform.position.z);
						}
					}
					if (previousTrack.transform.eulerAngles.z == 344 && previousTrack.activeSelf) {
						currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y - (1.09f), currentTrack.transform.position.z);
					}
					if (previousTrack.transform.eulerAngles == new Vector3 (0, 0, 344) && previousTrack.activeSelf && !currentTrack.activeSelf) {
						currentTrack.SetActive (true);
					}

					//If current track is rotated after a hole then add/take some hieght in order to account for end peice
					if (currentTrack.transform.eulerAngles == new Vector3 (0, 0, 16) && !previousTrack.activeSelf) {
						currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y + (1f), currentTrack.transform.position.z);
					}
					//Same as above but different angle
					if (currentTrack.transform.eulerAngles == new Vector3 (0, 0, 344) && !previousTrack.activeSelf) {
						currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y - (1f), currentTrack.transform.position.z);
					}
						
					//Support rules
					if (!currentTrack.activeSelf) {
						currentTrack.transform.FindChild ("support").gameObject.SetActive (false);
						//if track before last does not has support then add to previous track before current hole
						if(!track.ElementAt (track.Count - 1).transform.FindChild ("support").gameObject.activeSelf){
							track.LastOrDefault ().transform.FindChild ("support").gameObject.SetActive (false);
						}
					}

					//if last track was a hole make current track have support
					if(!track.LastOrDefault().activeSelf){
						currentTrack.transform.FindChild ("support").gameObject.SetActive (true);
					}
					//if track is rotated remove supports
					if(currentTrack.transform.eulerAngles != new Vector3 (0, 0, 0)){
						currentTrack.transform.FindChild ("support").gameObject.SetActive (false);
					}
					if (track.LastOrDefault ().transform.FindChild ("support").gameObject.activeSelf) {
						currentTrack.transform.FindChild ("support").gameObject.SetActive (false);
					}

					//Set end peice to false if previous track is visible and so is current
					//Current may have been previously set to false causing the previous tracks endpeice to be set to true
					//But since then current has been overwritten to true
					if(previousTrack.activeSelf && currentTrack.activeSelf){
						track.LastOrDefault ().transform.FindChild ("endRight").gameObject.SetActive (false);
					}

					//If last track is false but one before and current are true then widen the gap due to end peices
					if((!previousTrack.activeSelf) && previousTrack2.activeSelf && currentTrack.activeSelf){
						currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x + 7f, currentTrack.transform.position.y, currentTrack.transform.position.z);
					}
					//If lasttwo tracks are false then again, widen the gap
					if((!previousTrack.activeSelf) && (!previousTrack2.activeSelf) && currentTrack.activeSelf){
						currentTrack.transform.position = new Vector3 (currentTrack.transform.position.x + 6f, currentTrack.transform.position.y, currentTrack.transform.position.z);
					}


					//Set gold position above track
					//Set up the whokle gold thing
					bool goldfound = false;
					//Randomly decide to show the gold for this track peice
					if(goldAction <= 2){
						int xcount = 0;
							while (xcount < 10) {
							//A suitable peice of gold has been found to spawn from the pool
							if (!goldfound) {
								if (!gold.ElementAt (xcount).activeSelf || !gold.ElementAt(xcount).GetComponentInChildren<Renderer>().isVisible) {
									goldfound = true;
									gold.ElementAt (xcount).SetActive (true);
									gold.ElementAt (xcount).transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y + 4f, currentTrack.transform.position.z);
								}
							}
							xcount = xcount + 1;
						}
					}



					bool foundObs = false;
					bool tooClose = false;
					int obsRandom = Random.Range (1, 10);
					if (currentTrack.activeSelf) {
						for (int y = 0; y < 5; y++) {
							if(obstacles.ElementAt (y).transform.position.x <= currentTrack.transform.position.x + 20 &&  obstacles.ElementAt (y).transform.position.x >= currentTrack.transform.position.x - 20){
								tooClose = true;
							}
						}
						if (!tooClose&& (obsRandom >= 5)) {
							if(track.LastOrDefault().activeSelf){
								if(track.LastOrDefault().transform.eulerAngles == new Vector3 (0, 0, 0)){
									if (currentTrack.transform.eulerAngles == new Vector3 (0, 0, 0)) {
										if(!(track.LastOrDefault().activeSelf && !track[track.Count-2].activeSelf)){
											for (int y = 0; y < 5; y++) {
												Renderer obsChild = obstacles.ElementAt (y).GetComponentInChildren<Renderer> ();
												if (!obsChild.isVisible && !foundObs) {
													obstacles.ElementAt (y).transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y, currentTrack.transform.position.z);
													if(!obsDebug)
													{
														//enemies.ElementAt(y).transform.position = new Vector3 (currentTrack.transform.position.x, currentTrack.transform.position.y, currentTrack.transform.position.z);
														//enemies.ElementAt (y).SetActive (true);
														obstacles.ElementAt (y).SetActive (true);
													}

													foundObs = true;
												}
											}
										}
									}
								}
							}
						}
					}


					//print (trackAction);
					track.Add (currentTrack);
				}
			}
		}
		//checkDistanceScore ();
	}

	void FixedUpdate(){
		checkDistanceScore ();
	}

	public void checkDistanceScore(){
		float distanceDifference = player.transform.position.x - startPosition.x;
//		print (Mathf.Floor(distanceDifference/trackSize));
		scoreUI.addDistanceScore (Mathf.Floor(distanceDifference/trackSize));
	}

	public bool obsNearBy(GameObject trackToTest){
		for (int y = 0; y < 5; y++) {
			if(obstacles.ElementAt (y).transform.position.x <= trackToTest.transform.position.x + 20 &&  obstacles.ElementAt (y).transform.position.x >= trackToTest.transform.position.x - 20){
				return true;
			}
		}
		return false;
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
		//Reset the background to currentPlayerPosition
		for (int i = 0; i < 2; i++) {
			if(i == 0){
				backgrounds.ElementAt(i).transform.position = new Vector3(kartObject.transform.position.x,kartObject.transform.position.y,kartObject.transform.position.z- 20);
			} else {
				backgrounds.ElementAt(i).transform.position = new Vector3 (backgrounds.ElementAt(i-1).transform.position.x + (backgrounds.ElementAt(i-1).GetComponentInChildren<Renderer>().bounds.size.x *2.6f + 85.18f), backgrounds.ElementAt(i-1).transform.position.y, backgrounds.ElementAt(i-1).transform.position.z);
			}
		}

		for (int i = 0; i < 5; i++) {
			obstacles.ElementAt (i).SetActive (false);
		}

		for(int i = 0; i < 10; i++){
			gold.ElementAt(i).SetActive (false);
		}

	}

	public void deadPlayer(){
		playerDead = true;
	}


	public void startFromSavePosition(){
		if (player.transform.position.y < startPosition.y) {
			//move player up to reasonable position
			player.transform.position = new Vector3(player.transform.position.y, startPosition.x, player.transform.position.z);
		} else {
			//player died from collision, just reset the game.
			resetGame();
		}
	}

}
