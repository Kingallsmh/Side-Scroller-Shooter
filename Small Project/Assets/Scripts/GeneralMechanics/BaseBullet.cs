using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    Rigidbody2D rb;
    public float speed = 400;
    public int dmg = 1;

    public BulletType type = BulletType.Standard;

    public enum BulletType
    {
        Standard, Penetrate, Bounce, Chain, Charge, Stick
    }

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate() {
        rb.velocity = new Vector2(transform.localScale.x, 0) * speed * Time.deltaTime;
    }

    public void ChangeTag(string newTag)
    {
        gameObject.tag = newTag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
