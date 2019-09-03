using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<Texture2D> maps;
    public Texture2D map;
    public ImageReader.types[] types_array;

    public Color void_colour;
    public Color wall_colour;
    public Color pac_colour;
    public Color ghost_colour;


    void Start ()
    {
		if(maps.Count > 0)
        {
            map = maps[Random.Range(0, maps.Count)];

            types_array = ImageReader.GetImageData(
                map,
                void_colour,
                wall_colour,
                pac_colour,
                ghost_colour
                );
        }
        else
        {
            Debug.Log("No Maps");
        }
	}

	void Update ()
    {
		
	}
}
