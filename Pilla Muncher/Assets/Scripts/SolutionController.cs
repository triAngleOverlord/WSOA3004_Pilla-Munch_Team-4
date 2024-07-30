using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionController : MonoBehaviour
{
    private SolutionNode[] _solutionNodes;

    private float timecounter = 0;
    private float timeInterval = 2;
    
    public List<GameObject> _blocks;
    // Start is called before the first frame update
    void Start()
    {
        _solutionNodes = GetComponentsInChildren<SolutionNode>();
    }
    

    private void CheckSolution()
    {
        foreach (var solution in _solutionNodes)
        {
            solution.Validate(false);
        }
        foreach (var block in _blocks)
        {
            foreach (var solutionNode in _solutionNodes)
            {
                if (solutionNode.ShouldContainABlock&&solutionNode.collider.bounds.Contains(block.transform.position))
                {
                    solutionNode.Validate(true);
                }
                /*else if (!solutionNode.ShouldContainABlock&&!solutionNode.collider.bounds.Contains(block.transform.position))
                {
                    solutionNode.Validate(true);
                }*/
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckSolution();
    }
}
