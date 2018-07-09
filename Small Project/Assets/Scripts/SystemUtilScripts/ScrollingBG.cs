using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour {

    public float speed = 1;
    public GameObject BGObject;
    GameObject nextObject;

    Rigidbody2D rbObj1, rbObj2;
    float xWidth;

    // Use this for initialization
    void Start () {
        xWidth = BGObject.GetComponent<SpriteRenderer>().bounds.extents.x * 2;
        nextObject = Instantiate(BGObject,transform);
        nextObject.transform.localPosition = new Vector3(xWidth, 0, 0);

        rbObj1 = BGObject.GetComponent<Rigidbody2D>();
        rbObj2 = nextObject.GetComponent<Rigidbody2D>();

        rbObj1.velocity = new Vector3(-speed * Time.deltaTime * 10, 0, 0);
        rbObj2.velocity = new Vector3(-speed * Time.deltaTime * 10, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        PutToFront(BGObject, nextObject);
    }

    void PutToFront(GameObject obj1, GameObject obj2)
    {
        if(obj1.transform.position.x < -xWidth)
        {
            obj1.transform.position = new Vector3(obj2.transform.position.x + xWidth, 0, 0);
        }
        if (obj2.transform.position.x < -xWidth)
        {
            obj2.transform.position = new Vector3(obj1.transform.position.x + xWidth, 0, 0);
        }
    }
}
