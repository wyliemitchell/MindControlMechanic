using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEnemyFriendly : MonoBehaviour
{
    public float friendlyRadius = 5f;

    private bool isChanneling = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isChanneling)
        {
            StartCoroutine(ChannelNearestEnemy());
        }
        
        
    }

    IEnumerator ChannelNearestEnemy()
    {
        isChanneling = true;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, friendlyRadius);
            //Physics.OverlapSphere(transform.position, friendlyRadius);

        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;


        for(int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                //Do stuff because this is an enemy.
            }
        }

        
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemyScript = collider.GetComponent<Enemy>();
                /*
                if(enemyScript.enemyType == EnemyType.Basic)
                {

                }

                if (enemyScript != null)
                {
                    string enemyType = enemyScript.GetEnemyType();

                    if (enemyChannelingTimes.ContainsKey(enemyType))
                    {
                        float channelingTime = enemyChannelingTimes[enemyType];

                        float distance = Vector3.Distance(transform.position, collider.transform.position);

                        if (distance < nearestDistance)
                        {
                            nearestEnemy = enemyScript;
                            nearestDistance = distance;
                        }

                        yield return new WaitForSeconds(channelingTime);
                    }
                    else
                    {
                        Debug.LogWarning("No channeling time specified for enemy type: " + enemyType);
                    }
                }
                */
            }
        }
        
        yield return null;
        if (nearestEnemy != null)
        {
            nearestEnemy.ToggleFriendlyState();
        }

        isChanneling = false;
    }
}
