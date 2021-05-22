using System;
using UnityEngine;

[Serializable]
public struct EnemyStats
{
    public int health;
    public int damage;
    public float speed;
    public float rotationSpeed;
}
public class EnemyBase : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private DecisionCompBase decisionComp;
    public EnemyStats stats;
    private bool bActive = false;
    private Vector2 destination = new Vector2(0.0f, 0.0f);

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        EnemyManager.instance.AddEnemyToList(this);
    }

    private void Update()
    {
        if (EnemyManager.instance.playerRef)
        {
            Vector2 dir = EnemyManager.instance.playerRef.transform.position - gameObject.transform.position;
            dir.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), stats.rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
    }

    //TODO
    //Add decision component

    public void UpdateLevelData(LevelBase level_)
    {

    }
}
