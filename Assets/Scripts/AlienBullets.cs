using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullets : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public float speed = 30;

    public Sprite explodedShipImage;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.velocity = Vector2.down * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);

            col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;

            Destroy(gameObject);

            Object.Destroy(col.gameObject, 0.5f);
        }

        if (col.tag == "Shield")
        {
            Destroy(gameObject);
            Object.Destroy(col.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
