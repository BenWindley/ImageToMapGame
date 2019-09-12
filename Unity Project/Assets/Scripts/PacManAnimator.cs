using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManAnimator : MonoBehaviour
{
    public enum Directions
    {
        NONE = -1,
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    private SpriteRenderer sprite_renderer;

    public List<Sprite> up_sprites;
    public List<Sprite> left_sprites;
    public List<Sprite> down_sprites;
    public List<Sprite> right_sprites;

    public bool alt_sprite = false;
    public bool moving = false;

    public float animation_timer = 0.0f;
    public float animation_speed = 0.5f;

    public Directions direction = Directions.LEFT;

    void Start ()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
	}
	
	void LateUpdate ()
    {
        if(moving)
        {
            animation_timer += Time.deltaTime * animation_speed;

            alt_sprite = Mathf.RoundToInt(Mathf.PingPong(animation_timer, 1.0f)) == 1;
        }
        else
        {
            animation_timer = 0;

            alt_sprite = false;
        }

        switch (direction)
        {
            case Directions.UP:
                sprite_renderer.sprite = up_sprites[alt_sprite ? 1 : 0];
                break;
            case Directions.LEFT:
                sprite_renderer.sprite = left_sprites[alt_sprite ? 1 : 0];
                break;
            case Directions.DOWN:
                sprite_renderer.sprite = down_sprites[alt_sprite ? 1 : 0];
                break;
            case Directions.RIGHT:
                sprite_renderer.sprite = right_sprites[alt_sprite ? 1 : 0];
                break;
        }
    }
}