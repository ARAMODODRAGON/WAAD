using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Decisions
{
    CHASE,
    ATTACK,
    FLEE,
    STAYINPLACE
}

public enum EState
{
    IDLE,
    SHOOTING,
    RELOADING
}

public class DecisionCompBase : MonoBehaviour
{

    private LevelBase level = null;
    private GameObject target;
    public EState enemyState { get; private set; }
    EnemyBase owner;

    delegate void decisionDelegate();
    public List<Decisions> decisionMap = new List<Decisions>();
    private List<decisionDelegate> decisions = new List<decisionDelegate>();
    private int decisionIndex = 0;

    private void Start()
    {
        foreach (Decisions d in decisionMap)
        {
            decisionDelegate step;
            switch (d)
            {
                case Decisions.CHASE:
                    step = TargetIsPlayer;
                    break;
                case Decisions.ATTACK:
                    step = TargetIsShoot;
                    break;
                case Decisions.FLEE:
                    step = TargetIsFlee;
                    break;
                case Decisions.STAYINPLACE:
                    step = TargetIsNull;
                    break;
                default:
                    step = TargetIsPlayer;
                    break;
            }

            decisions.Add(step);
        }

        owner = gameObject.GetComponent<EnemyBase>();
    }

    public void UpdateLevel(LevelBase level_)
    {
        level = level_;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void NextStep()
    {
        if (enemyState != EState.RELOADING)
        {
            //Move the decision index a step to the right and call the next function
            decisionIndex++;
            if (decisionIndex > decisions.Count)
                decisionIndex = 0;

            decisions[decisionIndex]();
        }
    }

    //Chase
    private void TargetIsPlayer()
    {
        target = EnemyManager.instance.playerRef.gameObject;
    }

    //Flee
    private void TargetIsFlee()
    {
        if (level)
            target = level.GetMinOrMaxAtRandom();
    }

    //Attack
    private void TargetIsShoot()
    {
        enemyState = EState.SHOOTING;
        if (owner)
            owner.Shoot();
    }

    //Reload
    private void TargetIsNull()
    {
        target = null;
        enemyState = EState.RELOADING;
        if(owner)
            Invoke("EndReload", owner.stats.reloadTime);
    }

    private void EndReload()
    {
        enemyState = EState.IDLE;
        NextStep();
    }


}
