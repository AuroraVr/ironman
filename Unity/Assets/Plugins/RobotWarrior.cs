using UnityEngine;
using System.Collections;

public class RobotWarrior : MonoBehaviour {

	public Transform head;
	public float yOffset = 0.0f;
	public float xOffset = 0.0f;
	public float zOffset = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(head!=null){
			Vector3 newpos;
			newpos = head.position;
			newpos.y += yOffset;
			newpos.x += xOffset;
			newpos.z += zOffset;
			transform.position = newpos;
		}
	}
}
