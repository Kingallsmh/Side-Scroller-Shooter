using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    Camera myCam;
    public Transform target;
    public float speed = 0.1f;

    // Use this for initialization
    void Start() {
        myCam = GetComponent<Camera>();
        myCam.orthographicSize = (Screen.height / 10f) / 4f;
    }

    // Update is called once per frame
    void Update() {

    }

    private void LateUpdate() {
        
        if (target) {
            //transform.position = Vector3.Lerp(transform.position, target.position, speed) + new Vector3(0, 0, -10f);
            transform.position = new Vector3(target.position.x, target.position.y + 0.5f, -10);
        }
    }
}
