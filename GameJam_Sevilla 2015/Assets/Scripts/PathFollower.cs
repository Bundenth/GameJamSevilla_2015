using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFollower : MonoBehaviour {

	public Transform[] pathPoints;

	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination(pathPoints[0].position);
	}
}
