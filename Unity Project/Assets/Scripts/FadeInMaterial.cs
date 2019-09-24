using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInMaterial : MonoBehaviour
{
    public Material material;

    public float speed;

    public bool active = false;
    public float timer = 0.0f;
    public float delay = 0.0f;

    void Start()
    {
        material.color = new Color(1, 1, 1, 0.0f);
    }

    void Update ()
    {
        if (delay > 0.0f)
        {
            delay -= Time.deltaTime;
            return;
        }
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime * speed;

            material.color = new Color(1, 1, 1, 1.0f - timer);
        }
        else
        {
            material.color = new Color(1, 1, 1, 1.0f - timer);

            Destroy(this);
        }
	}
}
