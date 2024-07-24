using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private int CellSize;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject gridHolder;
    [SerializeField] private GameObject prevBlock;
    void Start()
    {
        generateGrid();
    }

    private void generateGrid()
    {
        var relativeXZero = 0 - CellSize*gridSize.x / 2+ 0.5f*CellSize;
        var relativeYZero = 0 - 3.5f;
        var x = gridSize.x;
        var y = gridSize.y;
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                var block =  Instantiate(blockPrefab, gridHolder.transform);
                block.transform.position = new Vector3(relativeXZero + j, relativeYZero + i);
                var joint = block.GetComponent<FixedJoint2D>();
                if (j>0)
                {
                    joint.connectedBody = prevBlock.GetComponent<Rigidbody2D>();
                }
                else
                {
                    joint.enabled = false;
                }

                prevBlock = block;
            }   
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
