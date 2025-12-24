using UnityEngine;
using System.Collections.Generic;

public enum PlayerDirection { Left, Right, Up, Bottom }

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    public GameObject bodyPart;
    public Rigidbody2D rb;

    [Header("Movement Settings")]
    public float velocity;
    public float step;
    public float movementDelay;

    private List<GameObject> bodyParts = new List<GameObject>();

    private Vector3 targetPosition;
    private List<Vector3> positionsHistory = new List<Vector3>();

    private Vector3 leftMovement;
    private Vector3 rightMovement;
    private Vector3 topMovement;
    private Vector3 bottomMovement;

    private PlayerDirection? direction;
    private PlayerDirection? lastDirection;

    private bool updatePlayerBehaviour = false;

    private float movementDelayTimer;

    private float? positionBeforeFalling;

    private Vector3 tempVector3;

    void Start()
    {
        movementDelayTimer = movementDelay;

        leftMovement = new Vector3(-step, 0, 0);
        rightMovement = new Vector3(step, 0, 0);
        topMovement = new Vector3(0, step, 0);
        bottomMovement = new Vector3(0, -step, 0);

        positionsHistory.Add(transform.position);
    }

    void Update()
    {
        if (rb.linearVelocity.y != 0f)
        {
            // Reset player movement delay every time he start falling
            movementDelayTimer = movementDelay;
        }
        else
        {
            movementDelayTimer -= Time.deltaTime;
        }

        // The player is moving toward the target position
        if (direction != null && transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);

            if (positionBeforeFalling != null)
            {
                if(positionBeforeFalling != transform.position.y && positionBeforeFalling > transform.position.y)
                {
                    for (int i = 0; i < positionsHistory.Count - 1; i++)
                    {
                        tempVector3 = positionsHistory[i];
                        tempVector3.y -= (float)positionBeforeFalling - transform.position.y;
                        positionsHistory[i] = tempVector3;
                    }
                }

                positionBeforeFalling = null;
            }

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].transform.position = Vector2.MoveTowards(bodyParts[i].transform.position, positionsHistory[positionsHistory.Count - 2 - i], velocity * Time.deltaTime);
            }

            // Toggle the player behaviour in order to not call the code inside this function too many times
            if (updatePlayerBehaviour == false)
            {
                rb.gravityScale = 0f;
                updatePlayerBehaviour = true;
            }
        }
        // The player is not moving toward the target position and is waiting for the next input
        else
        {
            // When the player reach his targetPosition
            if (updatePlayerBehaviour)
            {
                // Give the player the possibility to fall
                rb.gravityScale = 1.0f;

                // Set the position before falling after adding the gravity. This will be used to re-calculate the targets position based on the height he fell
                positionBeforeFalling = transform.position.y;

                // Set the transform as the parent of all the body parts, so that they all fall togheter for gravity
                toggleBodyPartsParents(transform);

                // Reset player movement delay every time he start falling
                movementDelayTimer = movementDelay;
                updatePlayerBehaviour = false;
            }

            if (transform.position == targetPosition)
            {
                direction = null;
            }

            if(movementDelayTimer <= 0f)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    if(bodyParts.Count == 0 || lastDirection != PlayerDirection.Right)
                    {
                        targetPosition = transform.position + leftMovement;
                        positionsHistory.Add(targetPosition);
                        direction = PlayerDirection.Left;
                        lastDirection = direction;
                        toggleBodyPartsParents(null);
                    }
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    if(bodyParts.Count == 0 || lastDirection != PlayerDirection.Left)
                    {
                        targetPosition = transform.position + rightMovement;
                        positionsHistory.Add(targetPosition);
                        direction = PlayerDirection.Right;
                        lastDirection = direction;
                        toggleBodyPartsParents(null);
                    }
                }
                else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    targetPosition = transform.position + topMovement;
                    positionsHistory.Add(targetPosition);
                    direction = PlayerDirection.Up;
                    lastDirection = direction;
                    toggleBodyPartsParents(null);
                }
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    targetPosition = transform.position + bottomMovement;
                    positionsHistory.Add(targetPosition);
                    direction = PlayerDirection.Bottom;
                    lastDirection = direction;
                    toggleBodyPartsParents(null);
                }
            }
        }
    }

    void addBodyPart()
    {
        bodyParts.Add(Instantiate(bodyPart, positionsHistory[positionsHistory.Count - 2 - bodyParts.Count], Quaternion.identity));
    }

    void toggleBodyPartsParents(Transform parent)
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.parent = parent;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Food")
        {
            addBodyPart();
            collider.gameObject.SetActive(false);
        }
        else if(collider.tag == "Portal")
        {
            GameManager.instance.nextLevel();
        }
    }
}
