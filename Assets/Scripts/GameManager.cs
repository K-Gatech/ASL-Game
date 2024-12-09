using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class GameManager : MonoBehaviour
{
    //Reference Menu
    public Menu menuScript;
    // Array to hold all categories 
    public QuestionData[] categories;

    // Reference to the selected category
    private QuestionData selectedCategory;

    // Index to track the current question within the selected category
    private int currentQuestionIndex = 0;

    // UI elements to display the question text, image, and reply buttons
    public TMP_Text questionText;
    public Image questionImage;
    public VideoPlayer questionVideo;
    public Button[] replyButtons;
    private Color StartColor;

    [Header("Score")]
    public ScoreManager score;
    public int correctReply = 1;
    public int wrongReply = 1;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;


    [Header("correctReplyIndex")]
    public int correctReplyIndex;
    int correctReplies;

    [Header("Game Finished Panel")]
    public GameObject GameFinished;
    public GameObject GameProgress;

    [Header("Timer")]
    public TMP_Text timerText;
    public float timerDuration = 10f;
    private float currentTimer;

    //Checker for timer to stop if game is over
    private bool isGameActive = true;


    void Start()
    {
        GameFinished.SetActive(false);
        GameProgress.SetActive(false);
        StartColor = replyButtons[0].GetComponent<Image>().color; // Starting color 
        SelectCategory(0);
        ResetTimer();
    }

    //Stop timer if pop up is visible
    void CheckVisibility()
    {
       if (Menu.isVisible)
       {
            isGameActive = false;
       }
       else
       {    
            isGameActive = true;
       }
    }

    void Update()
    {
        //Check if Quit Pop is visible
        CheckVisibility();
        // Decrease the timer if the game is active
        if (isGameActive && selectedCategory != null && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTimer <= 0)
            {
                EndGame();
            }
        }
    }

    // Method to reset the timer for each question
    void ResetTimer()
    {
        currentTimer = timerDuration;
        UpdateTimerUI();
    }

    // Update the timer display
    void UpdateTimerUI()
    {
        timerText.text = " " + Mathf.CeilToInt(currentTimer).ToString();
    }

    // Method to end the game when the timer runs out
    void EndGame()
    {
        isGameActive = false; // Stop the timer

        // Calculate the percentage of correct answers
        float percentageCorrect = (float)correctReplies / selectedCategory.questions.Length * 100;

        if (percentageCorrect >= 80)
        {
            if (IsLastScene())
            {
                // Show the "Game Finished" screen for the last scene
                GameProgress.SetActive(true);
                scoreText2.text = correctReplies + " / " + selectedCategory.questions.Length;
                Debug.Log("Game completed! Congratulations!");
            }
            else
            {
                // Player passed the level, unlock the next level and return to Levels screen > opens gamepassed
                Debug.Log("Player passed! Unlocking next level.");
                scoreText2.text = correctReplies + " / " + selectedCategory.questions.Length;
                GameProgress.SetActive(true);
                UnlockNextLevel();
                //SceneManager.LoadScene("Levels"); // Return to the Levels scene
            }
        }
        else
        {
            // Player failed, show the "Game Finished" canvas
            GameFinished.SetActive(true);
            scoreText.text = correctReplies + " / " + selectedCategory.questions.Length;
            Debug.Log("Player did not pass.");
        }
    }


    void UnlockNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "EasyLevel") // Unlock Level 2 after Level 1
        {
            PlayerPrefs.SetInt("Level2_Unlocked", 1);
            PlayerPrefs.Save(); // Ensure changes are saved
            Debug.Log("Level 2 unlocked!");
        }
        else if (SceneManager.GetActiveScene().name == "MediumLevel") // Unlock Level 3 after Level 2
        {
            PlayerPrefs.SetInt("Level3_Unlocked", 1);
            PlayerPrefs.Save(); // Ensure changes are saved
            Debug.Log("Level 3 unlocked!");
        }
    }




    // Method to select a category based on the player's choice
    public void SelectCategory(int categoryIndex)
    {
        // Set the selectedCategory 
        selectedCategory = categories[categoryIndex];

        // Reset the current question index to start from the first question in the selected category
        currentQuestionIndex = 0;

        // Display the first question in the selected category
        DisplayQuestion();

        // Reset the timer for the first question
        ResetTimer();
    }

    // Method to display the current question
    void DisplayQuestion()
    {
        // Check if a category has been selected
        if (selectedCategory == null) return;

        // Get the current question from the selected category
        var question = selectedCategory.questions[currentQuestionIndex];

        // Set the question text in the UI
        questionText.text = question.questionText;

        // For showing only an image
        //questionImage.sprite = question.questionImage;

        // set the image or video
        if (question.questionVideoClip != null) // If a video is provided hide image and play video
        {
            questionImage.gameObject.SetActive(false); 
            questionVideo.gameObject.SetActive(true); 
            questionVideo.clip = question.questionVideoClip; 
            questionVideo.Play(); 
        }
        else if (question.questionImage != null) // If an image is provided show image and hide video
        {
            questionImage.gameObject.SetActive(true); 
            questionVideo.gameObject.SetActive(false);
            questionImage.sprite = question.questionImage; 
        }
        
        // Loop through all reply buttons and set their text to the corresponding replies
        for (int i = 0; i < replyButtons.Length; i++)
        {
            // Use TextMeshPro component for reply buttons
            TMP_Text buttonText = replyButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = question.replies[i];
        }

        // Reset the timer for the displayed question
        ResetTimer();
    }

    void LoadNextLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if there is a next scene
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1); // Load the next scene
        }
        else
        {
            Debug.Log("All levels completed. No next scene.");
        }
    }


    // Method to handle when a player selects a reply
    public void OnReplySelected(int replyIndex)
    {
        // Check if the selected reply is correct
        if (replyIndex == selectedCategory.questions[currentQuestionIndex].correctReplyIndex)
        {
            score.AddScore(correctReply);  // Add points for correct answer
            StartCoroutine(AnswerColor(replyButtons[replyIndex], Color.green)); // adds a green color 
            correctReplies++;
            Debug.Log("Correct Reply!");
        }
        else
        {
            score.SubtractScore(wrongReply);  // Subtract points for incorrect answer
            StartCoroutine(AnswerColor(replyButtons[replyIndex], Color.red));  // adds a red color
            Debug.Log("Wrong Reply!");
        }

        // Proceed to the next question or end the quiz if all questions are answered
        currentQuestionIndex++;
        if (currentQuestionIndex < selectedCategory.questions.Length)
        {
            StartCoroutine(NextQuestion()); // wait to display question to show color feedback
        }
        else
        {
            EndGame();
            Debug.Log("Quiz Finished!");
        }
    }

    private bool IsLastScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if the current scene is the last one
        return currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1;
    }


    // question switch delay 
    private IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(0.3f);
        DisplayQuestion();
    }

    // flashing color delay
    private IEnumerator AnswerColor(Button button, Color flashColor)
    {
        Image buttonImage = button.GetComponent<Image>();

        if (buttonImage != null)
        {
            buttonImage.color = flashColor;
            yield return new WaitForSeconds(0.3f);
            buttonImage.color = StartColor;
        }
    }
}


