using System;
using UnityEngine;
using System.Collections.Generic;

public class WormController : MonoBehaviour
{
    public GameObject head;
    public GameObject body;
    public GameObject tail;

    private List<GameObject> segments = new List<GameObject>();
    private Vector2 direction = Vector2.up;
    private Vector2 nextDirection = Vector2.up;
    private bool isMoving = false;
    private bool canMove = true;

    void Start()
    {
        segments.Add(head);
        segments.Add(body);
        segments.Add(tail);

        InitializeRigidbodies();
        SetupHingeJoints();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        // Check if any segment is touching the ground or an apple
        canMove = IsTouchingGroundOrApple();

        if (isMoving && canMove)
        {
            Move();
            isMoving = false;
        }

        CheckAppleProximity();
    }

    void HandleInput()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down)
            {
                nextDirection = Vector2.up;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
            {
                nextDirection = Vector2.down;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
            {
                nextDirection = Vector2.left;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left)
            {
                nextDirection = Vector2.right;
                isMoving = true;
            }

            // Update the direction only if the new direction is not the opposite of the current direction
            if (isMoving && direction + nextDirection != Vector2.zero)
            {
                direction = nextDirection;
            }
        }
    }

    void Move()
    {
        Physics2D.IgnoreLayerCollision(3,7,true);
        // Move the head
        Vector3 previousPosition = segments[0].transform.position;
        Vector3 newPosition = previousPosition + new Vector3(direction.x, direction.y, 0);

        segments[0].transform.position = new Vector3(
            Mathf.Round(newPosition.x),
            Mathf.Round(newPosition.y),
            Mathf.Round(newPosition.z) // Round the z position as well, just in case
        );

        // Move the body segments
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 tempPosition = segments[i].transform.position;
            segments[i].transform.position = new Vector3(
                Mathf.Round(previousPosition.x),
                Mathf.Round(previousPosition.y),
                Mathf.Round(previousPosition.z) // Round the z position as well, just in case
            );
            previousPosition = tempPosition;
        }

        // Clamp the positions of all segments to ensure they are integers
        foreach (var segment in segments)
        {
            ClampPosition(segment);
        }

        Physics2D.IgnoreLayerCollision(3, 7, false);
    }

    void ClampPosition(GameObject segment)
    {
        if (segment != null)
        {
            segment.transform.position = new Vector3(
                Mathf.Round(segment.transform.position.x),
                Mathf.Round(segment.transform.position.y),
                Mathf.Round(segment.transform.position.z) // Round the z position as well, just in case
            );
        }
    }

    void InitializeRigidbodies()
    {
        foreach (var segment in segments)
        {
            if (segment != null)
            {
                var rb = segment.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 1; // Default gravity scale
                    rb.isKinematic = false;
                }
            }
        }
    }

    void SetupHingeJoints()
    {
        for (int i = 0; i < segments.Count - 1; i++)
        {
            if (segments[i] != null && segments[i + 1] != null)
            {
                var currentSegment = segments[i].GetComponent<Rigidbody2D>();
                var nextSegment = segments[i + 1].GetComponent<Rigidbody2D>();

                if (currentSegment != null && nextSegment != null)
                {
                    var hingeJoint = segments[i].AddComponent<HingeJoint2D>();
                    hingeJoint.connectedBody = nextSegment;
                    hingeJoint.autoConfigureConnectedAnchor = false;
                    hingeJoint.anchor = Vector2.zero;
                    hingeJoint.connectedAnchor = Vector2.zero;
                }
            }
        }
    }

    void CheckAppleProximity()
    {
        bool isNextToApple = false;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(body.transform.position, 0.51f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Apple"))
            {
                /*
                Vector3 directionToApple = hitCollider.transform.position - body.transform.position;
                if (Mathf.Abs(directionToApple.x) <= 1 && Mathf.Abs(directionToApple.y) <= 1 && (Mathf.Abs(directionToApple.x) == 0 || Mathf.Abs(directionToApple.y) == 0))
                {*/
                isNextToApple = true;
                //Debug.Log("Body is directly next to an apple.");
                break;
            }
        }

        SetGravity(!isNextToApple);
    }

    public void SetGravity(bool enableGravity)
    {
        float gravityScale = enableGravity ? 0.1f : 0f;
        foreach (var segment in segments)
        {
            if (segment != null)
            {
                var rb = segment.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = gravityScale;
                }

                if (!enableGravity)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                  //  rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
                else
                {
                    //rb.constraints = RigidbodyConstraints2D.None;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
    }


    bool IsTouchingGroundOrApple()
    {
        foreach (var segment in segments)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(segment.transform.position, 0.51f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Ground") || hitCollider.CompareTag("Apple"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
          Destroy(other.transform.parent.gameObject);
            Debug.Log("Destroying");
        }
    }
}