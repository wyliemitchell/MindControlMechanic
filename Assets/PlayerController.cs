using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterAllegiance
{
    Player,
    Friendly,
    TempFriendly, //Mind Control
    Hostile
}

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
    public bool isHiding = false;
    private bool isInHidingSpotArea = false;

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public float mindControlRange = 1f;

    public const CharacterAllegiance characterAllegiance = CharacterAllegiance.Player;

    private InputAction hideAction;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hideAction = GetComponent<PlayerInput>().actions["Hide"];

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
        }


    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            // If movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {

                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // Set direction of sprite to movement direction
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        CheckPlayerHidden();
    }

    private void CheckPlayerHidden()
    {
        if (hideAction.IsPressed())
        {
            Debug.Log("Pressing Hide");
            if (isInHidingSpotArea)
            {
                HidePlayer();
            }
            else
            {
                UnhidePlayer();
            }
        }
        else
        {
            UnhidePlayer();
        }
    }

    private void HidePlayer()
    {
        if (isHiding == false)
        {
            Color hidingColor = new Color(1f, 1f, 1f, 0.5f);

            GetComponent<SpriteRenderer>().color = hidingColor;
            Debug.Log("Do this the first time you hide!");
        }
        isHiding = true;
    }

    private void UnhidePlayer()
    {
        if (isHiding == true)
        {
            Color normalColor = new Color(1f, 1f, 1f, 1f);

            GetComponent<SpriteRenderer>().color = normalColor;
        }
        //Debug.Log("Is not hiding!");
        isHiding = false;
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values betwen -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Can't move if there's no direction to move in
            return false;
        }

    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HidingSpot")
        {
            isInHidingSpotArea = true;
        }
        else if (collision.tag == "Enemy")
        {
            if (isHiding == false)
            {
                //Deal Damage to Player!
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "HidingSpot")
        {
            isInHidingSpotArea = false;
            UnhidePlayer();
        }
    }

    public void OnMindControl()
    {
        Debug.Log("Getting Called?");
        //Do all the logic for mind controlling an enemy (probably an enum for a channel time, etc.)
        Vector2 mcDir;

        if (spriteRenderer.flipX == true)
        {
            mcDir = Vector2.left;
        }
        else
        {
            mcDir = Vector2.right;
        }
        /*
        RaycastHit hit3D;

        if(Physics.Raycast(this.transform.position, Vector3.right, out hit3D)) //3D Raycast!
        {
            if(hit3D.collider.tag == "Enemy")
            {

            }
        }
        */
        /* //Raycast only does a line :( Feels bad if you miss
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, mcDir, mindControlRange);

        if(hit.collider != null)
        {
            Debug.Log(hit.transform.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit!");
            }
        }
        else
        {
            Debug.Log("Didn't Hit Anything");
        }*/
        Vector2 OppositeRectanglePoint = this.transform.position;

        /* //Perfectly Proper to check left or right and assign values accordingly. OR...
        if(mcDir == Vector2.right)
        {
            OppositeRectanglePoint += new Vector2(mindControlRange, -0.226f);
        }
        else if(mcDir == Vector2.left)
        {
            OppositeRectanglePoint += new Vector2(-mindControlRange, -0.226f);
        }*/

        OppositeRectanglePoint += new Vector2(mcDir == Vector2.left ? -mindControlRange : mindControlRange, -0.226f);


        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(this.transform.position, OppositeRectanglePoint);

        List<Transform> enemies = new List<Transform>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                Transform enemy = hitColliders[i].transform;

                enemies.Add(enemy);
            }
        }

        float shortestDistance = mindControlRange + 1;
        Transform currentTarget = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector2.Distance(this.transform.position, enemies[i].position) < shortestDistance)
            {
                shortestDistance = Vector2.Distance(this.transform.position, enemies[i].position);
                currentTarget = enemies[i];
            }
        }

        /* if(currentTarget != null) //This is the same as...
         {
             Debug.Log(currentTarget.name);
         }*/

        Debug.Log("Current Target's Name is: " + currentTarget?.name);

        // WYLIE - Check if current target is an enemy with the EnemyController script
        if (currentTarget != null && currentTarget.CompareTag("Enemy"))
        {
            EnemyController enemyController = currentTarget.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                // WYLIE - change the enemy's allegiance through the GetMindControlled function
                enemyController.GetMindControlled();
            }
            else
            {
                // WYLIE - test to make sure it's hitting the intended target
                Debug.LogWarning("Current Target does not have EnemyController script!");
            }
        }
    }
}
