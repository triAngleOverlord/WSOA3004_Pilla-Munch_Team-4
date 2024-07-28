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

    private List<GameObject> blocks = new List<GameObject>();

    void Start()
    {
        generateGrid();
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
                block.transform.position = new Vector3(relativeXZero + j, relativeYZero + i);
                var joint = block.GetComponent<FixedJoint2D>();
                if (j > 0)
                {
                    joint.connectedBody = leftBlock.GetComponent<Rigidbody2D>();
                }
                else
                {
                    if (i > 0)
                    {
                        joint.connectedBody = bottomBlock.GetComponent<Rigidbody2D>();
                    }
                    else
                    {
                        joint.enabled = false;
                    }

                    bottomBlock = block;
                }

                blocks.Add(block);

                leftBlock = block;
            }
        }
    }
}