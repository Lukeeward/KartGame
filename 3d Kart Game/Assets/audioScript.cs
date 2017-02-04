using UnityEngine;
using System.Collections;


public class audioScript : MonoBehaviour {
	bool wasGrounded = false;
	private CharacterController controller;
	public AudioSource audioTrack;
	public AudioSource audioTrackLeave;
	public AudioSource audioTrackEnter;
	public AudioSource audioToken;
	public AudioSource audioDeath;
	void Start(){
		controller = GetComponent<CharacterController>();
	}
	void Update()
	{
		if (controller.isGrounded && !wasGrounded) { // just hit the ground
			audioTrackEnter.Play ();
			audioTrack.Play ();
		}
		else if (wasGrounded && !controller.isGrounded) { // just left the ground
			audioTrackLeave.Play ();
			audioTrack.Pause ();
		}
		wasGrounded = controller.isGrounded;
	}

	public void stopTrackSound(){
		audioTrack.Pause ();
	}

	public void startTrackSound (){
		audioTrack.pitch = 1f;
		audioTrack.Play ();
	}

	public void increaseTrackPitch() {
		if(audioTrack.pitch < 2.5f) {
			audioTrack.pitch = audioTrack.pitch + 0.05f;
		}
	}

	public void resetTrackPitch() {
		audioTrack.pitch = 1f;
	}


	public void playToken(){
		audioToken.Play ();
	}

	public void playDeath(){
		audioDeath.Play ();
	}

}
