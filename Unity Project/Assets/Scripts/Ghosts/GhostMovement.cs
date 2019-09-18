using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public enum Type
    {
        BLINKY,
        INKY,
        PINKY,
        CLYDE
    }
    public enum State
    {
        SCATTER,
        CHASE,
        RUN,
        DEAD
    }

    public Type type = Type.BLINKY;
    public State state = State.CHASE;
    public Vector3 scatter_destination = Vector3.zero;
    public Vector3 spawn_location = Vector3.zero;
    public Vector3 destination = Vector2.zero;
    private Vector3 map_offset = Vector3.zero;
    private MapGenerator generator;
    private Pathfinder path;

    private PacManCornerMovement pac_man;

    public float speed = 1.0f;
    public float death_speed = 2.0f;
    private float initial_speed;

    public void Run()
    {
        if (state == State.DEAD)
            return;

        state = State.RUN;
        speed = initial_speed * 0.25f;
        GetComponent<GhostAnimator>().scared = true;

        Invoke("StopRun", 8.0f);
    }

    public void StopRun()
    {
        state = State.CHASE;
        speed = initial_speed;
        GetComponent<GhostAnimator>().scared = false;
    }

    public void Die()
    {
        GetComponent<GhostAnimator>().dead = true;
        GetComponent<BoxCollider>().enabled = false;

        CancelInvoke("StopRun");
        speed = death_speed;

        state = State.DEAD;
    }

    private void Start()
    {
        destination = transform.position;
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<MapGenerator>();
        path = GameObject.FindGameObjectWithTag("Generator").GetComponent<Pathfinder>();
        pac_man = GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>();

        initial_speed = speed;
        scatter_destination = GetRandomFreeTile();
    }

    void Update ()
    {
        switch (state)
        {
            case State.SCATTER:

                break;
            case State.CHASE:
                switch (type)
                {
                    case Type.BLINKY:
                        // Go to pac man
                        if (transform.position == destination)
                        {
                            destination = CalculateDestination(pac_man.GetCurrentTile());
                        }
                        break;
                    case Type.INKY:
                        // Go to space twice the vector from red to pac-man
                        // OR
                        // Go to random tiles
                        if(transform.position == destination)
                        {
                            if(destination == scatter_destination - map_offset)
                            {
                                scatter_destination = GetRandomFreeTile();
                            }

                            destination = CalculateDestination(scatter_destination);
                        }
                        break;
                    case Type.PINKY:
                        // Goes two spaces in front of pac-man
                        if (transform.position == destination)
                        {
                            destination = CalculateDestination(pac_man.GetTileInFront());
                        }
                        break;
                    case Type.CLYDE:
                        // Aproaches and runs away at a certain distance
                        if(transform.position == destination)
                        {
                            if(Vector3.Distance(pac_man.transform.position, transform.position) < 5.0f)
                            {
                                state = State.SCATTER;
                                scatter_destination = GetRandomFreeTile();
                            }
                            else
                            {
                                destination = CalculateDestination(pac_man.GetCurrentTile());
                            }
                        }
                        break;
                }
                break;
            case State.RUN:
                if (transform.position == destination)
                {
                    destination = CalculateDestination(GetRandomFreeTile());
                }
                break;
            case State.DEAD:
                if (transform.position == destination)
                {
                    destination = CalculateDestination(spawn_location);
                }
                if(transform.position == destination)
                {
                    speed = initial_speed;
                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<GhostAnimator>().scared = false;
                    GetComponent<GhostAnimator>().dead = false;

                    state = State.CHASE;
                }
                break;
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private Vector3 CalculateDestination(Vector3 target, bool can_go_backwards = true)
    {
        return (Vector3)path.CalculateNextMove(transform.position + generator.offset, target) -
               generator.offset +
               new Vector3(0, 0, transform.position.z);
    }

    private Vector3 GetRandomFreeTile()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Void");

        if (map_offset == Vector3.zero)
        {
            map_offset = new Vector3(generator.map.width / 2.0f, generator.map.height / 2.0f, 0.0f);
        }

        return objects[Random.Range(0, objects.Length)].transform.position + map_offset;
    }
}
