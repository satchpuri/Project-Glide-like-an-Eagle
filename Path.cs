using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {

    //make path nodes
	public Transform[] nodes;
	//get node position
	public Vector3 GetNodePos(int id){
		return nodes[id].position;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
