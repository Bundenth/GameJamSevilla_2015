using UnityEngine;
using System.Collections;

public class HostageDetector : MonoBehaviour {

	// EVENTS
	public delegate void HostageSaved();

	public static event HostageSaved OnHostageSaved;
	//

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Hostage") {
			// HOSTAGE SAVED!
			Destroy(col.gameObject);
			if(OnHostageSaved != null) OnHostageSaved();
			DeactivateCheckpoint();
		}
	}

	private void DeactivateCheckpoint() {
		Renderer[] rs = GetComponents<Renderer>();
		foreach(Renderer r in rs) {
			foreach(Material m in r.materials) {
				m.color = Color.grey;
			}
		}
	}
}
