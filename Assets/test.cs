using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private bool isFriendly = false;

    public void ToggleFriendlyState()
    {
        isFriendly = !isFriendly;

        if (isFriendly)
        {
            Debug.Log("Enemy turned friendly!");

            GetComponent<SpriteRenderer>().color = Color.green;

            GetComponent<EnemyController>().enabled = false;
        }
        else
        {
            Debug.Log("Enemy turned unfriendly!");

            GetComponent<SpriteRenderer>().color = Color.red;

            GetComponent<EnemyController>().enabled = true;
        }
    }
}
