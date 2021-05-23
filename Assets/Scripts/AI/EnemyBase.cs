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
}
public class EnemyBase : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private DecisionCompBase decisionComp;
    public EnemyStats stats;
    private Vector2 direction = new Vector2(0.0f, 0.0f);
    float distanceToDestination = 1000.0f;
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
            transform.up = direction;
            direction.Normalize();
           // Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), stats.rotationSpeed * Time.deltaTime);
            //rot.y = transform.rotation.y;
           // transform.rotation = rot;
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
        Vector2 pos0 = gameObject.transform.position;
        switch (stats.pattern)
        {
            case BulletsPattern.SINGLE:
                
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, direction);
                break;
            case BulletsPattern.DOUBLE:
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, direction);
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, new Vector2(direction.x,direction.y + 0.1f));
                break;
            case BulletsPattern.TRIPLE:
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, direction);
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, new Vector2(direction.x, direction.y + 0.1f));
                proj_ = Instantiate<EnemyProjectile>(bullet, pos0, gameObject.transform.rotation);
                if (proj_)
                    proj_.UpdateProjectileParameters(stats.bulletSpeed, stats.damage, new Vector2(direction.x, direction.y - 0.1f));

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
