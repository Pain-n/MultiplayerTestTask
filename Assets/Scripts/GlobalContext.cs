using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

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
    public static event Action PlayersInRoomUpdate;

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined lobby");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("created room");
    }

    public override void OnJoinedRoom()
    {
        Instantiate(Resources.Load<RoomPanelPresenter>("Prefabs/RoomPanel"), Canvas.transform);
        PlayersInRoomUpdate?.Invoke();
        Debug.Log("joined room");
    }
}
