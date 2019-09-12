using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{
    public float speed = 1.0f;
    private PacManAnimator animator;
    private bool first_move = true;
    public Vector3 offset;

    public bool dead = false;

	void Start ()
    {
        animator = GetComponent<PacManAnimator>();
        animator.moving = false;
        transform.position = RoundPosition();
	}

	void Update ()
    {
        if(dead)
        {
            return;
        }

		if(Input.GetButtonDown("Left") && (animator.direction != PacManAnimator.Directions.LEFT || first_move))
        {
            // Left
            animator.direction = PacManAnimator.Directions.LEFT;
            animator.moving = true;
            first_move = false;
            transform.position = RoundPosition();
        }
        else if (Input.GetButtonDown("Right") && (animator.direction != PacManAnimator.Directions.RIGHT || first_move))
        {
            // Right
            animator.direction = PacManAnimator.Directions.RIGHT;
            animator.moving = true;
            first_move = false;
            transform.position = RoundPosition();
        }
        else if (Input.GetButtonDown("Up") && (animator.direction != PacManAnimator.Directions.UP || first_move))
        {
            // Up
            animator.direction = PacManAnimator.Directions.UP;
            animator.moving = true;
            transform.position = RoundPosition();
        }
        else if (Input.GetButtonDown("Down") && (animator.direction != PacManAnimator.Directions.DOWN || first_move))
        {
            // Down
            animator.direction = PacManAnimator.Directions.DOWN;
            animator.moving = true;
            first_move = false;
            transform.position = RoundPosition();
        }

        if (animator.moving)
        {
            switch (animator.direction)
            {
                case PacManAnimator.Directions.UP:
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                    break;
                case PacManAnimator.Directions.LEFT:
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                    break;
                case PacManAnimator.Directions.DOWN:
                    transform.Translate(-Vector3.up * speed * Time.deltaTime);
                    break;
                case PacManAnimator.Directions.RIGHT:
                    transform.Translate(-Vector3.left * speed * Time.deltaTime);
                    break;
            }
        }

        LoopPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        if(collision.gameObject.tag == "Wall")
        {
            // Hit wall

            animator.moving = false;
            transform.position = RoundPosition();
        }
        else if(collision.gameObject.tag == "Ghost")
        {
            // Gameover

            GetComponent<PacManStatus>().dead = true;
            dead = true;
            animator.moving = false;
        }
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
}
