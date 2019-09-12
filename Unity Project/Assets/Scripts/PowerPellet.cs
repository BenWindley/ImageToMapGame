using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Pac-Man")
        {
            // Change to power mode
            // Set ghosts to run
            Destroy(gameObject);
        }
    }
}
