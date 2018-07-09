using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCatcherScript : MonoBehaviour {

    public CatcherType type = CatcherType.Bullet;

    public enum CatcherType
    {
        Enemy, Bullet
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(type == CatcherType.Enemy)
        {
            Destroy(collision.GetComponent<Collider2D>().attachedRigidbody.gameObject);
        }
        else if(type == CatcherType.Bullet)
        {
            if (collision.GetComponent<BaseBullet>())
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
