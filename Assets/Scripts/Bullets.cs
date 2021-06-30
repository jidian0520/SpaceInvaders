using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullets : MonoBehaviour
{
    public float speed = 30;

    private Rigidbody2D _rigidBody;

    public Sprite explodedAlienImage;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.velocity = Vector2.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (col.tag == "Alien")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDies);

            IncreaseTextUIScore();

            col.GetComponent<SpriteRenderer>().sprite = explodedAlienImage;

            Destroy(gameObject);

            //original code is "DestroyObject" but it's obsolete
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

    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int _score = int.Parse(textUIComp.text);

        _score += 10;

        textUIComp.text = _score.ToString();
    }
}
