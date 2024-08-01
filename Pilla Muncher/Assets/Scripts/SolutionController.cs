using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolutionController : MonoBehaviour
{
    private SolutionNode[] _solutionNodes;

    private float timecounter = 0;
    private float timeInterval = 2;
    public static SolutionController Instance;
    public List<GameObject> _blocks;

    private bool checking = false;

    private float timercounter = 0;
    private float timeout = 2f; 
    private float finTimerCounter = 0;
    private float finTimeOut = 2f;

    private bool fin;
    private bool heart;
    // Start is called before the first frame update
    void Start()
    {
        fin = false;
        _solutionNodes = GetComponentsInChildren<SolutionNode>();
        heart = false;
    }
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

    private bool CheckSolution()
    {
        var correct = false;
        foreach (var solution in _solutionNodes)
        {
            solution.Validate(!solution.ShouldContainABlock);
        }

        foreach (var block in _blocks)
        {
           
            if (block != null)
            {
                 
                foreach (var solutionNode in _solutionNodes)
                {
                    var type = solutionNode.ShouldContainABlock;
                    if (solutionNode.ShouldContainABlock &&
                        solutionNode.collider.bounds.Contains(block.transform.position))
                    {
                        solutionNode.Validate(type);
                    }
                    else if (!solutionNode.ShouldContainABlock&&solutionNode.collider.bounds.Contains(block.transform.position))
                    {
                        solutionNode.Validate(type);
                    }
                }
            }
        }

        foreach (var solnode in _solutionNodes)
        {
            if (!solnode.valid)
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        var correct =CheckSolution();
        if (correct)
        {
            checking = true;
            
        }
        else
        {
            checking = false;
            timecounter = 0;
        }

        if (checking)
        {
            if (heart == false)
            {
                Instantiate(Resources.Load("heart anim"));
                Instantiate(Resources.Load("complete title"));
                heart = true;
            }
            timecounter += Time.deltaTime;
            if (timecounter>= timeout)
            {
                Debug.Log("yaay");
                fin = true;
                
            }
        }

        if (fin)
        {
            finTimerCounter += Time.deltaTime;
        }

        if (finTimerCounter>=finTimeOut)
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator CheckSol()
    {
        CheckSolution();
        yield return new WaitForSeconds(0.2f);
        
    }
}
