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
    private Vector3 originalScale;
    private Vector3 selectedScale;

    void Start()
    {
        
        if (levelButtons.Length > 0)
        {
            originalScale = levelButtons[0].transform.localScale;
            selectedScale = originalScale * 1.2f; 
        }

        UpdateLevelSelection();
    }

    void Update()
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
                
                levelButtons[i].transform.localScale = selectedScale;
            }
            else
            {
                
                levelButtons[i].transform.localScale = originalScale;
            }
        }
    }

    void LoadSelectedLevel()
    {
        
        SceneManager.LoadScene(levelNames[currentIndex]);
    }
}
