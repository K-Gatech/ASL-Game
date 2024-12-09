using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int totalScore;  // Static variable to carry score across scenes
    public TextMeshProUGUI scoreText;  // Reference to the TextMeshProUGUI component for displaying the score

    void Start()
    {
        UpdateScoreText();  // Update the score display at the start of each scene
    }

    // Adds points to the current score
    public void AddScore(int points)
    {
        totalScore += points;  // Increase the total score
        UpdateScoreText();  // Update the score display to reflect the new score
    }

    // Subtracts points from the current score
    public void SubtractScore(int points)
    {
        totalScore -= points;  // Decrease the total score
        UpdateScoreText();  // Update the score display to reflect the new score
    }

    // Resets the score to 0
    public void ResetScore()
    {
        totalScore = 0;  // Reset the total score
        UpdateScoreText();  // Update the score display
        Debug.Log("Score reset to 0.");
    }

    // Updates the score display
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = " " + totalScore.ToString();
        }
    }
}
