using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThoughBlock : MonoBehaviour
{

    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer sr;
    public bool above;
    public bool left;
    public bool right;
    public bool below;


    void Start()
    {
        if (!above && !left && right && below)
        {
            sr.sprite = _sprites[0];
        }
        else if (!above && left && right && below)
        {
            sr.sprite = _sprites[1];
        }
        else if (!above && left && !right && below)
        {
            sr.sprite = _sprites[2];
        }
        else if (above && !left && right && below)
        {
            sr.sprite = _sprites[3];
        }
        else if (above && left && right && below)
        {
            sr.sprite = _sprites[4];
        }
        else if (above && left && !right && below)
        {
            sr.sprite = _sprites[5];
        }
        else if (above && !left && right && !below)
        {
            sr.sprite = _sprites[6];
        }
        else if (above && left && right && !below)
        {
            sr.sprite = _sprites[7];
        }
        else if (above && left && !right && !below)
        {
            sr.sprite = _sprites[8];
        }
        else if (right)
        {
            sr.sprite = _sprites[3];
        }else if (above)
        {
            sr.sprite = _sprites[7];
        }else if (below)
        {
            sr.sprite = _sprites[1];
        }else if (left)
        {
            sr.sprite = _sprites[5];
        }
        else
        {
            sr.sprite = _sprites[4];
        }
    }
}