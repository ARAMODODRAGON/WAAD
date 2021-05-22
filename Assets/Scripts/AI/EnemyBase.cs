using System;
using UnityEngine;

public enum BulletsPattern //How many bullets per shot
{
    SINGLE,
    DOUBLE,
    TRIPLE
}
[Serializable]
public struct EnemyStats
{
    public int health; //HP
    public int damage; //Damage for bullets
    public float speed; //Movement speed
    public float rotationSpeed; //Rotational speed
    public float attackRadius; //How far away from the player should you before you start shooting
    public float fireRate; //How frequently are bullets shot
    public int ammo;  //How many bullets per firing session
    public int shotsSoFar; //How many we've shot so far
    public float reloadTime; //How long to wait before shooting again
    public BulletsPattern pattern;
    public float bulletSpeed;
    public float bulletInitializationDistance; //How far away from the enemy's body does the bullet instantiate?
}
public class EnemyBase : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private DecisionCompBase decisionComp;
    public EnemyStats stats;
    private Vector2 direction = new Vector2(0.0f, 0.0f);
    float distanceToDestination = 0.0f;
    public EnemyProjectile bullet;
    private int enemyIndex;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        decisionComp = gameObject.GetComponent<DecisionCompBase>();
        enemyIndex = EnemyManager.instance.AddEnemyToList(this);
        decisionComp.NextStep();
    }

    private void OnDestroy()
    {
        EnemyManager.instance.RemoveEnemy(enemyIndex);
    }
    private void Update()
    {
        if (decisionComp.GetTarget())
        {
            direction = decisionComp.GetTarget().transform.position - gameObject.transform.position;
            distanceToDestination = direction.magnitude;
            direction.Normalize();
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), stats.rotationSpeed * Time.deltaTime);
            rot.y = transform.rotation.y;
            transform.rotation = rot;
        }
        else 
        {
            distanceToDestination = 0.0f; //Target is null, stay in place
        }
    }

    void FixedUpdate()
    {
        if (decisionComp)
        {
             if (distanceToDestination > stats.attackRadius)
              {
                  Vector2 movementVec = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction * stats.speed * Time.deltaTime;
                  rigidBody.MovePosition(movementVec);
              }
              else 
              {
                if(decisionComp.enemyState != EState.RELOADING && decisionComp.enemyState != EState.SHOOTING) //If we're not shooting or reloading, see what to do next
                {
                    decisionComp.NextStep();
                }
              }
        }
    }

    public void Shoot() //Called from the DC
    {
        //Instantiating the bullets
        EnemyProjectile proj_;
        switch (stats.pattern)
        {
            case BulletsPattern.SINGLE:
                Vector2 pos0 = gameObject.transform.position + gameObject.transform.forward * stats.bulletInitializationDistance;
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, gameObject.transform.forward);
                break;
            case BulletsPattern.DOUBLE:
                Vector2 pos1 = gameObject.transform.position + gameObject.transform.forward * stats.bulletInitializationDistance;
                Vector2 pos2 = pos1;
                pos2.y += 10.0f;
                proj_ = Instantiate<EnemyProjectile>(bullet, pos1, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, gameObject.transform.forward);
                proj_ = Instantiate<EnemyProjectile>(bullet, pos2, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, gameObject.transform.forward);
                break;
            case BulletsPattern.TRIPLE:

                Vector2 pos3 = gameObject.transform.position + gameObject.transform.forward * stats.bulletInitializationDistance;
                Vector2 pos4 = pos3;
                pos4.y += 10.0f;
                proj_ = Instantiate<EnemyProjectile>(bullet, pos3, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, gameObject.transform.forward);
                proj_ = Instantiate<EnemyProjectile>(bullet, pos4, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, gameObject.transform.forward);
                pos4.y -= 20.0f;
                proj_ = Instantiate<EnemyProjectile>(bullet, pos4, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, gameObject.transform.forward);

                break;
        }

        //See if you should shoot again
        stats.shotsSoFar++;
        if(stats.shotsSoFar < stats.ammo)
        {
            Invoke("Shoot", stats.fireRate);
        }
        else
        {
            stats.shotsSoFar = 0;
            if (decisionComp)
                decisionComp.NextStep();
        }
    }

    public void UpdateLevelData(LevelBase level_)
    {
        if (decisionComp)
            decisionComp.UpdateLevel(level_);
    }

    public void TakeDamage(int damage_)
    {
        stats.health -= damage_;
        if(stats.health <= 0)
        {
            //TODO
            Destroy(gameObject);
        }
    }
}
