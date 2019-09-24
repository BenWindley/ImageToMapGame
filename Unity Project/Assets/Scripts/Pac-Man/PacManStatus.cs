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
                if (collision.gameObject.GetComponent<GhostMovement>().state == GhostMovement.State.RUN)
                {
                    collision.gameObject.GetComponent<GhostMovement>().Die();
                }
                else
                {
                    GetComponent<PacManCornerMovement>().dead = true;
                    GetComponent<PacManAnimator>().PlayDeath();
                }
                break;
        }
    }
}
