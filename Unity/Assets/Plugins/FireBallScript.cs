using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour {

	GameObject palm;
	Quaternion palm_rotation;
	Transform parent;
	public bool move = false;
	public float speed = 5.0f;
	public float delayEnableCollision = 0.0f;
	public bool collide = false;
	public Transform detonator;

	void Start () {
		palm = GameObject.Find("RigidHand(Clone)");
		if (palm != null){
			palm_rotation = palm.transform.Find("palm").transform.rotation;
			transform.localRotation = palm_rotation;
		}
		if(collide)
			this.enableCollision();
		else
			this.disableCollision();
	}
	
	// Update is called once per frame
	void Update () {
		if(move)
			transform.Translate(-Vector3.up * Time.deltaTime * speed, Space.Self);
	}

	public void start_moving(){
		this.move = true;
		this.enableCollision(delayEnableCollision);
	}

	public void stop_moving(){
		this.move = false;
	}

	public void enableCollision(float after_seconds = 0.0f){
		StartCoroutine(enableCollisionHelper(after_seconds));
	}

	public void disableCollision(){
		this.transform.collider.enabled = false;
	}

	void OnTriggerEnter(Collider other){
		Instantiate(detonator, transform.position, transform.rotation);
		Network.Destroy(this.gameObject);
	}

	private IEnumerator enableCollisionHelper(float seconds){
		yield return new WaitForSeconds(seconds);
		this.transform.collider.enabled = true;
	}
}
