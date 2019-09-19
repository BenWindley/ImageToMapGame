﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncrease : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Pac-Man")
        {
            // Increase Score
            Camera.main.GetComponent<Score>().dots++;
            // Check end game
            Destroy(gameObject);
        }
    }
}
