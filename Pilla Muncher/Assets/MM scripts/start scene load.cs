using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startsceneload : MonoBehaviour
{
    public int sceneNum = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (sceneNum == 0)
            {
                SceneManager.LoadScene("Tutorial Scene");
            }
            else
            {
                SceneManager.LoadScene("Main Menu");
            }

        }
    }
}
