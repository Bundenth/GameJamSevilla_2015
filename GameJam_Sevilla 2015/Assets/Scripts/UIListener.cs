using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIListener : MonoBehaviour {

	public Text radioText;
	public Image radioImage;

	public float messageTime = 10f;
	public float timeSlowdown = 0.5f;

	private bool savedMessage = false;
	private bool lowSpeedMessage = false;
	private bool hitMessage = false;

	void Start() {
		HostageDetector.OnHostageSaved += OnHostageSaved;
		Player.OnLowSpeed += OnLowSpeed;
		Player.OnHit += OnHit;
	}

	void OnDisable() {
		HostageDetector.OnHostageSaved -= OnHostageSaved;
		Player.OnLowSpeed -= OnLowSpeed;
		Player.OnHit -= OnHit;
	}

	void MessageLog(string message) {
		StartCoroutine (logMessage(message));
	}
	
	IEnumerator logMessage(string m) {
		while(!string.IsNullOrEmpty(radioText.text)) 
			yield return new WaitForSeconds(messageTime * timeSlowdown);

		Time.timeScale = timeSlowdown;
		radioText.text = "[RADIO]: " + m;
		radioImage.gameObject.SetActive(true);
		yield return new WaitForSeconds(messageTime * timeSlowdown);
		radioText.text = "";
		radioImage.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	// Radio events
	void OnHostageSaved() {
		if(savedMessage) return;
		MessageLog ("Bad news: checkpoint broke. Seems you can only use them once...");
		savedMessage = true;
	}

	void OnLowSpeed() {
		if(lowSpeedMessage) return;
		MessageLog ("Try to keep up the pace. Any slower than 50 Km/h and the bomb will go off!");
		lowSpeedMessage = true;
	}

	void OnHit () {
		if(hitMessage) return;
		MessageLog ("I know it's hard, but keep away from other cars or the bomb might go off");
		hitMessage = true;
	}

	// GUI events
	public void OnRestartClicked() {
		Application.LoadLevel (0);
	}


}
