using UnityEngine;

public class BulletPresenter : MonoBehaviour
{
    public int Damage;
    public Rigidbody2D RB;
    public PlayerPresenter Creator;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Player") Destroy(gameObject);
        else if(collision.collider.tag == "Player" && collision.collider.gameObject != Creator.gameObject)
        {
            collision.collider.GetComponent<PlayerPresenter>().GetDamage(Damage);
            Destroy(gameObject);
        }

    }
}
