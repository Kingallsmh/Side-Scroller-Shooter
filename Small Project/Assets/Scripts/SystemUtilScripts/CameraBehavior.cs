using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {
    public static CameraBehavior Instance;
    Camera myCam;
    public Transform target;
    public float speed = 0.1f;
    public Vector2 ScreenSize;

    // Use this for initialization
    void Start() {
        Instance = this;
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
