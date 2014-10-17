using UnityEngine;
using System.Collections;

public class NetManager : MonoBehaviour {
	string registeredGameName = "IronManOculusIBM";
	string gameName = "IronManIBM";
	bool isRefreshing = false;
	float refreshRequestLength = 1.0f;
	HostData[] hostData;

	public GameObject player1spawn;
	public GameObject player2spawn;
	public GameObject head;
	public GameObject body;
	public GameObject handController;
	public GameObject ovrCamera;
	public GameObject wall;

	public void StartServer(){
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(registeredGameName, gameName);
	}

	public IEnumerator refreshHostList(){
		Debug.Log("Refreshing...");
		MasterServer.RequestHostList(registeredGameName);
		float timeStarted = Time.time;
		float timeEnd = Time.time + refreshRequestLength;

		while(Time.time < timeEnd){
			hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}

		if(hostData == null || hostData.Length == 0){
			Debug.Log("No active server have been found");
		}

	}

	//Server site
	void OnServerInitialized(){
		//Initializing Server player
		Debug.Log("Server Initialized");
		GameObject head_temp = (GameObject) Network.Instantiate(head, player1spawn.transform.position, player1spawn.transform.rotation, 0);
		GameObject ovr_temp = (GameObject) Instantiate(ovrCamera, player1spawn.transform.position, player1spawn.transform.rotation);
		GameObject body_temp = (GameObject) Network.Instantiate(body, player1spawn.transform.position, player1spawn.transform.rotation,0);
		GameObject handctrl = (GameObject) Network.Instantiate(handController, player1spawn.transform.position, player1spawn.transform.rotation, 0);


		//configuring head
		head_temp.GetComponent<HeadScript>().toFollow = GameObject.Find("CameraLeft");
		head_temp.transform.parent = player1spawn.transform;

		//configuring body
		body_temp.GetComponent<RobotWarrior>().head = GameObject.Find("CameraLeft").transform;
		body_temp.GetComponent<RobotWarrior>().xOffset = 0.0f;
		body_temp.GetComponent<RobotWarrior>().yOffset = -4.5f;
		body_temp.GetComponent<RobotWarrior>().zOffset = -0.2f;
		body_temp.transform.parent = player1spawn.transform;

		//configuring hand controller
		handctrl.GetComponent<LeapMotionScript>().left_hand = GameObject.Find("TerminatorLeft");
		handctrl.GetComponent<LeapMotionScript>().right_hand = GameObject.Find("TerminatorRight");
		handctrl.GetComponent<HandScript>().toFollow = GameObject.Find("CameraLeft");
		handctrl.transform.parent = GameObject.Find("CameraLeft").transform;
		handctrl.transform.localEulerAngles = new Vector3(270f,180f,0f);
		handctrl.transform.localPosition = new Vector3(0f, -0.2f, 0f);

	}

	//Client side
	void OnConnectedToServer(){
		//Initializing Server player
		Debug.Log("Server Initialized");
		GameObject head_temp = (GameObject) Network.Instantiate(head, player2spawn.transform.position, player2spawn.transform.rotation, 0);
		GameObject ovr_temp = (GameObject) Instantiate(ovrCamera, player2spawn.transform.position, player2spawn.transform.rotation);
		GameObject body_temp = (GameObject) Network.Instantiate(body, player2spawn.transform.position, player2spawn.transform.rotation,0);
		GameObject handctrl = (GameObject) Network.Instantiate(handController, player2spawn.transform.position, player2spawn.transform.rotation,0);

		
		//configuring head
		head_temp.GetComponent<HeadScript>().toFollow = GameObject.Find("CameraLeft");
		head_temp.transform.parent = player2spawn.transform;
		
		//configuring body
		body_temp.GetComponent<RobotWarrior>().head = GameObject.Find("CameraLeft").transform;
		body_temp.GetComponent<RobotWarrior>().xOffset = 0.0f;
		body_temp.GetComponent<RobotWarrior>().yOffset = -4.5f;
		body_temp.GetComponent<RobotWarrior>().zOffset = 0f;
		body_temp.transform.parent = player2spawn.transform;
		
		//configuring hand controller
		handctrl.GetComponent<LeapMotionScript>().left_hand = GameObject.Find("TerminatorLeft");
		handctrl.GetComponent<LeapMotionScript>().right_hand = GameObject.Find("TerminatorRight");
		handctrl.GetComponent<HandScript>().toFollow = GameObject.Find("CameraLeft");
		handctrl.transform.parent = GameObject.Find("CameraLeft").transform;
		handctrl.transform.localEulerAngles = new Vector3(270f,180f,0f);
		handctrl.transform.localPosition = new Vector3(0f, -0.2f, 0f);
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent){
		if(masterServerEvent == MasterServerEvent.RegistrationSucceeded){
			Debug.Log ("Registrattion succeeded");
		}
	}
	public void OnGUI(){

		if(Network.isClient || Network.isServer)
			return;

		if(GUI.Button(new Rect(Screen.width/2 - 200, Screen.height/2 - 200, 300f, 100f), "start server")){
			StartServer();
		}
		if(GUI.Button(new Rect(Screen.width/2 - 200, Screen.height/2 - 200 + 100, 300f, 100f), "Refresh server")){
			StartCoroutine("refreshHostList");
		}

		if(hostData != null){
			for (int i=0; i<hostData.Length; i++){
				if(GUI.Button(new Rect(Screen.width/2 - 200, Screen.height/2 - 200 + 200, 300f, 100f), hostData[i].gameName)){
					Network.Connect(hostData[i]);
				}
			}
		}

	}
}
