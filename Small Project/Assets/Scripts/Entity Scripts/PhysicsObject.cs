using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
    public MoveType moveType = MoveType.Pilot;
    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected bool unphysics;
    protected Vector2 groundNormal = new Vector2(0, 1);
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected ContactFilter2D stickyFilter;
    public LayerMask stickyLayerMask;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    public float maxYVelocity = 15f;

    public string debugString;

    public enum MoveType {
        Pilot, WallStick, Float
    }

    public void StartObject() {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        stickyFilter.SetLayerMask(stickyLayerMask);
        stickyFilter.useLayerMask = true;
        rb2d = GetComponent<Rigidbody2D>();
        FindBoxWidth();
    }

    void Update() {
        if (!unphysics) {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }
    }

    protected virtual void ComputeVelocity() {

    }

    private void FixedUpdate() {
        switch (moveType) {
            case MoveType.Pilot: PilotUpdate();
                break;
            case MoveType.WallStick: StickywallUpdate();
                break;
            case MoveType.Float: FloatMovementUpdate();
                break;
        }
    }

    bool hasReset = false;
    int frameCount = 0;
    void PilotUpdate() {
        if (unphysics) {
            velocity = Vector2.zero;
            return;
        }

        if (!hasReset) {
            if (FindNormalOfFloor() == Vector2.zero && targetVelocity.y <= 0) {
                velocity.y = 0;
                hasReset = true;
            }
        }
        else {
            if (FindNormalOfFloor() != Vector2.zero && targetVelocity.y <= 0) {
                hasReset = false;
            }
        }

        if (targetVelocity.y > 0 && !hasReset) {
            velocity.y = 0;
            hasReset = true;
        }

        velocity.y += targetVelocity.y;
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;
        velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -maxYVelocity, maxYVelocity));

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    Vector2 FindNormalOfFloor() {
        int count = rb2d.Cast(new Vector2(0, -1), contactFilter, hitBuffer, 0.5f + shellRadius);
        Debug.DrawRay(rb2d.position, new Vector2(0, -1), Color.red, 0.5f + shellRadius);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++) {
            hitBufferList.Add(hitBuffer[i]);
        }

        if (hitBufferList.Count == 1) {
            return hitBufferList[0].normal;
        }
        else {
            return Vector2.zero;
        }
    }

    public Vector2 moveDebug;
    bool groundedLastFrame = false;
    protected void Movement(Vector2 move, bool yMovement) {
        float distance = move.magnitude;
        if (distance > minMoveDistance) {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            //Debug.DrawRay(rb2d.position, move, Color.green, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) {
                    grounded = true;

                    if (yMovement) {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                moveDebug = velocity;
                debugString = "Projection" + projection;
                if (projection < 0 && velocity.y > 0) {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
            if (hitBufferList.Count == 0) {
                groundNormal = new Vector2(0, 1);
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
        if (grounded) {
            groundedLastFrame = true;
        }
        else {
            groundedLastFrame = false;
        }
    }

    Vector3 newVel;
    void StickywallUpdate() {
        if (unphysics) {
            newVel = Vector3.zero;
            return;
        }

        //Cast downwards from character
        RaycastHit2D currentHit = FindNormalOfFloorSticky();
        if (currentHit && targetVelocity.y == 0) {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, currentHit.normal);
            transform.rotation = Quaternion.Euler(0, 0, rot.eulerAngles.z);
            Vector2 yAdjust = transform.up * boxExtents.y;
            transform.position = currentHit.point;
            newVel.y = Mathf.Clamp(newVel.y, -1, 1);
            moveDebug = newVel;
        }
        else {
            if(transform.rotation != Quaternion.Euler(Vector3.zero)) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), 10);                
            }            
        }


        newVel.x = targetVelocity.x * Time.deltaTime * 100;
        newVel.y += -gravityModifier * 9.8f * Time.deltaTime;
        newVel.y += targetVelocity.y;
        newVel.x = Mathf.Clamp(newVel.x, -10, 10);
        newVel.y = Mathf.Clamp(newVel.y, -maxYVelocity, maxYVelocity);
        moveDebug = newVel;
        Vector2 y = transform.up * newVel.y;
        Vector2 x = transform.right * newVel.x;

        rb2d.velocity = y + x;
    }

    protected void Movement2(Vector2 move, bool yMovement) {
        float distance = move.magnitude;
        if (distance > minMoveDistance) {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            Debug.DrawRay(rb2d.position, move, Color.green, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) {
                Vector2 currentNormal = hitBufferList[i].normal;
                grounded = true;
                moveDebug = currentNormal;
                groundNormal = currentNormal;

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    Vector2 boxExtents;
    void FindBoxWidth() {
        Collider2D[] colList = new Collider2D[1];
        rb2d.GetAttachedColliders(colList);
        boxExtents = colList[0].bounds.extents;
    }

    RaycastHit2D FindNormalOfFloorSticky()
    {
        
        Vector3 pos = rb2d.position;
        Vector3 transformUp = transform.up;
        Vector3 transformRight = transform.right;
        float x = boxExtents.x + 0.2f;
        Ray leftRay = new Ray(pos + transformUp + x * transformRight, -transformUp);
        Ray centerRay = new Ray(pos + transformUp, -transformUp);
        Ray rightRay = new Ray(pos + transformUp - x * transformRight, -transformUp);
        Ray forwardRay = new Ray(pos + transformUp * (0.2f), transformRight * transform.localScale.x * x);

        Debug.DrawRay(leftRay.origin, leftRay.direction, Color.white);
        Debug.DrawRay(centerRay.origin, centerRay.direction, Color.blue);
        Debug.DrawRay(rightRay.origin, rightRay.direction, Color.white);
        Debug.DrawRay(forwardRay.origin, forwardRay.direction, Color.white);

        RaycastHit2D[] forwardHit = new RaycastHit2D[2];
        if(Physics2D.Raycast(forwardRay.origin, forwardRay.direction, stickyFilter, forwardHit, x) > 0) {            
            return forwardHit[0];
        }
        RaycastHit2D[] forwardHit2 = new RaycastHit2D[2];
        if (Physics2D.Raycast(leftRay.origin, leftRay.direction, stickyFilter, forwardHit, boxExtents.y+0.4f) > 0 && Physics2D.Raycast(rightRay.origin, rightRay.direction, stickyFilter, forwardHit2, boxExtents.y + 0.4f) > 0) {
            Vector2 avgNorm = (forwardHit[0].normal + forwardHit2[0].normal) / 2;
            Vector2 avgPos = (forwardHit[0].point + forwardHit2[0].point) / 2;
            RaycastHit2D newRay = new RaycastHit2D();
            newRay.normal = avgNorm;
            newRay.point = avgPos;
            return newRay;
        }
        if(Physics2D.Raycast(centerRay.origin, centerRay.direction, stickyFilter, forwardHit, boxExtents.y + 0.5f) > 0) {
            return forwardHit[0];
        }
        return forwardHit[0];
    }

    void FloatMovementUpdate() {
        newVel = rb2d.velocity + (targetVelocity * Time.deltaTime);
        
        if(newVel.x > 0)
        {
            newVel.x -= gravityModifier;
        }
        else if(newVel.x < 0)
        {
            newVel.x += gravityModifier;
        }

        if(newVel.y > 0)
        {
            newVel.y -= gravityModifier;
        }
        else if(newVel.y < 0)
        {
            newVel.y += gravityModifier;
        }

        newVel.x = Mathf.Clamp(newVel.x, -maxYVelocity, maxYVelocity);
        newVel.y = Mathf.Clamp(newVel.y, -maxYVelocity, maxYVelocity);

        rb2d.velocity = newVel;
    }

    public Vector3 GetPointInColliders() {

        Collider2D[] colList = new Collider2D[rb2d.attachedColliderCount];
        int numOFCols = rb2d.GetAttachedColliders(colList);
        if (numOFCols > 1) {
            int ranNum = Random.Range(0, numOFCols);
            float xWidth = colList[ranNum].bounds.extents.x;
            float yWidth = colList[ranNum].bounds.extents.y;
            float x = Random.Range(-xWidth, xWidth);
            float y = Random.Range(-yWidth, yWidth);
            return new Vector3(colList[ranNum].transform.position.x + x, colList[ranNum].transform.position.y + y);
        }
        else {
            float xWidth = colList[0].bounds.extents.x;
            float yWidth = colList[0].bounds.extents.y;
            float x = Random.Range(-xWidth, xWidth);
            float y = Random.Range(-yWidth, yWidth);
            return new Vector3(colList[0].transform.position.x + x, colList[0].transform.position.y + y);
        }
    }
}