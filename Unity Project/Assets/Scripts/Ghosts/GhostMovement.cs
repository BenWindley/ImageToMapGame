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

    public bool active = true;

    public Type type = Type.BLINKY;
    public State state = State.CHASE;
    public Vector3 scatter_destination = Vector3.zero;
    public Vector3 spawn_location = Vector3.zero;
    public Vector3 destination = Vector2.zero;
    public AudioClip death_clip;

    private Vector3 map_offset = Vector3.zero;
    private MapGenerator generator;
    private Pathfinder path;

    private PacManCornerMovement pac_man;
    private GhostAnimator anim;

    public float initial_timer = 0.0f;
    public float activation_timer = 0.0f;
    public int pinky_steps = 2;
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

        pac_man.GetComponent<PacManAnimator>().audio_source.clip = pac_man.GetComponent<PacManAnimator>().scared_clip;

        Camera.main.GetComponent<Score>().ResetStreak();

        CancelInvoke("StopRun");
        Invoke("StopRun", 8.0f);
    }

    public void StopRun()
    {
        state = State.CHASE;
        speed = initial_speed;

        pac_man.GetComponent<PacManAnimator>().audio_source.clip = pac_man.GetComponent<PacManAnimator>().waka_clip;

        anim.scared = false;
    }

    public void Die()
    {
        anim.dead = true;
        GetComponent<BoxCollider>().enabled = false;

        CancelInvoke("StopRun");
        speed = death_speed;
        AudioSource.PlayClipAtPoint(death_clip, Vector3.zero);

        Camera.main.GetComponent<Score>().Eat();

        state = State.DEAD;
    }

    public void Reset()
    {
        speed = initial_speed;
        anim.scared = false;
        anim.dead = false;
        GetComponent<BoxCollider>().enabled = true;
        scatter_destination = GetRandomFreeTile();
        activation_timer = initial_timer;

        CancelInvoke("StopRun");

        transform.position = spawn_location - generator.offset;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        destination = transform.position;

        state = State.SCATTER;
    }

    private void Start()
    {
        destination = transform.position;
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<MapGenerator>();
        path = GameObject.FindGameObjectWithTag("Generator").GetComponent<Pathfinder>();
        pac_man = GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>();
        anim = GetComponent<GhostAnimator>();

        initial_speed = speed;
        initial_timer = activation_timer;
        scatter_destination = GetRandomFreeTile();
    }

    void Update()
    {
        if (!active)
            return;
        if (pac_man.dead)
            return;

        activation_timer -= Time.deltaTime;

        if (activation_timer > 0.0f)
            return;
        
        switch (state)
        {
            case State.SCATTER:
                if (transform.position == destination)
                {
                    destination = CalculateDestination(scatter_destination);
                }
                if (destination == scatter_destination - map_offset)
                {
                    state = State.CHASE;
                }
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
                            destination = CalculateDestination(pac_man.GetCurrentTile(), true);
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
                    anim.scared = false;
                    anim.dead = false;
                    GetComponent<BoxCollider>().enabled = true;

                    state = State.CHASE;
                }
                break;
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private Vector3 CalculateDestination(Vector3 target, bool check_in_front = false)
    {
        Vector3 move;

        if (check_in_front)
        {
            move = path.CalculateNextMoveInFrontOfTarget(
                transform.position + generator.offset, target, (Pathfinder.Directions)pac_man.current_direction, pinky_steps
                );

            if(move == Vector3.zero)
            {
                move = path.CalculateNextMove(transform.position + generator.offset, target);
            }
        }
        else
        { 
            move = path.CalculateNextMove(transform.position + generator.offset, target);
        }

        move -= generator.offset + new Vector3(0, 0, transform.position.z);

        return move;
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
