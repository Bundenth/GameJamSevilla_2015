using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	//CarController1.js
	public Transform[] wheels;
	
	public float enginePower=150.0f;
	
	public float power=0.0f;
	public float brake=0.0f;
	public float steer=0.0f;
	
	public float maxSteer=25.0f;
	
	void Start(){
		//rigidbody. = Vector3(0,-0.5,0.3);
	}
	
	void Update () {
		power=Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0f;
		steer=Input.GetAxis("Horizontal") * maxSteer;
		brake=Input.GetKey("space") ? GetComponent<Rigidbody>().mass * 0.3f: 0.0f;
		
		GetCollider(0).steerAngle=steer;
		GetCollider(1).steerAngle=steer;
		
		if(brake > 0.0){
			GetCollider(0).brakeTorque=brake;
			GetCollider(1).brakeTorque=brake;
			GetCollider(2).brakeTorque=brake;
			GetCollider(3).brakeTorque=brake;
			GetCollider(2).motorTorque=0.0f;
			GetCollider(3).motorTorque=0.0f;
		} else {
			GetCollider(0).brakeTorque=0;
			GetCollider(1).brakeTorque=0;
			GetCollider(2).brakeTorque=0;
			GetCollider(3).brakeTorque=0;
			GetCollider(2).motorTorque=enginePower * Time.deltaTime * 250f;
			GetCollider(3).motorTorque=enginePower * Time.deltaTime * 250f;
		}
	}
	
	WheelCollider GetCollider(int n) {
		return wheels[n].gameObject.GetComponent<WheelCollider>();
	}
}
