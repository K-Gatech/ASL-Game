using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] replies;
    public int correctReplyIndex;
    public Sprite questionImage;
    public VideoClip questionVideoClip;
}

[CreateAssetMenu(fileName = "New Category", menuName = "Quiz/Question Data")]
public class QuestionData : ScriptableObject
{
    public string category;
    public Question[] questions;
}

// youtube video used to create the object that allowed us to create seperate questions directly through unity
// lines 5 - 19
// https://www.youtube.com/watch?v=xKyWwn5sQ0g
