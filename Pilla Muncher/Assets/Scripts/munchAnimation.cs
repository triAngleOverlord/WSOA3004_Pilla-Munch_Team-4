using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class munchAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroySelf());
    }

    IEnumerator destroySelf()
    {
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }
}
