using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRipperMain : MonoBehaviour {

    public Transform playerObj;

    public GameObject main;
    public GameObject head;
    public GameObject rClaw, lClaw;
    public List<Transform> chainList;

    public BossMode mode = BossMode.Idle;
    public bool MakesOwnDecision = true;
    bool pauseIdle = false;
    public float ClawAttackSpeed = 300;

    public enum BossMode {
        Idle = 0, Roar = 1, ClawAtk = 2
    }

    List<WeightedDecision> decisionsList;

	// Use this for initialization
	void Start () {
        InitDecisions();
        StartCoroutine(AssessSituation());
        StartCoroutine(StartHeadIdle(0.2f));

        //Testing
        StartCoroutine(SnakeMovement(lClaw.GetComponent<Rigidbody2D>(), new Vector3(1, 1, 0), 1));
        StartCoroutine(SnakeMovement(rClaw.GetComponent<Rigidbody2D>(), new Vector3(-1, 1, 0), 1));
    }

    void InitDecisions() {
        if (decisionsList == null) {
            decisionsList = new List<WeightedDecision>();
        }
        decisionsList.Add(new WeightedDecision(0, 65));
        decisionsList.Add(new WeightedDecision(1, 35));
        decisionsList.Add(new WeightedDecision(3, 20));
    }

    IEnumerator AssessSituation() {
        while (true) {
            switch (mode) {
                case BossMode.Idle:
                    yield return new WaitForSeconds(4);
                    break;
                case BossMode.Roar:
                    yield return StartCoroutine(RoarAbility());
                    break;
                case BossMode.ClawAtk:
                    yield return StartCoroutine(ClawPlayerAbility(playerObj));
                    break;
            }
            if (MakesOwnDecision) {
                mode = PickRandomNextMode();
            }
            yield return null;
        }
    }

    IEnumerator StartHeadIdle(float delay) {
        StartCoroutine(SnakeMovement(head.GetComponent<Rigidbody2D>(), new Vector3(0, 2, 0), 1));
        yield return new WaitForSeconds(delay);
        for(int i = 0; i < chainList.Count; i++) {
            StartCoroutine(SnakeMovement(chainList[i].GetComponent<Rigidbody2D>(), new Vector3(0, 2, 0), 1));
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator SnakeMovement(Rigidbody2D rb, Vector3 direction, float time, float speed = 10) {
        float currentSpeed = 10;
        float currentTime = time/2;
        while (true) {
            while (currentTime < time) {
                rb.velocity = direction * Time.deltaTime * speed * (currentSpeed - (currentSpeed * (currentTime/time)));
                currentTime += Time.deltaTime;
                yield return null;
                if(mode != BossMode.Idle) {
                    rb.velocity = Vector3.zero;
                    yield return new WaitWhile(() => mode != BossMode.Idle);
                }
            }
            currentSpeed = -currentSpeed;
            currentTime = 0;
        }        
    }

    IEnumerator RoarAbility() {
        yield return new WaitForSeconds(0.5f);
        head.GetComponent<Animator>().SetTrigger("Roar");
        yield return new WaitForSeconds(2);
        mode = BossMode.Idle;
    }

    IEnumerator ClawPlayerAbility(Transform target) {
        int clawNum = Random.Range(0, 2); //More claws = higher max
        Animator clawAnim = null;
        Rigidbody2D clawRB = null;
        if (clawNum == 0) {
            clawAnim = rClaw.GetComponent<Animator>();
            clawRB = rClaw.GetComponent<Rigidbody2D>();       
        }
        if(clawNum == 1) {
            clawAnim = lClaw.GetComponent<Animator>();
            clawRB = lClaw.GetComponent<Rigidbody2D>();
        }
        clawAnim.SetTrigger("OpenClaw");
        yield return new WaitForSeconds(clawAnim.GetCurrentAnimatorClipInfo(0).Length + 0.2f);
        Vector3 oldPos = clawRB.position;
        yield return StartCoroutine(MoveToPos(clawRB, target.position, ClawAttackSpeed));
        clawAnim.SetTrigger("SlashClaw");
        yield return new WaitForSeconds(clawAnim.GetCurrentAnimatorClipInfo(0).Length + 0.2f);
        yield return StartCoroutine(MoveToPos(clawRB, oldPos, ClawAttackSpeed));
        yield return new WaitForSeconds(1);
    }

    IEnumerator MoveToPos(Rigidbody2D rb, Vector2 pos, float speed, float withinDistance) {
        Vector3 direction = (pos - rb.position).normalized;
        while (true) {
            rb.velocity = direction * Time.deltaTime * speed;
            if((pos - rb.position).magnitude < withinDistance) {
                rb.velocity = Vector3.zero;
                break;
            }
            yield return null;
        }
    }

    IEnumerator MoveToPos(Rigidbody2D rb, Vector2 pos, float speed) {
        Vector3 direction = (pos - rb.position).normalized;
        while (true) {
            rb.velocity = direction * Time.deltaTime * speed;
            if ((pos - rb.position).magnitude < 0.4f) {
                rb.velocity = Vector3.zero;
                rb.position = pos;
                break;
            }
            yield return null;
        }
    }

    BossMode PickRandomNextMode() {
        int dec = WeightedDecision.MakeDecision(decisionsList);
        return (BossMode)dec;
    }

    void ChangeMode(BossMode _mode) {
        mode = _mode;
    }
}
