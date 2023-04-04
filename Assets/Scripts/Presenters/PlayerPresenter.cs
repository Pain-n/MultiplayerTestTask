using Photon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MonoBehaviour = Photon.MonoBehaviour;

public class PlayerPresenter : MonoBehaviour
{
    public PhotonView PView;
    public PlayerModel Hero;
    public Rigidbody2D ContainerRB;
    public Rigidbody2D RB;
    public Slider HPSlider;

    public static event UnityAction<PhotonView, int> UpdateCoinUI;
    void Awake()
    {
        if (!PView.isMine)
        {
            RB.isKinematic = true;
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

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.position = ContainerRB.position;
        if (move != Vector3.zero)
        {
            ContainerRB.velocity = move * 3;
            

            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, move);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3f);
            RB.MoveRotation(rotation);
        }
    }

    public void Shoot()
    {
        BulletPresenter bullet = PhotonNetwork.Instantiate("Prefabs/Bullet", transform.position,Quaternion.identity,0).GetComponent<BulletPresenter>();
        bullet.Creator = this;
        bullet.RB.velocity = new Vector2(transform.up.x, transform.up.y).normalized * 50;
    }

    public void GetDamage(int value)
    {
        Hero.HP -= value;
        HPSlider.value = Hero.HP;

        if(Hero.HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin" && PView.isMine)
        {
            Hero.GoldCollected++;
            UpdateCoinUI?.Invoke(PView,Hero.GoldCollected);
            Destroy(collision.gameObject);
        }
    }
}
