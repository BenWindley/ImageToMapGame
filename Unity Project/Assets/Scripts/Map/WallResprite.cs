using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallResprite : MonoBehaviour
{
    public bool inside;

    public void RecalculateWalls()
    {
        bool up = CheckDirection(Vector3.up);
        bool right = CheckDirection(Vector3.right);
        bool down = CheckDirection(Vector3.down);
        bool left = CheckDirection(Vector3.left);

        string type =
            (up ? "o" : "x") +
            (right ? "o" : "x") +
            (down ? "o" : "x") +
            (left ? "o" : "x");

        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Map_" + type);
    }

    private bool CheckDirection(Vector3 dir)
    {
        bool result = Physics.Raycast(transform.position, dir, 1.0f);

        RaycastHit info;

        if (!Physics.Raycast(transform.position, dir, out info) && !inside)
        {
            result = true;
        }

        return result;
    }
}
