using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpAnimation : MonoBehaviour
{
    public UnityEvent onGroundHitEvent;
    public Transform trnsObject;
    public Transform trnsBody;
    public float gravity = -10;
    public Vector2 groundVelocity;
    public float verticalVelocity;
    private float lastVerticalVelocity;
    public bool isGrounded;
    private float randomYDrop;
    float firstYPos;

    private void Awake()
    {


    }
    void Start()
    {
        randomYDrop = Random.Range(-1f, 1f);
        firstYPos = transform.position.y;
        Set(Vector3.right * Random.Range(-1, 2) * Random.Range(1f, 3f), Random.Range(2f, 5f));
    }

    void Update()
    {
        UPosition();
        CheckGroundHit();
    }
    public void Set(Vector2 groundVelocity, float verticalVelocity)
    {
        isGrounded = false;
        this.groundVelocity = groundVelocity;
        this.verticalVelocity = verticalVelocity;
        lastVerticalVelocity = verticalVelocity;

    }
    public void UPosition()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            trnsBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
        }
        trnsObject.position += (Vector3)groundVelocity * Time.deltaTime;

    }
    void CheckGroundHit()
    {
        if (trnsBody.position.y < firstYPos - randomYDrop && !isGrounded)
        {
            trnsBody.position = new Vector2(trnsObject.position.x, firstYPos - randomYDrop);
            isGrounded = true;
            GroundHit();
        }
    }
    void GroundHit()
    {
        onGroundHitEvent.Invoke();
    }
    public void Bounce(float division)
    {
        Set(groundVelocity, lastVerticalVelocity / division);
    }
    public void SlowDownVelocity(float division)
    {
        groundVelocity = groundVelocity / division;
    }
}