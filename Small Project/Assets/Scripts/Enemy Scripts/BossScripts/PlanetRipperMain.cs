﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRipperMain : MonoBehaviour {

    public GameObject main;
    public GameObject head;
    public List<Transform> chainList;

    public BossMode mode = BossMode.Idle;

    public enum BossMode {
        Idle, Roar
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(StartHeadIdle(0.2f));

    }

    IEnumerator AssessSituation() {
        while (true) {
            switch (mode) {
                case BossMode.Idle:
                    break;
            }
        }
    }

    IEnumerator StartHeadIdle(float delay) {
        StartCoroutine(SnakeMovement(head.GetComponent<Rigidbody2D>(), new Vector3(0, 1, 0), 1));
        yield return new WaitForSeconds(delay);
        for(int i = 0; i < chainList.Count; i++) {
            StartCoroutine(SnakeMovement(chainList[i].GetComponent<Rigidbody2D>(), new Vector3(0, 1, 0), 1));
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator SnakeMovement(Rigidbody2D rb, Vector3 direction, float time, float speed = 10) {
        float currentSpeed = 10;
        float currentTime = 0;
        while (true) {
            while (currentTime < time) {
                rb.velocity = direction * Time.deltaTime * speed * (currentSpeed - (currentSpeed * (currentTime/time)));
                currentTime += Time.deltaTime;
                yield return null;
            }
            currentSpeed = -currentSpeed;
            currentTime = 0;
        }        
    }

    IEnumerator RoarAbility() {
        yield return new WaitForSeconds(0.5f);
        head.GetComponent<Animator>();
    }
}
