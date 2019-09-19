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
