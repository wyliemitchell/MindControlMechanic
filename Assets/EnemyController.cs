using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Minion,
    Basic,
    Elite,
    Miniboss,
    Boss
}

public class EnemyController : MonoBehaviour
{
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
    }

    void Update()
    {
        CalculateTarget();
        MoveToTarget();
    }

    void CalculateTarget()
    {
        Vector2 thisObject = new Vector2(this.transform.position.x, this.transform.position.y);

        if (characterAllegiance == CharacterAllegiance.Hostile)
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

            if(Vector3.Distance(thisObject, playerPos) < aggroRadius)
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
                        Transform enemy = hitColliders[i].transform;

                        enemies.Add(enemy);
                    }
                }

                float shortestDistance = aggroRadius + 1;
                Transform currentTarget = null;
                

                for (int i=0; i< enemies.Count; i++)
                {
                    Vector2 enemyLocation = new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y);

                    if(Vector2.Distance(thisObject, enemyLocation) < shortestDistance)
                    {
                        shortestDistance = Vector2.Distance(thisObject, enemyLocation);
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
    }

    public void RevertMindControl()
    {
        characterAllegiance = CharacterAllegiance.Hostile;
    }
}
