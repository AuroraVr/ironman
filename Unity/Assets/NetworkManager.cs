using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "HereIsMyUniqueGameIronMan";
	private const string gameName = "Room1";
	private HostData[] hostList;
	public GameObject head;
	public GameObject body;
	public GameObject ovrCamera;
	public GameObject handController;
	//public GameObject playerPrefab;
	public GameObject player1Spawn;
	public GameObject player2Spawn;
	private int playerCount = 0;
	private GameObject PlayerServer;
	private GameObject PlayerClient;

	private GameObject head_test;

	private void StartServer()
	{
		//(NumberOfPlayers, portNumber
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	//SERVER SIDE
	void OnServerInitialized()
	{
		//create temporary ovcamera
		GameObject temp_ovr = (GameObject) Instantiate(ovrCamera, player1Spawn.transform.position, player1Spawn.transform.rotation);
		GameObject temp_handctrl = (GameObject) Instantiate(handController, player1Spawn.transform.position, player1Spawn.transform.rotation);
		head_test = (GameObject) Network.Instantiate(head, player1Spawn.transform.position, player1Spawn.transform.rotation, 0);
		//GameObject temp_body = (GameObject) Network.Instantiate(body, player1Spawn.transform.position, player1Spawn.transform.rotation, 0);

		//temp_ovr.transform.parent = player1Spawn.transform;
		//temp_head.transform.parent = GameObject.Find("CameraLeft").transform;
		//temp_handctrl.transform.parent = GameObject.Find("CameraLeft").transform;
		//temp_body.transform.parent = player1Spawn.transform;

//		temp_handctrl.GetComponent<LeapMotionScript>().right_hand = GameObject.Find("TerminatorRight");
//		temp_handctrl.GetComponent<LeapMotionScript>().left_hand = GameObject.Find("TerminatorLeft");
//		temp_handctrl.transform.localEulerAngles = new Vector3(270.0f, 180.0f, 0);
//		temp_handctrl.transform.localPosition = new Vector3(0.0f, -0.3f, 0.2f);

//		temp_body.GetComponent<RobotWarrior>().yOffset = -4.5f;
//		temp_body.GetComponent<RobotWarrior>().zOffset = -0.2f;
//		temp_body.GetComponent<RobotWarrior>().head = temp_head.transform;


//		//Configuring player on Server
//		PlayerServer = (GameObject) Network.Instantiate(playerPrefab, player1Spawn.transform.position, player1Spawn.transform.rotation, 0);
//		PlayerServer.transform.eulerAngles = new Vector3(270, 180, 0);
//		PlayerServer.transform.name += "Server";
		//Configuring camera on Server
		//GameObject.Find("Camera").transform.position = player1Spawn.transform.position;
		//GameObject.Find("Camera").transform.eulerAngles = new Vector3(0, 0, 0);
		//PlayerServer.transform.parent = GameObject.Find("Camera").transform;
	}

	void OnPlayerConnected(NetworkPlayer player) {
		//GameObject.Find("Player1").transform;
		//Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
	}

	//CLIENT SIDE
	void OnConnectedToServer()
	{
		//Configuring player on Client
//		PlayerClient = (GameObject) Network.Instantiate(playerPrefab, player2Spawn.transform.position, player2Spawn.transform.rotation, 0);
//		PlayerClient.transform.eulerAngles = new Vector3(270, 0, 0);
//		PlayerClient.transform.name += "Client";
//
//		//Configuring camera on Server
//		GameObject.Find("Camera").transform.position = player2Spawn.transform.position;
//		GameObject.Find("Camera").transform.eulerAngles = new Vector3(0, 180, 0);
//		PlayerClient.transform.parent = GameObject.Find("Camera").transform;
	}

	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived){
			hostList = MasterServer.PollHostList();
			Debug.Log("Registration succesful");
		}
	}


	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
