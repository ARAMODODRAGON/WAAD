using System;
using UnityEngine;

[Serializable]
public struct EnemyStats
{
    public int health;
    public int damage;
    public float speed;
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

    void FixedUpdate()
    {
    }

    //TODO
    //Add decision component

    public void UpdateLevelData(LevelBase level_)
    {

    }
}
