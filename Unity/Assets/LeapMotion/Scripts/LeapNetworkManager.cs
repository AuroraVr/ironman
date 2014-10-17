using UnityEngine;
using System.Collections;

public class LeapNetworkManager : MonoBehaviour {
	
	private const string typeName = "MyIronManGameIBM123";
	private const string gameName = "Room1";
	private HostData[] hostList;
	public string connectionIP = "127.0.0.1";
	public GameObject handctrl;
	public GameObject ovrcamera;
	public GameObject p1;
	public GameObject p2;
	
	private void StartServer()
	{
		//(NumberOfPlayers, portNumber
		Network.InitializeServer(4, 25000, true);
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
		Network.Instantiate(handctrl, p1.transform.position, p1.transform.rotation, 0);
		GameObject ovr_temp = (GameObject) Instantiate(ovrcamera, p1.transform.position, p1.transform.rotation);
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log ("Player Connected");
	}
	
	//CLIENT SIDE
	void OnConnectedToServer()
	{
		Debug.Log ("User Connected to server");
		Network.Instantiate(handctrl, p2.transform.position, p2.transform.rotation, 0);
		GameObject ovr_temp = (GameObject) Instantiate(ovrcamera, p2.transform.position, p2.transform.rotation);
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
			if (GUI.Button(new Rect(100, 100, 400, 200), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 300, 400, 200), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(500, 500 + (110 * i), 400, 200), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}
}
