using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float speed;

	public Rigidbody2D rb { get; private set; }

	Vector2 moveDir;

	bool hasShot = false;
	// gameObject prefab
	[SerializeField] GameObject bullet;

	[SerializeField] int testFireRate;
	float timerCurrent;
	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		timerCurrent = 1.0f - ((float)testFireRate / 60.0f);
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
		if (Input.GetButton("Jump"))
		{
			HandleShooting();
		}

	}

	void HandleMovement()
	{
		if (rb != null)
		{
			rb.velocity = moveDir * speed * Time.fixedDeltaTime;
		}
	}

	void HandleShooting()
	{
		if (!hasShot)
		{
			Debug.Log("Shot");
			//Instantiate(bullet, transform.position, Quaternion.identity);
			hasShot = true;
		}
		else
		{
			// start the timer

			timerCurrent -= Time.deltaTime;

			if (timerCurrent <= 0)
			{
				timerCurrent = 1.0f - ((float)testFireRate / 60.0f);
				hasShot = false;
			}
		}


	}
}
