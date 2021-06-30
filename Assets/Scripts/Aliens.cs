using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aliens : MonoBehaviour
{
    public float speed = 10;

    public Rigidbody2D rigidBody;

    public Sprite startingImage;

    public Sprite altImage;

    public float secBeforeSpriteChange = 0.5f;

    public GameObject alienBullet;

    public float minFireRateTime = 1.0f;

    public float maxFireRateTime = 3.0f;

    public float baseFireRateTime = 3.0f;

    public Sprite explodedShipImage;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.velocity = new Vector2(1, 0) * speed;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ChangeAlienSprite());

        baseFireRateTime = baseFireRateTime + Random.Range(minFireRateTime, maxFireRateTime);
    }

    //Turn in opposite direction
    void Turn(int direction)
    {
        Vector2 newVelocity = rigidBody.velocity;
        newVelocity.x = speed * direction;
        rigidBody.velocity = newVelocity;
    }

    //Move down after hitting wall
    void MoveDown()
    {
        Vector2 position = transform.position;
        position.y -= 1;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "LeftWall")
        {
            Turn(1);
            MoveDown();
        }

        if (col.gameObject.name == "RightWall")
        {
            Turn(-1);
            MoveDown();
        }

        if (col.gameObject.tag == "Bullet")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDies);
            Destroy(gameObject);
        }
    }

    public IEnumerator ChangeAlienSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(secBeforeSpriteChange);

            if (_spriteRenderer.sprite == startingImage)
            {
                _spriteRenderer.sprite = altImage;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienBuzz1);
            }
            else
            {
                _spriteRenderer.sprite = startingImage;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienBuzz2);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time > baseFireRateTime)
        {
            baseFireRateTime = baseFireRateTime + Random.Range(minFireRateTime, maxFireRateTime);

            Instantiate(alienBullet, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);
            col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;
            Destroy(gameObject);
            Object.Destroy(col.gameObject, 0.5f);
        }
    }
}
