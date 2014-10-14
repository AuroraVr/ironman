using UnityEngine;
using System.Collections;

public class FollowMyRotation : MonoBehaviour {

	public GameObject head;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		head.transform.localRotation = transform.localRotation;
	}
}
