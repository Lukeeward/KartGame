using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	private Vector3 moveDirection = Vector3.zero;
	public CharacterController enemyController;
	private float speed = 250.0F;
	public float jumpSpeed = 11.0F;
	public float gravity = 20.0F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveDirection = new Vector3 (-5, 0, 0);
		moveDirection = transform.TransformDirection (moveDirection);
		moveDirection *= speed;
		moveDirection.y -= gravity * Time.deltaTime;
		enemyController.Move (moveDirection * Time.deltaTime);
	}
}
