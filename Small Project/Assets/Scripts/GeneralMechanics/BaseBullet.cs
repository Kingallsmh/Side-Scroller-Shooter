using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

    Rigidbody2D rb;
    public float speed = 400;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate() {
        rb.velocity = new Vector2(1, 0) * speed * Time.deltaTime;
    }
}
