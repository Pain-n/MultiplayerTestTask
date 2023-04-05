using Photon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPresenter : PunBehaviour
{
    public PhotonView PView;
    public PlayerModel Hero;
    public Rigidbody2D ContainerRB;
    public Rigidbody2D RB;
    public SpriteRenderer SpriteRenderer;
    public Slider HPSlider;

    public JoystickPresenter jsMovement;
    public Vector3 direction;

    public static event UnityAction<PhotonView, int> UpdateCoinUI;
    void Awake()
    {
        if (!PView.isMine)
        {
            RB.mass = 9999;
        }
        Hero = new PlayerModel();
        Hero.HP = 100;
        Hero.DamageValue = 5;
        Hero.GoldCollected = 0;
        HPSlider.maxValue = 100;
        HPSlider.value = 100;
    }

    private void Update()
    {
        if (!PView.isMine) return;

        direction = jsMovement.InputDirection;

        transform.position = ContainerRB.position;
        if (direction.magnitude != 0)
        {
            ContainerRB.transform.position += direction * 0.01f;

            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, direction);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3f);
            RB.MoveRotation(rotation);
        }
    }

    [PunRPC]
    public void Shoot()
    {
        BulletPresenter bullet = PhotonNetwork.Instantiate("Prefabs/Bullet", transform.position,Quaternion.identity,0).GetComponent<BulletPresenter>();
        bullet.SetUp(PView.owner);
        bullet.RB.velocity = new Vector2(transform.up.x, transform.up.y).normalized * 50;
    }

    [PunRPC]
    public void GetDamage(int value)
    {
        Hero.HP -= value;
        HPSlider.value = Hero.HP;
        if (Hero.HP <= 0)
        {
            PhotonNetwork.automaticallySyncScene = false;

            if (!PView.isMine)
            {
                EndGame();
            }
            else
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(0);
            }
        }
    }

    [PunRPC]
    public void EndGame()
    {
        WinPanelPresenter winPanel = Instantiate(Resources.Load<WinPanelPresenter>("Prefabs/Panels/WinPanel"), GlobalContext.Instance.Canvas.transform);
        Debug.Log(Hero.GoldCollected);
        winPanel.CoinsText.text = "You collected " + Hero.GoldCollected + " coins!";
    }

    [PunRPC]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin" && PView.isMine)
        {
            Hero.GoldCollected++;

            UpdateCoinUI?.Invoke(PView,Hero.GoldCollected);
            if (collision.GetComponent<PhotonView>().isMine)
            {
                PhotonNetwork.Destroy(collision.GetComponent<PhotonView>());
            }
            else
            {
                collision.GetComponent<PhotonView>().TransferOwnership(PView.owner);
                PhotonNetwork.Destroy(collision.GetComponent<PhotonView>());
            }
        }
    }
}
