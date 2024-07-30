using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AppleBlock : MonoBehaviour
{
    private FixedJoint2D[] joints;

    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer sr;
    public GameObject above;
    public GameObject left;
    public GameObject right;
    public GameObject below;
    // Start is called before the first frame update
    void Start()
    {
        joints = GetComponents<FixedJoint2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (var joint in joints)
        {
            if (!joint.connectedBody)
            {
                Debug.Log("checked");
                joint.enabled = false;
            }
        }

        if (!above&&!left&&right&&below)
        {
            sr.sprite = _sprites[0];
        }
        else if(!above&&left&&right&&below)
        {
            sr.sprite = _sprites[1];
        } else if (!above&&left&&!right&&below)
        {
            sr.sprite = _sprites[2];
        }
        else if (above&&!left&&right&&below)
        {
            sr.sprite = _sprites[3];
        }else if (above&&left&&right&&below)
        {
            sr.sprite = _sprites[4];
        }else if (above&&left&&!right&&below)
        {
            sr.sprite = _sprites[5];
        }else if (above&&!left&&right&&!below)
        {
            sr.sprite = _sprites[6];
        }else if (above&&left&&right&&!below)
        {
            sr.sprite = _sprites[7];
        }else if (above&&left&&!right&&!below)
        {
            sr.sprite = _sprites[8];
        }
        /*else if (right)
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
        }*/
    }
}
