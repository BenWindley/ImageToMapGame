using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimator : MonoBehaviour
{
    public enum Directions
    {
        NONE = -1,
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    private Vector3 previous_position = Vector3.zero;

    public bool moving = true;
    public bool scared = false;
    public bool dead = false;

    public List<Sprite> up_sprites;
    public List<Sprite> left_sprites;
    public List<Sprite> down_sprites;
    public List<Sprite> right_sprites;

    public List<Sprite> scared_sprites;

    public List<Sprite> dead_sprites;

    public bool alt_sprite;
    public bool alt_scared;

    public float timer = 0.0f;
    public float animation_speed = 1.0f;
    public float scared_speed = 0.5f;

    public SpriteRenderer sprite;
    public PacManAnimator pac_anim;

    public void Reset()
    {
        timer = 0.0f;

        sprite.sprite = left_sprites[0];

        moving = true;
        scared = false;
        dead = false;
    }

    private void Start()
    {
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        previous_position = transform.position;
        pac_anim = GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManAnimator>();
    }

    void LateUpdate ()
    {
        if (pac_anim.dead)
            return;
        if (!GetComponent<GhostMovement>().active)
            return;

        if(GetComponent<GhostMovement>().activation_timer > 0)
        {
            transform.GetChild(0).transform.localPosition = Vector3.up * Mathf.PingPong(Time.time * GetComponent<GhostMovement>().speed, 0.5f);
        }
        else
        {
            transform.GetChild(0).transform.localPosition = Vector3.MoveTowards(
                transform.GetChild(0).transform.localPosition,
                Vector3.zero,
                Time.deltaTime * 0.5f
                );
        }

        timer += Time.deltaTime;
        alt_sprite = Mathf.RoundToInt(Mathf.PingPong(timer, 1.0f)) == 1;
        alt_scared = Mathf.RoundToInt(Mathf.PingPong(timer * scared_speed, 1.0f)) == 1;

        if (dead)
        {
            sprite.sprite = dead_sprites[(int)GetCurrentDirection()];
        }
        else if (scared)
        {
            sprite.sprite = scared_sprites[(alt_sprite ? 1 : 0) + (alt_scared ? 2 : 0)];
        }
        else
        {
            switch (GetCurrentDirection())
            {
                case Directions.NONE:
                    goto case Directions.LEFT;
                case Directions.UP:
                    sprite.sprite = up_sprites[alt_sprite ? 0 : 1];
                    break;
                case Directions.RIGHT:
                    sprite.sprite = right_sprites[alt_sprite ? 0 : 1];
                    break;
                case Directions.DOWN:
                    sprite.sprite = down_sprites[alt_sprite ? 0 : 1];
                    break;
                case Directions.LEFT:
                    sprite.sprite = left_sprites[alt_sprite ? 0 : 1];
                    break;
            }
        }

        previous_position = transform.position;
    }

    public Directions GetCurrentDirection()
    {
        if (transform.position.x > previous_position.x)
        {
            return Directions.RIGHT;
        }

        if (transform.position.x < previous_position.x)
        {
            return Directions.LEFT;
        }

        if (transform.position.y > previous_position.y)
        {
            return Directions.UP;
        }

        if (transform.position.y < previous_position.y)
        {
            return Directions.DOWN;
        }

        return Directions.NONE;
    }
}
