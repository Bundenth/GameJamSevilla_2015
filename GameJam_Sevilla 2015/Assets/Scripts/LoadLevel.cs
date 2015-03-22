using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LoadLevel : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(CrossPlatformInputManager.GetButtonDown ("Fire1")) {
			Application.LoadLevel (1);
		}
	}
}
