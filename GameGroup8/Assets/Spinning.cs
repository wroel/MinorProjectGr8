﻿using UnityEngine;
using System.Collections;

public class Spinning : MonoBehaviour {

	public float speed = 10f;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	
	}
}
