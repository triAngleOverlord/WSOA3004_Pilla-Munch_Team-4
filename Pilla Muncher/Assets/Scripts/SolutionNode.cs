using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SolutionNode : MonoBehaviour
{
    public bool ShouldContainABlock;
    public bool valid = false;
    [SerializeField] private SpriteRenderer sr;
    public Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {Validate(false);
        
    }

    public void Validate(bool isValid)
    {
        if (isValid)
        {
            valid = true;
           sr.color = Color.green;
        }
        else
        {
            valid = false;
            sr.color = Color.red;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
