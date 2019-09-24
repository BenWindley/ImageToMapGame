using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LivesCounter : MonoBehaviour
{
    public int lives = 3;
    private Text text;
    private string initial_text;

    public void DecreaseLives()
    {
        text.text = initial_text + --lives;
    }

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        initial_text = text.text;
        text.text = initial_text + lives;
    }
}
