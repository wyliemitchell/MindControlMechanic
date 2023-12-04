using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Minion,
    Basic,
    Elite,
    Miniboss,
    Boss,
    Trash
}

public class EnemyController : MonoBehaviour
{
    //public Dictionary<EnemyType, float> enemyDropRates = new Dictionary<EnemyType, float>(); // Dictionaries!

    public float moveSpeed = 5f;
    public Transform player;

    private Transform target;
    private Rigidbody2D rb;

    public float aggroRadius = 1f;

    public EnemyType enemyType = EnemyType.Basic;
    public CharacterAllegiance characterAllegiance = CharacterAllegiance.Hostile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        /* //Dictionaries!
        enemyDropRates.Add(EnemyType.Minion, .15f);
        enemyDropRates.Add(EnemyType.Basic, .25f);
        enemyDropRates.Add(EnemyType.Elite, .5f);
        enemyDropRates.Add(EnemyType.Miniboss, .75f);
        enemyDropRates.Add(EnemyType.Boss, 1f);

        //Debug.Log(enemyDropRates.GetValueOrDefault(EnemyType.Elite));

        float DropRate;

        if(enemyDropRates.TryGetValue(EnemyType.Miniboss, out DropRate))
        {
            Debug.Log(DropRate);
        }*/
    }

    void Update()
    {
        CalculateTarget();
        MoveToTarget();
    }

    void CalculateTarget()
    {
        if (characterAllegiance == CharacterAllegiance.Hostile)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position) < aggroRadius)
            {
                target = player;
            }
            else
            {
                target = null;
            }
        }
        else if(characterAllegiance == CharacterAllegiance.TempFriendly)
        {
            if(target == player || target == null)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, aggroRadius);

                List<Transform> enemies = new List<Transform>();

                for(int i=0; i< hitColliders.Length; i++)
                {
                    if(hitColliders[i].CompareTag("Enemy"))
                    {
                        if (this.gameObject != hitColliders[i].gameObject)
                        {
                            Transform enemy = hitColliders[i].transform;

                            enemies.Add(enemy);
                        }
                    }
                }

                float shortestDistance = aggroRadius + 1;
                Transform currentTarget = null;

                for (int i=0; i< enemies.Count; i++)
                {
                    Vector2 enemyLocation = new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y);

                    if(Vector2.Distance(this.transform.position, enemies[i].position) < shortestDistance)
                    {
                        shortestDistance = Vector2.Distance(this.transform.position, enemies[i].position);
                        currentTarget = enemies[i];
                    }
                }

                target = currentTarget;
            }
        }
    }

    void MoveToTarget()
    {
        if(target != null)
        {
            // Calculate direction towards player
            Vector2 direction = target.position - transform.position;

            // Normalize direction
            direction.Normalize();

            // Calculate movement vector
            Vector2 move = direction * moveSpeed;

            // Move enemy
            rb.velocity = new Vector2(move.x, move.y);
        }
    }

    public void GetMindControlled()
    {
        characterAllegiance = CharacterAllegiance.TempFriendly;
        target = null;
    }

    public void RevertMindControl()
    {
        characterAllegiance = CharacterAllegiance.Hostile;
        target = null;
    }
}
