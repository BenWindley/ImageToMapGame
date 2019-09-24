using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int dots = 0;
    public int additional_score = 0;
    public Text score_text;
    public string initial_text;

    public int eat_streak = 0;

    public void ResetStreak()
    {
        eat_streak = 0;
    }

    public void Eat()
    {
        switch(eat_streak++)
        {
            case 0:
                additional_score += 200;
                break;
            case 1:
                additional_score += 400;
                break;
            case 2:
                additional_score += 800;
                break;
            case 3:
                additional_score += 1600;
                break;
        }
    }

	void Start ()
    {
        initial_text = score_text.text;
	}

	void Update ()
    {
        int score = 10 * dots + additional_score;
        score_text.text = initial_text + score;
	}
}
