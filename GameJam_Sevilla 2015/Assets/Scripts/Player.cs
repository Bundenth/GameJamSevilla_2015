using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

[RequireComponent(typeof(CarController))]
public class Player : MonoBehaviour {

	// EVENTS
	public delegate void LowSpeed();
	public delegate void Hit();

	public static event LowSpeed OnLowSpeed;
	public static event Hit OnHit;
	//

	public GameObject[] hostagesPrefabs;
	public GameObject explosionPrefab;

	public Text speedText;
	public Text healthText;
	public Text hostagesText;
	public Text rescuedText;
	public Text messageText;

	// ENDINGS
	public GameObject gameOverPanel;
	public GameObject winEnding1;
	public GameObject winEnding2;

	public float lowSpeedTime = 3f;
	public float speedLimit = 50f;
	public float dropOffHostageTopSpeed = 60f;
	public float rescueDistance = 2.5f;

	private CarController car;
	private float lowSpeedCounter;
	private float health;
	private int hostages;
	private int rescued = 0;
	private float startTime;

	private const float SPEED_CONVERSION = 10f;
	private const float MAX_HEALTH = 100f;
	private const int NUM_HOSTAGES = 10;
	private const float GRACE_PERIOD = 5;

	// Use this for initialization
	void Start () {
		car = GetComponent<CarController>();
		health = MAX_HEALTH;
		hostages = NUM_HOSTAGES;
		startTime = Time.time;

		HomingHostage.OnHostageKilled += OnHostageKilled;
		HostageDetector.OnHostageSaved += OnHostageSaved;
	}

	void OnDisable() {
		HomingHostage.OnHostageKilled -= OnHostageKilled;
		HostageDetector.OnHostageSaved -= OnHostageSaved;
	}

	void Update() {
		if(speedText) {
			speedText.text = "Speed: " + car.CurrentSpeed*SPEED_CONVERSION;
			if(car.CurrentSpeed*SPEED_CONVERSION < speedLimit) {
				speedText.color = Color.red;
			} else if(car.CurrentSpeed*SPEED_CONVERSION < dropOffHostageTopSpeed) {
				speedText.color = Color.green;
			} else {
				speedText.color = Color.white;
			}
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
			gameOverPanel.SetActive(true);
			gameObject.SetActive(false);
			GameObject.Instantiate (explosionPrefab,transform.position,Quaternion.identity);
		}
		if(CheckHostages()) {
			Debug.Log ("FINISHED!");
			EndGame();
			gameObject.SetActive(false);
		}
	}

	bool CheckSpeedLimit() {
		if(startTime + GRACE_PERIOD > Time.time) return false;
		if(car.CurrentSpeed * SPEED_CONVERSION < speedLimit) {
			if(OnLowSpeed != null) OnLowSpeed();
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
		if(car.CurrentSpeed * SPEED_CONVERSION <= dropOffHostageTopSpeed) {
			HomingHostage h = (GameObject.Instantiate (hostagesPrefabs[Random.Range (0,hostagesPrefabs.Length)],transform.position + Vector3.up*0.5f,Quaternion.identity) as GameObject).GetComponent<HomingHostage>();
			h.PushHostage(transform.forward + transform.right);
			hostages--;
		} else {
			MessageLog("Too fast for the hostages to jump!");
		}
		/*hostages--;

		Debug.DrawRay(transform.position+Vector3.up*0.1f,transform.right,Color.red,rescueDistance);
		if(car.CurrentSpeed * SPEED_CONVERSION <= dropOffHostageTopSpeed) {
			RaycastHit hit;
			if(Physics.Raycast(transform.position+Vector3.up*0.1f,transform.right,out hit,rescueDistance,(1 << LayerMask.NameToLayer("Checkpoint")))) {
				Debug.Log ("Well done! Rescued " + hit.collider.name);
				hit.collider.enabled = false;
				rescued++;
			} 
		}*/
	}

	void EndGame() {
		if(rescued >= 5) {
			if(rescued > 8) {
				// ONE ENDING
				winEnding1.SetActive(true);
			} else {
				// SECOND ENDING
				winEnding2.SetActive (true);
			}
		} else {
			gameOverPanel.SetActive(true);
		}
	}

	void MessageLog(string message) {
		StartCoroutine (logMessage(message));
	}

	IEnumerator logMessage(string m) {
		while(!string.IsNullOrEmpty(messageText.text)) 
			yield return new WaitForEndOfFrame();

		messageText.text = m;
		yield return new WaitForSeconds(1f);
		messageText.text = "";
	}

	public void OnHostageKilled() {
		MessageLog("Hostage dead!");
	}

	public void OnHostageSaved() {
		rescued++;
		MessageLog("Saved!");
	}
	
	public void ApplyDamage(float damage) {
		health -= damage;
		MessageLog("Be careful!");
		if(OnHit != null) OnHit();
	}
}
