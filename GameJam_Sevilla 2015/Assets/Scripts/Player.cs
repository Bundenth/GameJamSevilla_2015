﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

[RequireComponent(typeof(CarController))]
public class Player : MonoBehaviour {

	public Text speedText;
	public Text healthText;
	public Text hostagesText;
	public Text rescuedText;

	public float lowSpeedTime = 3f;
	public float speedLimit = 50f;
	public float dropOffHostageTopSpeed = 60f;
	public float rescueDistance = 2.5f;

	private CarController car;
	private float lowSpeedCounter;
	private float health;
	private int hostages;
	public int rescued = 0;

	private const float SPEED_CONVERSION = 10f;
	private const float MAX_HEALTH = 100f;
	private const int NUM_HOSTAGES = 10;

	// Use this for initialization
	void Start () {
		car = GetComponent<CarController>();
		health = MAX_HEALTH;
		hostages = NUM_HOSTAGES;
	}
	
	void Update() {
		if(speedText) {
			speedText.text = "Speed: " + car.CurrentSpeed*SPEED_CONVERSION;
			if(car.CurrentSpeed*SPEED_CONVERSION < speedLimit) speedText.color = Color.red;
			else speedText.color = Color.white;
		}
		if(hostagesText) hostagesText.text = "Hostages: " + hostages;
		if(healthText) healthText.text = "Health: " + health;
		if(rescuedText) rescuedText.text = "Rescued: " + rescued;

		if(Input.GetButtonDown ("Fire1")) {
			ReleaseHostage();
		}
	}

	void LateUpdate () {
		if(CheckSpeedLimit() || CheckHealth()) {
			// DIE!
			Debug.Log ("DEAD!");
		}
		if(CheckHostages()) {
			Debug.Log ("FINISHED!");
		}
	}

	bool CheckSpeedLimit() {
		if(car.CurrentSpeed * SPEED_CONVERSION < speedLimit) {
			lowSpeedCounter += Time.deltaTime;
		} else {
			lowSpeedCounter = 0;
		}
		if(lowSpeedCounter >= lowSpeedTime) {
			Debug.Log ("My grandma drives faster than you");
			lowSpeedCounter = 0;
			return true;
		}
		return false;
	}

	bool CheckHealth() {
		return health <= 0;
	}

	bool CheckHostages() {
		return hostages <= 0;
	}

	void ReleaseHostage() {
		hostages--;
		if(car.CurrentSpeed * SPEED_CONVERSION <= dropOffHostageTopSpeed) {
			RaycastHit hit;
			if(Physics.SphereCast(transform.position,0.5f,transform.right,out hit,rescueDistance,(1 << LayerMask.NameToLayer("Checkpoint")))) {
				Debug.Log (hit.collider.name);
				Debug.Log ("Well done! Rescued");
				hit.collider.enabled = false;
				rescued++;
			} 
		}
	}

	public void ApplyDamage(float damage) {
		health -= damage;
		Debug.Log ("Damaged!");
	}
}
