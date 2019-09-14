using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManStatus : MonoBehaviour
{ 
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ghost":
                GetComponent<PacManCornerMovement>().dead = true;
                GetComponent<PacManAnimator>().PlayDeath();

                foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
                {
                    ghost.GetComponent<GhostMovement>().enabled = false;
                }

                break;
            case "Pellet":
                break;
            case "Power Pellet":
                break;
        }
    }
}
