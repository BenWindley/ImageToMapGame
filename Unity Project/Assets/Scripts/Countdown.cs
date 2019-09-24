using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float timer = 0.0f;
    public Text text;

    public void StartTimer()
    {
        timer = 4.0f;
    }

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update ()
    {
        if (timer < 0)
            return;

        timer -= Time.deltaTime;

        switch((int) Mathf.Ceil(timer))
        {
            case 4:
                text.color = Color.white;
                text.text = "3";
                break;
            case 3:
                text.text = "2";
                break;
            case 2:
                text.text = "1";
                break;
            case 1:
                text.text = "START";
                break;
        }
        if (timer <= 1.0f)
        {
            text.color = new Color(1, 1, 1, timer);
        }
	}
}
