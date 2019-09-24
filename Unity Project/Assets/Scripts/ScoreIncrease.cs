using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncrease : MonoBehaviour
{
    private void Start()
    {
        Camera.main.GetComponent<Manager>().dots++;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Pac-Man")
        {
            // Increase Score
            Camera.main.GetComponent<Score>().dots++;
            Camera.main.GetComponent<Manager>().DecreaseDots();

            Destroy(gameObject);
        }
    }
}
