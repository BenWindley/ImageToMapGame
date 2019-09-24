using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlashText : MonoBehaviour
{
    Text text;

    public bool stop = false;

	void Start ()
    {
        text = GetComponent<Text>();	
	}

    void Update()
    {
        if (stop)
        {
            text.enabled = false;
        }
        else
        {
            text.enabled = Mathf.RoundToInt(Mathf.PingPong(Time.time, 1.0f)) == 1;
        }
    }
}
