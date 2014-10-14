using UnityEngine;
using System.Collections;

public class HeadScript : MonoBehaviour {

	public GameObject toFollow;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine){
			if(toFollow != null){
				transform.position = toFollow.transform.position;
				transform.rotation = toFollow.transform.rotation;
			}
		}
	}
	
}
