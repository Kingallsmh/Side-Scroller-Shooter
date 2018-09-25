using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour {

    EnemyController controller;
    MonsterScript monster;    

    public GameObject target;

	// Use this for initialization
	void Start () {
        monster = GetComponent<MonsterScript>();
        controller = GetComponent<EnemyController>();
        StartCoroutine(DecisionLoop());
	}

    IEnumerator DecisionLoop()
    {
        while (this)
        {
            if(monster.moveType != PhysicsObject.MoveType.Float)
            {
                yield return StartCoroutine(RandomWalk());
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return StartCoroutine(RandomFloat());
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator RandomFloat()
    {
        int rndX = Random.Range(-1, 2);
        int rndY = Random.Range(-1, 2);
        controller.SetInputDirection(0, new Vector2(rndX, rndY));
        float time = Random.Range(0f, 2f);
        yield return new WaitForSeconds(time);
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

    IEnumerator HeadToTarget() {
        if(target != null) {
            Vector2 move = target.transform.position - monster.transform.position;
        }
        yield return null;
    }
}
