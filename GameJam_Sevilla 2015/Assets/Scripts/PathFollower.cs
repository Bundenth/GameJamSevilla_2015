﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFollower : MonoBehaviour {

	public Transform[] pathPoints;

	private NavMeshAgent agent;
	private int nextIndex = 0;

	// Use this for initialization
	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(pathPoints[0].position);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.frameCount % 3 == 0) return;
		if(agent.remainingDistance <= agent.stoppingDistance) {
			nextIndex++;
			if(nextIndex >= pathPoints.Length) nextIndex = 0;
			agent.SetDestination(pathPoints[nextIndex].position);
		}
	}
}
