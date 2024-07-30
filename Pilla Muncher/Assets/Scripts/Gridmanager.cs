using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private int CellSize;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject gridHolder;
    [SerializeField] private GameObject leftBlock;
    [SerializeField] private GameObject bottomBlock;
    [SerializeField] private SolutionController sc;
    public static Gridmanager Instance;
    private List<GameObject> blocks = new List<GameObject>();

    private void Awake()
    {
        if (Instance!= null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        generateGrid();
        sc._blocks = blocks;
    }

    private void generateGrid()
    {
        float adjustment;
        if (gridSize.x %2 ==0)
        {
            adjustment = 1f;
        }
        else
        {
            adjustment = 0.5f;
        }

        
        var relativeXZero = 0 - CellSize * gridSize.x / 2 + adjustment * CellSize;
        var relativeYZero = 0 - 5f;
        var x = gridSize.x;
        var y = gridSize.y;
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                var block = Instantiate(blockPrefab, gridHolder.transform);
                var appleScript = block.GetComponent<AppleBlock>();
                block.transform.position = new Vector3(relativeXZero + j, relativeYZero + i);
                var joints = block.GetComponents<FixedJoint2D>();
                if (i>0)
                {
                    appleScript.below = blocks[^(int)gridSize.x];
                    joints[1].connectedBody = blocks[^(int)gridSize.x].GetComponent<Rigidbody2D>();
                }
                else
                {
                    joints[1].enabled = false;
                }
                if (j > 0)
                {
                    joints[0].connectedBody = leftBlock.GetComponent<Rigidbody2D>();
                    appleScript.left = leftBlock;
                }
                else
                {
                    joints[0].enabled = false;
                }
                blocks.Add(block);
                leftBlock = block;
            }
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            var apple = blocks[i].GetComponent<AppleBlock>();
            if (i<blocks.Count-gridSize.x)
            {
                apple.above = blocks[i + (int)gridSize.x];
            }

            if ((i+1)%(gridSize.x)!=0)
            {
                apple.right = blocks[i + 1];
            }
           
        }
    }
}