using UnityEngine;
using System.Collections;

public class DestroyFireball : MonoBehaviour {

	Transform parent;
	// Use this for initialization
	void Start () {
		parent = parent = GameObject.Find("RigidHand(Clone)").transform.Find("palm");
	}
	
	// Update is called once per frame
	void Update () {
		if(parent==null)
			Destroy(gameObject);
	}
}
