using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect : MonoBehaviour
{
    public Vector3 offset;
    public float resolution_ppu = 16;

	void Update ()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x / resolution_ppu) * resolution_ppu,
            Mathf.Round(transform.position.y / resolution_ppu) * resolution_ppu,
            Mathf.Round(transform.position.z / resolution_ppu) * resolution_ppu);
	}
}
