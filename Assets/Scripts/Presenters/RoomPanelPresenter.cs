using Photon;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomPanelPresenter : MonoBehaviour
{
    public TextMeshProUGUI PlayersInRoomText;
    public Button StartGameButton;

    private void Start()
    {
        GlobalContext.PlayersInRoomUpdate += UpdatePlayersInRoomText;

        UpdatePlayersInRoomText();

        StartGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
        StartGameButton.onClick.AddListener(() =>
        {
            for(int i = 0; i< GlobalContext.Instance.Canvas.transform.childCount; i++)
            {
                Destroy(GlobalContext.Instance.Canvas.transform.GetChild(i).gameObject);
            }
            PhotonNetwork.LoadLevel(1);
        });
    }
    public void UpdatePlayersInRoomText()
    {
        PlayersInRoomText.text = "";
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            PlayersInRoomText.text += photonPlayer.NickName + "\n";
        }
    }

    private void OnDestroy()
    {
        GlobalContext.PlayersInRoomUpdate -= UpdatePlayersInRoomText;
    }
}
