using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManCornerMovement : MonoBehaviour
{
    public enum Directions
    {
        NONE = -1,
        UP,
        RIGHT,
        DOWN,
        LEFT
    }

    public Directions current_direction = Directions.NONE;
    public Directions next_direction = Directions.NONE;

    public Vector3 offset;

    private PacManAnimator animator;

    public float speed = 1.0f;

    private void Start()
    {
        animator = GetComponent<PacManAnimator>();
    }

    void Update ()
    {
        ChangeDirection();

        if(next_direction != current_direction)
        {
            if (IsOnTile() && !CheckDirection(next_direction))
            {
                current_direction = next_direction;
                transform.position = RoundPosition();
            }
        }

        if(IsOnTile() && CheckDirection(current_direction))
        {
            current_direction = Directions.NONE;
            transform.position = RoundPosition();
        }

        animator.moving = current_direction != Directions.NONE;

        if(animator.moving)
        {
            animator.direction = (PacManAnimator.Directions)current_direction;
        }

        transform.position += GetDirectionOffset(current_direction);

        LoopPosition();
    }

    private void ChangeDirection()
    {
        if (Input.GetButtonDown("Left"))
        {
            // Left
            next_direction = Directions.LEFT;
        }
        else if (Input.GetButtonDown("Right"))
        {
            // Right
            next_direction = Directions.RIGHT;
        }
        else if (Input.GetButtonDown("Up"))
        {
            // Up
            next_direction = Directions.UP;
        }
        else if (Input.GetButtonDown("Down"))
        {
            // Down
            next_direction = Directions.DOWN;
        }
    }

    private bool IsOnTile()
    {
        if(Vector3.Distance(transform.position + GetDirectionOffset(current_direction), RoundPosition()) < Time.deltaTime * speed)
        {
            return true;
        }

        return false;
    }

    private Vector3 GetDirectionOffset(Directions direction)
    {
        Vector3 d_offset = Vector3.zero;

        switch (direction)
        {
            case Directions.NONE:
                d_offset = Vector3.zero;
                break;
            case Directions.UP:
                d_offset = Vector3.up * Time.deltaTime * speed;
                break;
            case Directions.RIGHT:
                d_offset = Vector3.right * Time.deltaTime * speed;
                break;
            case Directions.DOWN:
                d_offset = - Vector3.up * Time.deltaTime * speed;
                break;
            case Directions.LEFT:
                d_offset = - Vector3.right * Time.deltaTime * speed;
                break;
        }

        return d_offset;
    }

    private bool CheckDirection(Directions direction)
    {
        RaycastHit hit_info;

        bool hit = Physics.Raycast(transform.position - offset, GetDirectionOffset(direction).normalized, out hit_info);

        return hit && hit_info.distance < 1.0f;
    }

    private void LoopPosition()
    {
        Bounds pac_bounds = GetComponent<BoxCollider>().bounds;
        Bounds screen_bounds = Camera.main.GetComponent<CameraAdjustToWalls>().screen_region;

        if (transform.position.x > screen_bounds.max.x)
            transform.position -= Vector3.right * screen_bounds.size.x;
        if (transform.position.x + 0.5f < screen_bounds.min.x)
            transform.position += Vector3.right * screen_bounds.size.x;
        if (transform.position.y > screen_bounds.max.y)
            transform.position -= Vector3.up * screen_bounds.size.y;
        if (transform.position.y + 0.5f < screen_bounds.min.y)
            transform.position += Vector3.up * screen_bounds.size.y;
    }

    private Vector3 RoundPosition()
    {
        return new Vector3(
                Mathf.RoundToInt(transform.position.x + offset.x),
                Mathf.RoundToInt(transform.position.y + offset.y),
                Mathf.RoundToInt(transform.position.z + offset.z)) - offset;
    }

    public Vector2 GetCurrentTile()
    {
        MapGenerator generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<MapGenerator>();

        Vector3 map_offset = new Vector3(generator.map.width / 2.0f, generator.map.height / 2.0f, 0.0f);

        Vector3 position = RoundPosition() + map_offset - offset;

        return position;
    }
}
