using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float speed;

	public Rigidbody2D rb { get; private set; }

	Vector2 moveDir;
	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		HandleInput();

	}

	void FixedUpdate()
	{
		// for physics stuff
		HandleMovement();
	}

	void HandleInput()
	{
		// handle movement inputs
		moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		//handle shooting
		//if()
	}

	void HandleMovement()
	{
		if (rb != null)
		{
			rb.velocity = moveDir * speed * Time.fixedDeltaTime;
		}
	}
}
