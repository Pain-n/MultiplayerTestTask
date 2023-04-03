using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class MainMenuPanelPresenter : PunBehaviour
{
    public TMP_InputField CreateLobbyInput;
    public Button CreateLobbyButton;

    public TMP_InputField FindLobbyInput;
    public Button FindLobbyButton;

    void Start()
    {
        GlobalContext.Instance.Canvas = GetComponentInParent<Canvas>();
        DontDestroyOnLoad(GlobalContext.Instance.Canvas);

        PhotonNetwork.ConnectUsingSettings("0.1");

        CreateLobbyButton.onClick.AddListener(() =>
        {
            CreateRoom();
        });
        FindLobbyButton.onClick.AddListener(() =>
        {
            FindRoom();
        });
    }

    public void CreateRoom()
    {
        if (CreateLobbyInput.text != "")
        {
            PhotonNetwork.CreateRoom(CreateLobbyInput.text);
        }
    }

    public void FindRoom()
    {
        if (FindLobbyInput.text != "")
        {
            PhotonNetwork.JoinRoom(FindLobbyInput.text);
        }
    }
}
