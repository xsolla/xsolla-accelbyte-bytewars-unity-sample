using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class GameDirector : NetworkBehaviour
{

    public static GameDirector Instance { get; private set; }

    public NetworkManager NetManager;

    public TMP_Text ConsoleText;

    public NetworkVariable<Vector3> Player1Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> Player2Position = new NetworkVariable<Vector3>();

    public int MaxAllowedPlayers = 2;

    public NetworkVariable<int> RandomMasterSeed;

    public E_GameMode GameMode = E_GameMode.MAIN_MENU;

    private int _totalPlayersConnected = 0;
    // Start is called before the first frame update
    [FormerlySerializedAs("_sceneName")] [SerializeField]
    private string _gameSceneName = "GalaxyWorld1";

    public GameObject PlayerPref;

    private Scene m_LoadedScene;

    public enum E_GameMode {MAIN_MENU, SINGLE_PLAYER, MULTI_PLAYER, LOCAL_MULTI_PLAYER};

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public override void OnNetworkSpawn()
    {
        if (!IsServer) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            WriteToConsole("OnNetworkSpawn as a Client!");
            //TestServerRpc(0);
        }else
        {
            WriteToConsole("OnNetworkSpawn as a Server!");
            //TestClientRpc(3);
        }

        NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;

        NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

        WriteToConsole("Transport mode: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);

        base.OnNetworkSpawn();
    }

    public void StartSinglePlayer()
    {
        GameMode = E_GameMode.SINGLE_PLAYER;
        WriteToConsole("Starting SinglePlayer Mode");
        SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
    }

    public void StartLocalMultiplayer()
    {
        // Debug.Log($"Start local multiplayer with {totalPlayer} players");
        GameMode = E_GameMode.LOCAL_MULTI_PLAYER;
        SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
    }
    public void StartServer()
    {
        GameMode = E_GameMode.MULTI_PLAYER;
        WriteToConsole("Starting as a Server...");
        NetManager.StartServer();
        RandomMasterSeed.Value = Random.Range(0, 1000);
        WriteToConsole("Server Random MasterSeed number: " + RandomMasterSeed.Value);

    }

    public void StartClient()
    {
        GameMode = E_GameMode.MULTI_PLAYER;
        WriteToConsole("Starting as a Client...");
        NetManager.StartClient();
    }

    [ClientRpc]
    void TestClientRpc(int value)
    {
        WriteToConsole("TestClientRPC with InValue: " + value.ToString());
    }

    [ServerRpc(RequireOwnership = false)]
    void TestServerRpc(int value)
    {
        WriteToConsole("TestServerRPC with InValue: " + value.ToString());
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        _totalPlayersConnected += 1;
        if (NetworkManager.Singleton.IsServer)
        {
            WriteToConsole("New Client connected- ClientID: " + clientId + " total Connected Clients: " + _totalPlayersConnected.ToString());

            if(_totalPlayersConnected == MaxAllowedPlayers)
            {
                WriteToConsole("Max players reached: " + MaxAllowedPlayers.ToString() + " Sending StartGameRPC to clients... ");

                var status = NetworkManager.SceneManager.LoadScene(_gameSceneName,UnityEngine.SceneManagement.LoadSceneMode.Single);
                if (status != SceneEventProgressStatus.Started)
                {
                    Debug.LogWarning($"Failed to load {_gameSceneName} " +
                          $"with a {nameof(SceneEventProgressStatus)}: {status}");
                }
                //StartGameClientRpc();
            }
        }
    }

    [ClientRpc]
    void StartGameClientRpc()
    {
        WriteToConsole("Starting Game!");
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        _totalPlayersConnected -= 1;
        if (NetworkManager.Singleton.IsServer)
        {
            WriteToConsole("Client Disconnected - ClientID: " + clientId + " total Connected Clients: " + _totalPlayersConnected.ToString());
        }
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "server" : "client";
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    // We want to handle this for only the server-side
                    if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                    {
                        // *** IMPORTANT ***
                        // Keep track of the loaded scene, you need this to unload it
                        m_LoadedScene = sceneEvent.Scene;
                        

                    }
                    WriteToConsole(clientOrServer +" with ID: "+ sceneEvent.ClientId + " LoadSceneCompleted :" + m_LoadedScene.name);
                    break;
                }
            case SceneEventType.UnloadComplete:
                {
                    WriteToConsole(clientOrServer + " with ID: " + sceneEvent.ClientId + " UnloadSceneCompleted :" + m_LoadedScene.name);
                    break;
                }
            case SceneEventType.LoadEventCompleted:
            case SceneEventType.UnloadEventCompleted:
                {
                    var loadUnload = sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted ? "Load" : "Unload";
                    WriteToConsole($"{loadUnload} event completed... Starting Game");

                   // if (NetworkManager.Singleton.IsServer)
                   // {
                   //     GameObject.Find("GameMode").GetComponent<InGameGameMode>().ServerStartGame();
                  //  }

                    if (sceneEvent.ClientsThatTimedOut.Count > 0)
                    {
                        WriteToConsole($"{loadUnload} event timed out for the following client " +
                            $"identifiers:({sceneEvent.ClientsThatTimedOut})");
                    }
                    break;
                }
        }
    }

    public void WriteToConsole(string text)
    {
        ConsoleText.text += "\n" + text;
    }

    public override void OnDestroy()
    {
        if (NetworkManager == null)
        {
            base.OnDestroy();  return;
        }

        NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
