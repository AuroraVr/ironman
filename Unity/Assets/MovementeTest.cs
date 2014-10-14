using UnityEngine;
using System.Collections;

public class MovementeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			if(Input.GetKey(KeyCode.W)){
				transform.Translate(Vector3.forward * 10 * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.S)){
				transform.Translate(-Vector3.forward * 10 * Time.deltaTime);
			}
		}
	}
}
