using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
	protected Rigidbody2D rigidBody;
	protected SpriteRenderer spriteRenderer;
	protected float speed = 0.0f;
    protected int damage = 0;
	protected Vector2 direction = new Vector2(0.0f, 0.0f);
	protected PlayerController playerRef = null;
	protected bool bHoming = false;
    public float lifeSpan = 3.0f;

    // Start is called before the first frame update
    protected void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Invoke("DelayedDestroy", lifeSpan);
    }

    protected void FixedUpdate()
    {
        if (rigidBody)
        {
            Vector2 movementVec = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction * speed * Time.deltaTime;
            rigidBody.MovePosition(movementVec);
        }
    }

    public void UpdateProjectileParameters(float speed_, int damage_, Vector2 direction_, PlayerController playerRef_ = null, bool bHoming_ = false)
    {
        speed = speed_;
        damage = damage_;
        direction = direction_;
        playerRef = playerRef_;
        bHoming = bHoming_;
    }

    private void DelayedDestroy()
    {

    }
}
