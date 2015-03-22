using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class UIListener : MonoBehaviour {

	public GameObject quitPanel;
	public Text radioText;
	public Image radioImage;
	public UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration effect;

	public float messageTime = 10f;
	public float timeSlowdown = 0.5f;

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

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(Time.timeScale > 0f) {
				quitPanel.SetActive(true);
				Time.timeScale = 0f;
			} else {
				quitPanel.SetActive(false);
				Time.timeScale = 1f;
			}
		}
	}


	void MessageLog(string message) {
		StartCoroutine (logMessage(message));
	}
	
	IEnumerator logMessage(string m) {
		while(!string.IsNullOrEmpty(radioText.text)) 
			yield return new WaitForSeconds(messageTime * timeSlowdown);

		effect.enabled = true;
		Time.timeScale = timeSlowdown;
		radioText.text = "[RADIO]: " + m;
		radioImage.gameObject.SetActive(true);
		float counter = 0;
		effect.chromaticAberration = 0;
		while(messageTime * timeSlowdown > counter) {
			counter += Time.deltaTime;
			effect.chromaticAberration += counter;
			yield return new WaitForEndOfFrame();
		}
		effect.enabled = false;
		radioText.text = "";
		radioImage.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	// Radio events
	void OnHostageSaved() {
		if(PlayerPrefs.GetInt("SAVED_MESSAGE",0) == 1) return;
		MessageLog ("Bad news: checkpoint is broken. Seems you can only use them once...");
		PlayerPrefs.SetInt ("SAVED_MESSAGE",1);
	}

	void OnLowSpeed() {
		if(PlayerPrefs.GetInt("LOWSPEED_MESSAGE",0) == 1) return;
		MessageLog ("Try to keep up the pace. Any slower than 50 Km/h and the bomb will go off!");
		PlayerPrefs.SetInt ("LOWSPEED_MESSAGE",1);
	}

	void OnHit () {
		if(PlayerPrefs.GetInt("HIT_MESSAGE",0) == 1) return;
		MessageLog ("I know it's hard, but keep away from other cars or the bomb might go off");
		PlayerPrefs.SetInt ("HIT_MESSAGE",1);
	}

	// GUI events
	public void OnRestartClicked() {
		Application.LoadLevel (0);
	}

	public void OnQuitClicked() {
		Application.Quit();
	}

}
