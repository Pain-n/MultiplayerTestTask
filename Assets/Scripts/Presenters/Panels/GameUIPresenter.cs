using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIPresenter : MonoBehaviour
{
    public TextMeshProUGUI CoinText;
    public Button ShootButton;

    private void Awake()
    {
        PlayerPresenter.UpdateCoinUI += UpdateCoin;
    }

    void Start()
    {
        PlayerPresenter playerPresenter = PhotonNetwork.Instantiate("Prefabs/PlayerContainer", Vector3.zero, Quaternion.identity, 0).GetComponentInChildren<PlayerPresenter>();

        if (playerPresenter.PView.isMine)
        {
            ShootButton.onClick.AddListener(() =>
            {
                playerPresenter.Shoot();
            });
        }
    }

    public void UpdateCoin(PhotonView photonView,int value)
    {
        if(photonView.isMine) CoinText.text = value.ToString();
    }

    private void OnDestroy()
    {
        PlayerPresenter.UpdateCoinUI -= UpdateCoin;
    }
}
