using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GlobalContext : PunBehaviour
{
    private static GlobalContext globalContext;
    public static GlobalContext Instance
    {
        get
        {
            if (!globalContext)
            {
                globalContext = new GameObject().AddComponent<GlobalContext>();
                globalContext.name = globalContext.GetType().ToString();
                DontDestroyOnLoad(globalContext.gameObject);
            }
            return globalContext;
        }
    }

    public Canvas Canvas;
    public static event UnityAction PlayersInRoomUpdate;
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.automaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined lobby");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("created room");
    }

    public void ClearCanvas(Scene prevScene, Scene newScene)
    {
        Canvas = FindObjectOfType<Canvas>();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        PlayersInRoomUpdate?.Invoke();
    }
    public override void OnJoinedRoom()
    {
        Instantiate(Resources.Load<RoomPanelPresenter>("Prefabs/Panels/RoomPanel"), Canvas.transform);
        PlayersInRoomUpdate?.Invoke();
        Debug.Log("joined room");
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ClearCanvas;
    }
}
