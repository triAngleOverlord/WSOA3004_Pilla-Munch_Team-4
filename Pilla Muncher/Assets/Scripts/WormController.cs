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
        if (isMoving)
        {
            Move();
            isMoving = false;
        }

        CheckAppleProximity();
    }

    void HandleInput()
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

    void Move()
    {
        // Move the head
        Vector3 previousPosition = segments[0].transform.position;
        Vector3 newPosition = previousPosition + new Vector3(direction.x, direction.y, 0);

        segments[0].transform.position = new Vector3(
            Mathf.Round(newPosition.x),
            Mathf.Round(newPosition.y),
            newPosition.z
        );

        // Move the body segments
        for (int i = 1; i < segments.Count; i++)
        {
            Vector3 tempPosition = segments[i].transform.position;
            segments[i].transform.position = new Vector3(
                Mathf.Round(previousPosition.x),
                Mathf.Round(previousPosition.y),
                previousPosition.z
            );
            previousPosition = tempPosition;
        }

        // Clamp the positions of all segments
        foreach (var segment in segments)
        {
            ClampPosition(segment);
        }
    }

    void ClampPosition(GameObject segment)
    {
        if (segment != null)
        {
            segment.transform.position = new Vector3(
                Mathf.Round(segment.transform.position.x),
                Mathf.Round(segment.transform.position.y),
                segment.transform.position.z
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

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(body.transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Apple"))
            {
                Vector3 directionToApple = hitCollider.transform.position - body.transform.position;
                if (Mathf.Abs(directionToApple.x) <= 1 && Mathf.Abs(directionToApple.y) <= 1 && (Mathf.Abs(directionToApple.x) == 0 || Mathf.Abs(directionToApple.y) == 0))
                {
                    isNextToApple = true;
                    break;
                }
            }
        }

        SetGravity(!isNextToApple);
    }

    public void SetGravity(bool enableGravity)
    {
        float gravityScale = enableGravity ? 1f : 0f;
        foreach (var segment in segments)
        {
            if (segment != null)
            {
                var rb = segment.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = gravityScale;
                }
            }
        }
    }
}
