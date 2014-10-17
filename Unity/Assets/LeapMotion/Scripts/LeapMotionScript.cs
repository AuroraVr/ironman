using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class LeapMotionScript : MonoBehaviour {


	public Transform fireball;
	Transform fireball_instance;


	Controller m_leapController;

	
	Frame f = null;
	Frame previousFrame = null;
	Hand hand = null;
	Vector position = null;
	bool wasClosed = false;
	bool wasClosed2 = false;
	public GameObject right_hand;
	public GameObject left_hand;

	void Start () {
		m_leapController = new Controller();
		m_leapController.SetPolicyFlags(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
	}
	
	bool Pinching(Hand h) {
		if (h == null) return false;
		return h.PinchStrength > 0.7f;
	}
	
	bool Grabbing(Hand h) {
		if (h == null) return false;
		return h.GrabStrength > 0.45f;
	}
	
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			f = m_leapController.Frame();
			//Transform camera = GameObject.Find("Camera").transform;
			//HideInactiveHands(f.Hands);
			if ((f.Hands.Count > 0)&&(f.Hands[0].IsRight)){
				hand = f.Hands[0];
				position = hand.PalmPosition;
				
				if(previousFrame!=null){
					Vector rotationAxis = f.RotationAxis(previousFrame);
					int multiplier;
					
					
					if(getHandClosed(hand)){
						//Information about the hand that is closed
						GameObject handObj = GameObject.Find("RigidHand(Clone)");
						Vector3 hand_position = handObj.transform.Find("palm").position;
						Quaternion hand_rotation = handObj.transform.Find("palm").rotation;
						fireball_instance = (Transform) Network.Instantiate(fireball, hand_position, hand_rotation, 0);
						//fireball_instance.parent = transform;
						
					}
					if( (fireball_instance!=null) && (isClosed(hand)) ){
						Vector3 hand_position = GameObject.Find("RigidHand(Clone)").transform.Find("palm").position;
						fireball_instance.position = hand_position;
					}
					if(getHandOpen(hand)){
						//Debug.Log("ABRIU");
						Vector3 hand_position = GameObject.Find("RigidHand(Clone)").transform.Find("palm").position;
						if(fireball_instance==null)
							return;
						else{
							fireball_instance.position = hand_position;
							fireball_instance.GetComponent<FireBallScript>().start_moving();
						}
						//Vector3 hand_position = new Vector3(0,0,0);
						//fireball.GetComponent<move>().enabled = false;
						//Instantiate(fireball, hand_position, Quaternion.identity);
						
					}
					
					
					
				}else{
					previousFrame = f;
				}
			}else{
				previousFrame = null;
			}
		}
	}
	void HideInactiveHands(HandList hands){

		right_hand.SetActive(true);
		left_hand.SetActive(true);
		foreach(Hand hand in hands){
			if(hand.IsRight)
				right_hand.SetActive(false);
			if(hand.IsLeft)
				left_hand.SetActive(false);
		}

	}

	void OnGUI()
	{
		GUI.Button(new Rect(100, 100, 250, 100), " "+networkView.isMine);
	}
	
	bool isClosed(Hand h){
		if(h==null) return false;
		
		if(h.GrabStrength > 0.9)
			return true;
		else
			return false;
	}
	
	public bool getHandClosed(Hand h){
		//If it just got opened
		if(wasClosed && !isClosed(h)){
			wasClosed=false;
		}

		//If it just got closed
		if(!wasClosed && isClosed(h)){
			//Debug.Log("true");
			wasClosed = true;
			return true;
		}else{
			return false;
		}
		
	}

	public bool getHandOpen(Hand h){
		//If it just got closed
		if(!wasClosed2 && isClosed(h)){
			wasClosed2=true;
		}

		//If it just got opened
		if(wasClosed2 && !isClosed(h)){
			wasClosed2 = false;
			return true;
		}else{
			return false;
		}
	}
}
