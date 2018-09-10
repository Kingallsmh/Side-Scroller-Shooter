using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour {

    EnemyController controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<EnemyController>();
        StartCoroutine(DecisionLoop());
	}

    IEnumerator DecisionLoop()
    {
        while (this)
        {
            yield return StartCoroutine(RandomWalk());
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RandomWalk()
    {
        int rnd = Random.Range(-1, 2);
        controller.SetInputDirection(0, new Vector2(rnd, 0));
        float time = Random.Range(0f, 2f);
        yield return new WaitForSeconds(time);
    }

    IEnumerator StopMovementForGivenTime(float time)
    {
        controller.SetInputDirection(0, Vector2.zero);
        yield return new WaitForSeconds(time);
    }
}
