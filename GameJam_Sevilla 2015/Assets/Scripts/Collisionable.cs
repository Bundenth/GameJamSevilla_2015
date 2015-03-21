using UnityEngine;
using System.Collections;

public class Collisionable : MonoBehaviour {

	public float damage = 5f;
	public float colliderPauseTime = 3f;

	void OnCollisionStay(Collision col) {
		if(col.collider.tag == "Player") {
			col.collider.GetComponentInParent<Player>().ApplyDamage(damage);
			StartCoroutine(PauseCollider());
		}
	}

	private IEnumerator PauseCollider() {
		GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(colliderPauseTime);
		GetComponent<Collider>().enabled = true;

	}
}
