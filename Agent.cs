using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	//to access the class path
	public Path path;
	//draw debug

	//distance to the path checkpoint
	public float reachDistance = 5f;
	//speed	
	public float speed = 500f;
	//rotation speed
	public float rotSpeed = 1f;
	//current checkpoint
	private int currentCheckpoint = 0;

    public bool drawDebug = false;

	//start
	void Start () {
		
	}

	//update
	protected void Update () {

		//destination
		Vector3 destination = path.GetNodePos (currentCheckpoint);
		Vector3 offset = destination - transform.position;

		//change rotation
		if (offset.sqrMagnitude > reachDistance) {
			offset = offset.normalized;
			transform.Translate (offset * speed * Time.deltaTime, Space.World);

			//to rotation
			Quaternion lookRot = Quaternion.LookRotation(offset);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotSpeed * Time.deltaTime);

			} else {
            //to reset
			currentCheckpoint++;
			if(currentCheckpoint >= path.nodes.Length){
				currentCheckpoint = 0;
				}
			}

        
	}

    /*
	void DebugLines() {

        if (drawDebug == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, path.GetNodePos(currentCheckpoint));
        }
	} */
} 