// youtube video used to create the game manager and overall game logic - lines 5 - 143
// https://www.youtube.com/watch?v=xKyWwn5sQ0g

// Delay references for lines 116 - 135 are https://discussions.unity.com/t/quiz-game-tutorial-wait-period-before-the-next-question-appears/724389
// and https://www.youtube.com/watch?app=desktop&v=mtBWaXA96r8

//Refrences for timer and scene transition
//https://stackoverflow.com/questions/53139259/making-a-timer-in-unity
//https://discussions.unity.com/t/how-to-switch-between-scenes/189643
//https://www.youtube.com/watch?v=x9IFMcwqkPY
//https://www.youtube.com/watch?v=gtpXc_9MR6g

//References for video addition
// https://www.youtube.com/watch?v=-XzVq7qIuys&t=50s

//References for Pop Up
//https://www.youtube.com/watch?v=fw78JVgQV2Q&list=PL1R2qsKCcUCIQUinhxPE0gzqoKjlH7ksp&index=2&ab_channel=KreliStudio

// Transitioning back and forth between screens via button clicks and choose levels
//https://stackoverflow.com/questions/70196826/how-to-load-next-scene-when-pressed-a-button-while-playing-in-unity
//https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//https://www.youtube.com/watch?v=2XQsKNHk1vk&ab_channel=RehopeGames
//https://www.youtube.com/watch?v=vpbPd6jNEBs&ab_channel=Vinny

