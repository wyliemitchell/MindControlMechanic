using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

     public float Health {
        set {
            health = value;

            if (health <= 0){
                Defeated();
            }
        }
        get {
            return health;
        }
    }

    public float health = 1;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Defeated(){
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy() {
        Destroy(gameObject);
    }

    private bool isFriendly = false;

    // Call this method to toggle the state of the enemy
    public void ToggleFriendlyState()
    {
        isFriendly = !isFriendly;

        if (isFriendly)
        {
            // Change enemy behavior or appearance when turned friendly
            Debug.Log("Enemy turned friendly!");

            // Example: Change color to green
            GetComponent<SpriteRenderer>().color = Color.green;

            // Example: Stop attacking the player
            GetComponent<EnemyController>().enabled = false;
        }
        else
        {
            // Revert to unfriendly state
            Debug.Log("Enemy turned unfriendly!");

            // Example: Change color back to the original color
            GetComponent<SpriteRenderer>().color = Color.red;

            // Example: Enable attacking the player
            GetComponent<EnemyController>().enabled = true;
        }
    }

}


