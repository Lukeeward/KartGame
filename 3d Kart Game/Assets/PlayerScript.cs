using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public MenuScript menu;
	public ScoreScript score;
	private GameObject goldkart9;
	private GameObject goldkart10;
	private GameObject goldkart11;
	private GameObject goldkart12;
	private GameObject goldkart13;
	private GameObject goldkart14;
	private GameObject goldkart15;
	private GameObject goldkart16;
	private GameObject goldkart17;
	private GameObject goldkart18;

	private Vector3 moveDirection = Vector3.zero;
	private Vector3 startPosition;
	private Quaternion startRotation;
	private bool isDead = false;
	private RaycastHit hit;
	private float zPosition;
	private GameObject kart;
	private GameObject wheelfr;
	private GameObject wheelfl;
	private GameObject wheelbr;
	private GameObject wheelbl;
	private CharacterController controller;
	public TrackGeneration trackGen;
	private GameObject kartObject;

	private bool started = false;
	private bool playerDying = false;
	private bool hitObsticle = false;
	private float obsticleTimer = 0;

	private bool onSlope;

	// Use this for initialization
	void Awake () 
	{
		wheelfr = GameObject.Find("wheelfr");
		wheelfl = GameObject.Find("wheelfl");
		wheelbr = GameObject.Find("wheelbr");
		wheelbl = GameObject.Find("wheelbl");
		kart = GameObject.Find ("MineKart");
		kartObject = GameObject.Find ("kart");


		goldkart9 = GameObject.Find ("kart9");
		goldkart10 = GameObject.Find ("kart10");
		goldkart11 = GameObject.Find ("kart11");
		goldkart12 = GameObject.Find ("kart12");
		goldkart13 = GameObject.Find ("kart13");
		goldkart14 = GameObject.Find ("kart14");
		goldkart15 = GameObject.Find ("kart15");
		goldkart16 = GameObject.Find ("kart16");
		goldkart17 = GameObject.Find ("kart17");
		goldkart18 = GameObject.Find ("kart18");
		startPosition = transform.position;
		startRotation = transform.rotation;
		goldkart9.SetActive (false);
		goldkart10.SetActive (false);
		goldkart11.SetActive (false);
		goldkart12.SetActive (false);
		goldkart13.SetActive (false);
		goldkart14.SetActive (false);
		goldkart15.SetActive (false);
		goldkart16.SetActive (false);
		goldkart17.SetActive (false);
		goldkart18.SetActive (false);
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		if (hitObsticle) {
			obsticleTimer++;
			if (obsticleTimer > 20) {
				hitObsticle = false;
			}
		}

		if (started && !playerDying) {
			gravity = 20f;
			float snapdistance = 1.5f;
			RaycastHit hitInfo = new RaycastHit ();
			if (Physics.Raycast (new Ray (transform.position, Vector3.down), out hitInfo, snapdistance)) {
				transform.position = hitInfo.point;
				transform.position = new Vector3 (hitInfo.point.x, hitInfo.point.y + controller.height / 2, hitInfo.point.z);
			}

			//print ("IsGrounded: " + controller.isGrounded);
			if (!isDead) {
				turnWheel ();
				if (controller.isGrounded) {
					if (!hitObsticle) {
						rotateToSurface ();
					}
					moveDirection = new Vector3 (-2, 0, 0);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed;
					if (Input.GetButton ("Jump")) {
						moveDirection.y = jumpSpeed;
					}
					if (Input.GetMouseButtonDown(0)) {
							moveDirection.y = jumpSpeed;
					}
				}

				if (transform.position.z != startPosition.z) {
					moveDirection.z = (startPosition.z - transform.position.z) * 0.05f;
				}

				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move (moveDirection * Time.deltaTime);
				//controller.Move(new Vector3(10,0,0) * Time.deltaTime);
				//print ("Gravity: " + gravity);
			}
			if (isDead) {
				respawnPlayer (controller);
			}
		}
	}

	public void setDeath(bool death){
		isDead = death;
		playerDying = false;
		trackGen.deadPlayer ();
	}

	void respawnPlayer(CharacterController controller){
		transform.position = startPosition;
		kart.transform.rotation = startRotation;
		setDeath (false);
		started = false;
		resetColour ();
		menu.showMenu ();
		score.resetTokens ();
		score.resetScore ();
		resetKartGold ();
		trackGen.resetGame ();
	}

	float checkAngle(){
		if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {
			return Vector3.Angle (hit.normal, Vector3.up);
		} else {
			return 0f;
		}
	}

	void turnWheel(){
		wheelfr.transform.Rotate (0, 0, -5);
		wheelfl.transform.Rotate (0, 0, -5);
		wheelbr.transform.Rotate (0, 0, -5);
		wheelbl.transform.Rotate (0, 0, -5);
	}

	public void startGame(){
		started = true;
	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		if (hit.transform.tag == "Obstacle" && !hitObsticle) {
			hitObsticle = true;
			print ("yoyoyiggityyo");
			obsticleTimer = 0;
			StartCoroutine(obstacleDeath());
			playerDying = true;
		}
	}

	void rotateToSurface(){
		float angle;
		angle = checkAngle ();
		if (angle > 10) {
			//moveDirection = new Vector3 (-2, 0, 0);
			kart.transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
			onSlope = true;
		} else {
			if (onSlope) {
				kart.transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
				onSlope = false;
			}
		}
	}

	void setDamageColour(){
		GameObject player = GameObject.Find("Player");
		//player.transform.position = new Vector3 (player.transform.position.x,player.transform.position.y,player.transform.position.z + 5);
		//kart.transform.rotation = Quaternion.ro (new Vector3 (kart.transform.rotation.x, kart.transform.rotation.y + 0.5f, kart.transform.rotation.z));
		Renderer[] rend = player.GetComponentsInChildren<Renderer>();
		foreach (Renderer ren in rend) {
			ren.material.color = Color.red;
		}
	}

	void resetColour(){
		GameObject player = GameObject.Find("Player");
		//player.transform.position = new Vector3 (player.transform.position.x,player.transform.position.y,player.transform.position.z + 5);
		//kart.transform.rotation = Quaternion.ro (new Vector3 (kart.transform.rotation.x, kart.transform.rotation.y + 0.5f, kart.transform.rotation.z));
		Renderer[] rend = player.GetComponentsInChildren<Renderer>();
		foreach (Renderer ren in rend) {
			ren.material.color = Color.white;
		}
	}


	IEnumerator obstacleDeath(){

		for (int i = 0; i < 5; i++) {
			setDamageColour ();
			yield return new WaitForSeconds (.1f);
			resetColour ();
			yield return new WaitForSeconds (.1f);
		}
		setDeath (true);
	}

	public void changekart(float scoreValue){
		if (scoreValue <= 8) {
			kartObject.SetActive (true);
		} else {
			kartObject.SetActive (false);
			if (scoreValue == 9) {
				goldkart9.SetActive (true);
			} else {
				if (scoreValue == 10) {
					goldkart9.SetActive (false);
					goldkart10.SetActive (true);
				} else {
					if (scoreValue == 11) {
						goldkart10.SetActive (false);
						goldkart11.SetActive (true);
					} else {
						if (scoreValue == 12) {
							goldkart11.SetActive (false);
							goldkart12.SetActive (true);
						} else {
							if (scoreValue == 13) {
								goldkart12.SetActive (false);
								goldkart13.SetActive (true);
							} else {
								if (scoreValue == 14) {
									goldkart13.SetActive (false);
									goldkart14.SetActive (true);
								} else {
									if (scoreValue == 15) {
										goldkart14.SetActive (false);
										goldkart15.SetActive (true);
									} else {
										if (scoreValue == 16) {
											goldkart15.SetActive (false);
											goldkart16.SetActive (true);
										} else {
											if (scoreValue == 17) {
												goldkart16.SetActive (false);
												goldkart17.SetActive (true);
											} else {
												goldkart17.SetActive (false);
												goldkart18.SetActive (true);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
	void resetKartGold(){
		goldkart9.SetActive (false);
		goldkart10.SetActive (false);
		goldkart11.SetActive (false);
		goldkart12.SetActive (false);
		goldkart13.SetActive (false);
		goldkart14.SetActive (false);
		goldkart15.SetActive (false);
		goldkart16.SetActive (false);
		goldkart17.SetActive (false);
		goldkart18.SetActive (false);
		kartObject.SetActive (true);
	}
}