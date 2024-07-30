using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WormController : MonoBehaviour
{
    public GameObject head;
    public GameObject body;
    public GameObject tail;

    public Sprite headUpSprite;
    public Sprite headDownSprite;
    public Sprite headHorizontalSprite; // Use this for both left and right
    public Sprite tailUpSprite;
    public Sprite tailDownSprite;
    public Sprite tailHorizontalSprite; // Use this for both left and right

    public Animator tailAnimator;

    private List<GameObject> segments = new List<GameObject>();
    private Vector2 direction = Vector2.up;
    private Vector2 nextDirection = Vector2.up;
    private bool isMoving = false;
    private bool canMove = true;

    private Stack<List<Vector3>> positionHistory = new Stack<List<Vector3>>();
    private Stack<List<Quaternion>> rotationHistory = new Stack<List<Quaternion>>();

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
        HandleUndoAndReset();
    }

    void FixedUpdate()
    {
        // Check if any segment is touching the ground or an apple
        canMove = IsTouchingGroundOrApple();

        if (isMoving && canMove)
        {
            // Save current state before moving
            SaveState();

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

    void HandleUndoAndReset()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoLastMove();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ResetScene();
        }
    }

    void Move()
    {
        Physics2D.IgnoreLayerCollision(3, 7, true);

        // Store the current positions and rotations of each segment
        List<Vector3> previousPositions = new List<Vector3>();
        List<Quaternion> previousRotations = new List<Quaternion>();
        foreach (var segment in segments)
        {
            previousPositions.Add(segment.transform.position);
            previousRotations.Add(segment.transform.rotation);
        }

        // Move and rotate the head
        Vector3 newPosition = previousPositions[0] + new Vector3(direction.x, direction.y, 0);
        segments[0].transform.position = new Vector3(
            Mathf.Round(newPosition.x),
            Mathf.Round(newPosition.y),
            Mathf.Round(newPosition.z) // Round the z position as well, just in case
        );
        Quaternion headRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        segments[0].transform.rotation = headRotation;

        // Update the head sprite based on the rotation
        float headRotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        UpdateSprite(segments[0], headRotationAngle, true);

        // Move and rotate the body segments to follow the previous segment's position
        for (int i = 1; i < segments.Count - 1; i++)
        {
            segments[i].transform.position = new Vector3(
                Mathf.Round(previousPositions[i - 1].x),
                Mathf.Round(previousPositions[i - 1].y),
                Mathf.Round(previousPositions[i - 1].z) // Round the z position as well, just in case
            );

            // Apply the head's rotation to the body segments
            segments[i].transform.rotation = headRotation;
        }

        // Move the tail to the previous position of the last body segment
        segments[segments.Count - 1].transform.position = new Vector3(
            Mathf.Round(previousPositions[segments.Count - 2].x),
            Mathf.Round(previousPositions[segments.Count - 2].y),
            Mathf.Round(previousPositions[segments.Count - 2].z) // Round the z position as well, just in case
        );

        // Apply the previous rotation of the last body segment to the tail
        segments[segments.Count - 1].transform.rotation = previousRotations[segments.Count - 2];

        // Update the tail sprite based on the previous rotation of the last body segment
        float tailRotationAngle = previousRotations[segments.Count - 2].eulerAngles.z;
        UpdateSprite(segments[segments.Count - 1], tailRotationAngle, false);

        // Clamp the positions of all segments to ensure they are integers
        foreach (var segment in segments)
        {
            ClampPosition(segment);
        }

        Physics2D.IgnoreLayerCollision(3, 7, false);
    }

    void UpdateSprite(GameObject segment, float rotationAngle, bool isHead)
    {
        SpriteRenderer spriteRenderer = null;

        // Find the SpriteRenderer component in the child GameObjects
        if (segment.transform.childCount > 0)
        {
            spriteRenderer = segment.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            if (isHead)
            {
                if (rotationAngle == 0)
                {
                    spriteRenderer.sprite = headHorizontalSprite;
                    spriteRenderer.flipX = true; // Horizontal sprite facing right
                    spriteRenderer.flipY = false;
                }
                else if (rotationAngle == 90 || rotationAngle == -270)
                {
                    spriteRenderer.sprite = headUpSprite;
                    spriteRenderer.flipX = true; // Up sprite facing left
                }
                else if (rotationAngle == 180 || rotationAngle == -180)
                {
                    spriteRenderer.sprite = headHorizontalSprite;
                    spriteRenderer.flipX = true; // Horizontal sprite facing left
                    spriteRenderer.flipY = true;
                }
                else if (rotationAngle == 270 || rotationAngle == -90)
                {
                    spriteRenderer.sprite = headDownSprite;
                    spriteRenderer.flipX = true; // Down sprite facing left
                }
            }
            else
            {
                if (rotationAngle == 0)
                {
                    spriteRenderer.sprite = tailHorizontalSprite;
                    tailAnimator.Play("side pilla butt_FWD");
                    spriteRenderer.flipX = true; // Horizontal sprite facing right
                    spriteRenderer.flipY = false;
                }
                else if (rotationAngle == 90 || rotationAngle == -270)
                {
                    spriteRenderer.sprite = tailUpSprite;
                    tailAnimator.Play("up pilla butt_DEFAULT");
                    spriteRenderer.flipX = true; // Up sprite facing left
                }
                else if (rotationAngle == 180 || rotationAngle == -180)
                {
                    spriteRenderer.sprite = tailHorizontalSprite;
                    tailAnimator.Play("side pilla butt_FWD");
                    spriteRenderer.flipX = true; // Horizontal sprite facing left
                    spriteRenderer.flipY = true;
                }
                else if (rotationAngle == 270 || rotationAngle == -90)
                {
                    spriteRenderer.sprite = tailDownSprite;
                    tailAnimator.Play("down pilla_butt_FWD");
                    spriteRenderer.flipX = true; // Down sprite facing left
                }
            }
        }
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

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(body.transform.position, 0.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Apple"))
            {
                Debug.Log("next to apple");
                if (hitCollider.transform.position.y <= transform.position.y + 0.3f)
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

                if (!enableGravity)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }
    }

    bool IsTouchingGroundOrApple()
    {
        foreach (var segment in segments)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(segment.transform.position, 0.5f);
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
            SolutionController.Instance._blocks.Remove(other.gameObject);
            Debug.Log("Destroying");
        }
    }

    void SaveState()
    {
        List<Vector3> currentPositions = new List<Vector3>();
        List<Quaternion> currentRotations = new List<Quaternion>();
        foreach (var segment in segments)
        {
            currentPositions.Add(segment.transform.position);
            currentRotations.Add(segment.transform.rotation);
        }
        positionHistory.Push(currentPositions);
        rotationHistory.Push(currentRotations);
    }

    void UndoLastMove()
    {
        if (positionHistory.Count > 0 && rotationHistory.Count > 0)
        {
            List<Vector3> lastPositions = positionHistory.Pop();
            List<Quaternion> lastRotations = rotationHistory.Pop();

            for (int i = 0; i < segments.Count; i++)
            {
                segments[i].transform.position = lastPositions[i];
                segments[i].transform.rotation = lastRotations[i];

                // Update the sprites based on the rotations
                float rotationAngle = lastRotations[i].eulerAngles.z;
                UpdateSprite(segments[i], rotationAngle, i == 0); // The first segment is the head
            }
        }
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
