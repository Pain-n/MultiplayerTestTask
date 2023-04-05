using TMPro;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanelPresenter : MonoBehaviour
{
    public TextMeshProUGUI CoinsText;
    public Button ToLobbyButton;

    private void Start()
    {
        ToLobbyButton.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        });
    }
}
