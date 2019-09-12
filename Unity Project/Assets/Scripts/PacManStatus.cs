using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManStatus : MonoBehaviour
{
    public bool dead = false;
    private float death_timer = 0.0f;
    public float time_to_death = 2.0f;

    public float initial_scale = 1.0f;
    public AnimationCurve death_size;

    public GameObject death_particle;
    
	void Start ()
    {
        initial_scale = transform.localScale.x;
	}
	
	void Update ()
    {
		if(dead)
        {
            death_timer += Time.deltaTime;

            transform.parent.position += transform.position;
            transform.position = Vector3.zero;

            transform.GetComponent<BoxCollider>().enabled = false;

            transform.localScale = Vector3.one * (initial_scale + death_size.Evaluate(death_timer / time_to_death));

            if(death_timer > time_to_death)
            {
                death_particle.GetComponent<ParticleSystem>().Play();
                death_particle.transform.parent = null;
                Destroy(death_particle, 5.0f);
                Destroy(gameObject);
            }
        }
	}
}
