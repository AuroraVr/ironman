using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("TRIGGER Collided with "+other.transform.name);
		Destroy (other.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log(" COLLISION Collided with "+collision.gameObject);
		Destroy (collision.gameObject);
	}
}
