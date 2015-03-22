using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class HomingHostage : MonoBehaviour {

	// EVENTS
	public delegate void HostageKilled();
	
	public static event HostageKilled OnHostageKilled;
	//

	public float speed = 3f;

	private const float THRESHOLD = 0.01f;

	public void PushHostage (Vector3 targetForce) {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForce(targetForce * speed,ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision col) {
		// KILL HOSTAGE
		if(OnHostageKilled != null) OnHostageKilled();
	}
}
