using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

[RequireComponent(typeof(CarController))]
public class Player : MonoBehaviour {

	public Text speedText;
	public Text healthText;
	public Text hostagesText;

	public float lowSpeedTime = 3f;
	public float speedLimit = 50f;
	public float dropOffHostageTopSpeed = 60f;

	private CarController car;
	private float lowSpeedCounter;
	private float health;
	private int hostages;

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

	}

	void LateUpdate () {
		if(CheckSpeedLimit() || CheckHealth()) {
			// DIE!
			Debug.Log ("DEAD!");
		}
		if(CheckHostages()) {
			Debug.Log ("VICTORY!");
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

	public void ApplyDamage(float damage) {
		health -= damage;
		Debug.Log ("Damaged!");
	}

	public bool DropOffHostage() {
		if(car.CurrentSpeed * SPEED_CONVERSION <= dropOffHostageTopSpeed) {
			hostages--;
			return true;
		}
		return false;
	}
}
