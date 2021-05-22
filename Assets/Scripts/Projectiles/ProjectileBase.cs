using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float speed = 0.0f;
    private float damage = 0.0f;
    private Vector2 direction = new Vector2(0.0f, 0.0f);
    private PlayerController playerRef = null;
    private bool bHoming = false;

    // Start is called before the first frame update
    protected void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    protected void FixedUpdate()
    {
        Vector2 movementVec = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction * speed * Time.deltaTime;
        rigidBody.MovePosition(movementVec);
    }

    public void UpdateProjectileParameters(float speed_, float damage_, Vector2 direction_, PlayerController playerRef_ = null, bool bHoming_ = false)
    {

    }
}
