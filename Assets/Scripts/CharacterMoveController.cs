using System;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveAcceleration;
    public float maxSpeed;

    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 velocityVector = rb2D.velocity;
        velocityVector.x = Mathf.Clamp(
            velocityVector.x + moveAcceleration * Time.deltaTime, 
            0f, maxSpeed);
        
        rb2D.velocity = velocityVector;
    }
}
