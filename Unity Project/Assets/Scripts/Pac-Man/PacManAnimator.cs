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

    public List<Sprite> death_sprites;

    public bool dead = false;
    public float dead_counter = 0.0f;
    public float dead_speed = 1.0f;

    public bool alt_sprite = false;
    public bool moving = false;

    public float animation_timer = 0.0f;
    public float animation_speed = 0.5f;

    public Directions direction = Directions.LEFT;

    public AudioSource audio_source;
    public AudioClip death_clip;
    public AudioClip scared_clip;
    public AudioClip waka_clip;

    void Start ()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        audio_source = GetComponent<AudioSource>();
	}

    public void PlayDeath()
    {
        dead = true;
        moving = false;

        audio_source.Stop();
        AudioSource.PlayClipAtPoint(death_clip, transform.position);
    }

    public void Reset()
    {
        dead = false;
        direction = Directions.RIGHT;
        moving = false;

        sprite_renderer.sprite = left_sprites[0];

        dead_counter = 0.0f;
    }

    void LateUpdate ()
    {
        if (!GetComponent<PacManCornerMovement>().active)
        {
            audio_source.Stop();
            return;
        }
        if(dead)
        {
            dead_counter += Time.deltaTime * dead_speed;
            sprite_renderer.sprite = death_sprites[Mathf.Clamp((int) Mathf.Floor(dead_counter), 0, death_sprites.Count - 1)];

            if(dead_counter >= death_sprites.Count - 1)
            {
                Camera.main.GetComponent<Manager>().EndSequence();
            }

            return;
        }

        if(moving)
        {
            animation_timer += Time.deltaTime * animation_speed;

            alt_sprite = Mathf.RoundToInt(Mathf.PingPong(animation_timer, 1.0f)) == 1;

            if(!audio_source.isPlaying)
            {
                audio_source.Play();
            }
        }
        else
        {
            animation_timer = 0;

            alt_sprite = false;

            if (audio_source.isPlaying)
            {
                audio_source.Stop();
            }
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