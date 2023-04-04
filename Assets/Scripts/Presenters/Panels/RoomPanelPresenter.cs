using Photon;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanelPresenter : PunBehaviour
{
    public TextMeshProUGUI PlayersInRoomText;

    private void Awake()
    {
        GlobalContext.PlayersInRoomUpdate += UpdatePlayersInRoomText;
    }

    private void Start()
    {
        UpdatePlayersInRoomText();
    }
    public void UpdatePlayersInRoomText()
    {
        PlayersInRoomText.text = "Players: \n";
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            PlayersInRoomText.text += photonPlayer + "\n";
        }
        if(PhotonNetwork.playerList.Length >= 2) PhotonNetwork.LoadLevel(1);
    }

    private void OnDestroy()
    {
        GlobalContext.PlayersInRoomUpdate -= UpdatePlayersInRoomText;
    }
}
