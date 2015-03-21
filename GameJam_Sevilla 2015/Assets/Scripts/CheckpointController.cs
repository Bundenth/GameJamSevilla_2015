using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

	void OnTriggerStay(Collider col) {
		if(col.tag == "Player") {
			if(col.GetComponentInParent<Player>().DropOffHostage()) {
				Debug.Log ("DROPPED OFF!!");
				GetComponent<Collider>().enabled = false;
			}
		}
	}
}
