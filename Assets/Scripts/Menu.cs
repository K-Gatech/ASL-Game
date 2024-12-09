using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Level2_locked;   // Locked UI for Level 2
    public GameObject Level2_unlocked; // Unlocked UI for Level 2
    public GameObject Level3_locked;   // Locked UI for Level 3
    public GameObject Level3_unlocked; // Unlocked UI for Level 3
    public GameObject QuitPopUp;  //Pop up to quit

    public static bool isVisible = false; //Check if pop up is visible

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Reset levels to locked state
            PlayerPrefs.SetInt("Level2_Unlocked", 0);
            PlayerPrefs.SetInt("Level3_Unlocked", 0);
            PlayerPrefs.Save(); // Ensure changes are saved
            Debug.Log("Reset levels to locked state.");
        }

        // Update UI based on PlayerPrefs values in the Levels scene
        if (SceneManager.GetActiveScene().name == "Levels")
        {
            UpdateLevelUI();
        }
        
        if (QuitPopUp != null)
        {
            QuitPopUp.SetActive(false);
            isVisible = false;
        }
    }
    
    public void ShowPopUp()
    {
        if (QuitPopUp != null)
        {
            QuitPopUp.SetActive(true); //show pop up
            isVisible = true;
        }
    }

    public void HidePopUp()
    {
        if (QuitPopUp != null)
        {
            QuitPopUp.SetActive(false); //show pop up
            isVisible = false;
        }
    }


    void UpdateLevelUI()
    {
        // Default state: All levels locked except Level 1
        Level2_unlocked.SetActive(false);
        Level2_locked.SetActive(true);

        Level3_unlocked.SetActive(false);
        Level3_locked.SetActive(true);

        Debug.Log("Default state set. Level3_locked active, Level3_unlocked inactive.");

        // Check PlayerPrefs for Level 2
        int level2State = PlayerPrefs.GetInt("Level2_Unlocked", 0);
        if (level2State == 1)
        {
            Level2_unlocked.SetActive(true);
            Level2_locked.SetActive(false);
            Debug.Log("Level 2 UI updated to unlocked.");
        }

        // Check PlayerPrefs for Level 3
        int level3State = PlayerPrefs.GetInt("Level3_Unlocked", 0);
        if (level3State == 1)
        {
            Level3_unlocked.SetActive(true);
            Level3_locked.SetActive(false);
            Debug.Log("Level 3 is unlocked!");
        }
        else
        {
            Debug.Log("Level 3 is still locked.");
        }


        // Debug the active states
        Debug.Log("Level3_locked active: " + Level3_locked.activeSelf);
        Debug.Log("Level3_unlocked active: " + Level3_unlocked.activeSelf);
    }




    public void Level1()
    {
        // Start Level 1
        SceneManager.LoadScene("EasyLevel");

    }

    public void Level2()
    {
        // Allow Level 2 only if it is unlocked
        if (Level2_unlocked.activeSelf)
        {
            Debug.Log("Starting Level 2...");
            SceneManager.LoadScene("MediumLevel");
        }
        else
        {
            Debug.Log("Level 2 is locked!");
        }
    }

    public void Level3()
    {
        // Allow Level 3 only if it is unlocked
        if (Level3_unlocked.activeSelf)
        {
            Debug.Log("Starting Level 3...");
            SceneManager.LoadScene("HardLevel");
        }
        else
        {
            Debug.Log("Level 3 is locked!");
        }
    }

    public void Level3Restart()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>(); // restarts score on level reset
        scoreManager.ResetScore();
        SceneManager.LoadScene("HardLevel");

    }

    public void Level1Restart()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>(); // restarts score on level reset
        scoreManager.ResetScore();
        SceneManager.LoadScene("EasyLevel");

    }

    public void Level2Restart()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>(); // restarts score on level reset
        scoreManager.ResetScore();
        SceneManager.LoadScene("MediumLevel");

    }

    public void MainMenu()
    {
        // Return to Main Menu and reset the score
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void levels()
    {
        SceneManager.LoadScene("Levels");
    }

    public void Exit()
    {
        // Quit the application
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}


// Transitioning back and forth between screens via button clicks and choose levels
//https://stackoverflow.com/questions/70196826/how-to-load-next-scene-when-pressed-a-button-while-playing-in-unity
//https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//https://www.youtube.com/watch?v=2XQsKNHk1vk&ab_channel=RehopeGames
//https://www.youtube.com/watch?v=vpbPd6jNEBs&ab_channel=Vinny
