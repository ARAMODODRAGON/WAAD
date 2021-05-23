using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float speed;
	float halfSpeed;
	float currentSpeed;

	public Rigidbody2D rb { get; private set; }

	Vector2 moveDir;

	bool hasShot = false;
	// gameObject prefab
	[SerializeField] GameObject bullet;

	[SerializeField] int testFireRate;
	[SerializeField] float testReloadRate;
	[SerializeField] float bulletSpeed;
	float currentReloadTime;
	[SerializeField] int testShots;
	int testShotsCurrent;
	float timerCurrent;

	bool aiming = false;
	bool reloading = false;

	[SerializeField] private StatGenerator statGenerator = null;
	private CharacterStats characterStats = CharacterStats.Null;
	private WeaponBaseStats weaponBaseStats = WeaponBaseStats.Null;
	private WeaponStats weaponStats = WeaponStats.Null;

	private int health;

	private void Awake()
	{
		characterStats = statGenerator.GenerateCharacter();
		weaponBaseStats = statGenerator.BasicWeapon;
		weaponStats = statGenerator.Calculate(characterStats, weaponBaseStats);
	}

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		timerCurrent = 1.0f - ((float)weaponStats.firerate / 60.0f);
		halfSpeed = speed / 2.0f;

		testShotsCurrent = testShots;
		currentReloadTime = testReloadRate;

		health = characterStats.maxHitpoints;
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
		HandleRotation();
		HandleReloading();
	}

	void HandleInput()
	{
		// handle movement inputs
		moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//handle shooting
		if (Input.GetButton("Fire1"))
		{
			HandleShooting();
		}

		if (Input.GetButtonDown("Fire3") && !Input.GetButton("Fire1"))
		{
			reloading = true;
		}


	}

	void HandleMovement()
	{
		if (rb != null)
		{
			rb.velocity = moveDir * currentSpeed * Time.fixedDeltaTime;
		}
	}

	void HandleShooting()
	{
		if (!hasShot && aiming && testShotsCurrent > 0 && !reloading)
		{
			Debug.Log("Shot");
			testShotsCurrent--;
			GameObject g = Instantiate(bullet, transform.position, Quaternion.identity);

			Vector3 mouseScreenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			g.GetComponent<PlayerProjectile>().UpdateProjectileParameters(bulletSpeed, weaponStats.damage, new Vector2(mouseScreenPoint.x - transform.position.x, mouseScreenPoint.y - transform.position.y), this);
			hasShot = true;
		}
		else
		{
			// start the timer
			timerCurrent -= Time.deltaTime;

			if (timerCurrent <= 0)
			{
				timerCurrent = 1.0f - ((float)weaponStats.firerate / 60.0f);
				hasShot = false;
			}
		}
	}

	void HandleRotation()
	{
		if (Input.GetButton("Fire2") )
		{

			aiming = true;
			//Vector2 objScreenPoint = Camera.main.ScreenToWorldPoint(transform.position);
			Vector3 mouseScreenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Vector2 dir = new Vector2(mouseScreenPoint.x - transform.position.x, mouseScreenPoint.y - transform.position.y);

			transform.up = dir;

			currentSpeed = halfSpeed;
		}
		else
		{
			aiming = false;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.up), 5.0f * Time.fixedDeltaTime);
			currentSpeed = speed;
		}

	}

	void HandleReloading()
	{
		// check to see if the player is trying to reload while reloading 
		if (reloading && testShotsCurrent != testShots)
		{
			Debug.Log("Reloading!");
			reloading = true;
			currentReloadTime -= Time.deltaTime;

			if (currentReloadTime <= 0)
			{
				reloading = false;
				testShotsCurrent = testShots;
				currentReloadTime = testReloadRate;
			}
		}
	}

	public void TakeDamage(int damage_)
	{

	}

	public void SetBaseStats(CharacterStats characterStats_, WeaponBaseStats weaponBaseStats_)
	{
		characterStats = characterStats_;
		weaponBaseStats = weaponBaseStats_;
	}
}
