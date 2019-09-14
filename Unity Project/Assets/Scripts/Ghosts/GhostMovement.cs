using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public enum State
    {
        START,
        CHASE
    }

    public State state = State.CHASE;
    public Vector3 destination = Vector2.zero;
    private MapGenerator generator;
    private Pathfinder path;

    private PacManCornerMovement pac_man;

    public float speed = 1.0f;

    private void Start()
    {
        destination = transform.position;
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<MapGenerator>();
        path = GameObject.FindGameObjectWithTag("Generator").GetComponent<Pathfinder>();
        pac_man = GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>();
    }

    void Update ()
    {
        switch (state)
        {
            case State.START:

                break;
            case State.CHASE:
                if(transform.position == destination)
                {
                    destination =
                        (Vector3) path.CalculateNextMove(transform.position + generator.offset, pac_man.GetCurrentTile()) -
                        generator.offset +
                        new Vector3(0,0,transform.position.z);
                }
                break;
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }
}
