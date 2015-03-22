using UnityEngine;
using System.Collections;

public class UIListener : MonoBehaviour {

	public void OnRestartClicked() {
		Application.LoadLevel (0);
	}


}
