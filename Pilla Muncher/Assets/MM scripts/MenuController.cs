using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject[] levelButtons; 
    public TextMeshProUGUI levelNameText; 
    public string[] levelNames; 

    public int columns = 4; 

    private int currentIndex = 0;

    void Start()
    {
        UpdateLevelSelection();
    }

    void Update()// change accordingly if needed 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex -= columns;
            if (currentIndex < 0) currentIndex += levelButtons.Length;
            UpdateLevelSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex += columns;
            if (currentIndex >= levelButtons.Length) currentIndex -= levelButtons.Length;
            UpdateLevelSelection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = levelButtons.Length - 1;
            UpdateLevelSelection();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex++;
            if (currentIndex >= levelButtons.Length) currentIndex = 0;
            UpdateLevelSelection();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            LoadSelectedLevel();
        }
    }

    void UpdateLevelSelection()
    {
        
        levelNameText.text = levelNames[currentIndex];

        
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i == currentIndex)
            {
                // change colour if you want 
                // instead of colour changing, make the button bigger 
                levelButtons[i].GetComponent<Image>().color = Color.gray;
            }
            else
            {
                // again change colour if you want <3
                // here have all the buttons of a smaller size 
                levelButtons[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    void LoadSelectedLevel()
    {
       
        SceneManager.LoadScene(levelNames[currentIndex]);
    }
}
