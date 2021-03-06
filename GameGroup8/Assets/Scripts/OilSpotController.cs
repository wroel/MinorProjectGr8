﻿using UnityEngine;
using System.Collections;

public class OilSpotController : MonoBehaviour {

	/// <summary>
	/// Slows the player and the walk animation down.
	/// </summary>
	void Start () {
        this.gameObject.transform.Rotate(-90, 0, 0);
        GameObject.Find("player").GetComponent<PlayerController>().speedMultiplier = 0.1f;
        GameObject.Find("player").GetComponent<PlayerController>().playerAnimator.speed = 0.25f;
    }

	/// <summary>
	/// When the player leaves the oil spot the walking speed is restored and the oil spot is destroyed.
	/// </summary>
	void Update () {
		if (Mathf.Abs (GameObject.Find ("player").transform.position.x - this.gameObject.transform.position.x) > 3 || Mathf.Abs (GameObject.Find ("player").transform.position.z - this.gameObject.transform.position.z) > 3) { 
			GameObject.Find ("player").GetComponent<PlayerController> ().speedMultiplier = 1f;
			GameObject.Find ("player").GetComponent<PlayerController> ().playerAnimator.speed = 1f;
			Destroy (this.gameObject);
		}
	}
}
