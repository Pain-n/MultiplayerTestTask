using Unity.VisualScripting;
using UnityEngine;

public class BulletPresenter : MonoBehaviour
{
    public int Damage;
    public Rigidbody2D RB;
    public Collider2D Collider;
    public PhotonPlayer Owner;
    public PhotonView PView;

    public void SetUp(PhotonPlayer owner)
    {
        Owner = owner;
        Collider.enabled = true;
    }

    [PunRPC]
    void SendDamage(PlayerPresenter player)
    {
        player.PView.RPC("GetDamage", PhotonTargets.All, Damage);
    }

    [PunRPC]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Field")
        {
            if (PView.isMine)
            {
                PhotonNetwork.Destroy(PView);
            }
            else
            {
                PView.TransferOwnership(PView.owner);
                PhotonNetwork.Destroy(PView);
            }
        }
        else if (collision.tag == "Player" && collision.gameObject.GetComponent<PhotonView>().owner != Owner)
        {
            SendDamage(collision.gameObject.GetComponent<PlayerPresenter>());

            if (PView.isMine)
            {
                PhotonNetwork.Destroy(PView);
            }
            else
            {
                PView.TransferOwnership(PView.owner);
                PhotonNetwork.Destroy(PView);
            }
        }

    }
}
