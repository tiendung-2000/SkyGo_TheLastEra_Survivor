using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyController enemyController;
    EnemyAttack enemyAttack;

    [Header("Chase Player")]
    public float rangeToChasePlayer;

    [Header("Wandering")]
    public float wanderLength;
    public float pauseLength;
    [HideInInspector]
    public float wanderCounter, pauseWanderCounter;
    [HideInInspector]
    public Vector3 wanderDirection;
    public bool shouldSpawnObject;

    [HideInInspector]
    public Vector3 moveDirection;

    [Header("Active Move Mode")]
    public bool isMoving;
    public bool isChasePlayer;
    public bool isWander;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyAttack = GetComponent<EnemyAttack>();

        if (isWander)
        {
            pauseWanderCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }

    public void EnemyMoving()
    {
        if (isMoving == true)
        {
            moveDirection = Vector3.zero;

            if (isChasePlayer)
            {
                moveDirection = PlayerController.Ins.transform.position - transform.position;
                transform.position = Vector2.MoveTowards(transform.position, 
                    new Vector2(PlayerController.Ins.transform.position.x -2f, PlayerController.Ins.transform.position.y - 2f), 
                    enemyController.moveSpeed * Time.deltaTime);
            }
            else
            {
                if (isWander)
                {
                    EnemyWander();
                }
            }
            moveDirection.Normalize();
        }
    }

    public void EnemyWander()
    {
        if (isWander)
        {
            enemyController.theRB.velocity = moveDirection * enemyController.moveSpeed;
            if (wanderCounter > 0)
            {
                wanderCounter -= Time.deltaTime;

                //move the enemy
                moveDirection = wanderDirection;

                if (wanderCounter <= 0)
                {
                    pauseWanderCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                }
            }

            if (pauseWanderCounter > 0)
            {
                pauseWanderCounter -= Time.deltaTime;
                if (pauseWanderCounter <= 0)
                {
                    wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                    wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

                    //spawn object
                    if (shouldSpawnObject)
                    {
                        enemyAttack.EnemySpawnObject();
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            EnemyWander();
        }
    }
}
