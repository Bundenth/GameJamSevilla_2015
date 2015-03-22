using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float positionDamping = 3f;
	public float angleDamping = 3f;
	public float xAngle = 36f;
	public float height = 3f;
	public float distance = 2.5f;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = new Vector3(0,-height,distance);//target.position - transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!target) return;
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * angleDamping);
		
		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = Vector3.Lerp (transform.position,target.transform.position - (rotation * offset),Time.deltaTime*positionDamping);
		
		transform.LookAt(target.transform);
		transform.eulerAngles = new Vector3(xAngle,transform.eulerAngles.y,transform.eulerAngles.z);

	}
}
