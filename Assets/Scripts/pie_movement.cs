using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pie_movement : MonoBehaviour {

	public float pieMoveSpeed;
	bool moveLeft = true;
	float moveVal = 0.0f;
	public float moveRange = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (moveVal);
		if (moveLeft) {
			transform.Translate (-Vector2.right * pieMoveSpeed * Time.deltaTime);
			moveVal += 0.1f;
			if (moveVal >= moveRange) {
				moveVal = 0.0f;
				moveLeft = false;
			}
		}
		if (!moveLeft) {
			transform.Translate (Vector2.right * pieMoveSpeed * Time.deltaTime);
			moveVal -= 0.1f;
			if (moveVal <= -moveRange) {
				moveVal = 0.0f;
				moveLeft = true;
			}
		}
	}
}
