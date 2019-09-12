using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<Texture2D> maps;
    public Texture2D map;
    public ImageReader.types[] types_array;

    public Color wall_colour;
    public Color pac_colour;
    public Color ghost_colour;
    public Color pellet_colour;
    public Color power_pellet_colour;

    public GameObject wall_prefab;
    public GameObject pellet_prefab;
    public GameObject power_pellet_prefab;

    public List<GameObject> walls;

    void Awake ()
    {
		if(maps.Count > 0)
        {
            map = maps[Random.Range(0, maps.Count)];

            types_array = ImageReader.GetImageData(
                map,
                wall_colour,
                pac_colour,
                ghost_colour,
                pellet_colour,
                power_pellet_colour
                );
        }
        else
        {
            Debug.Log("No Maps");
        }

        // Initiate map
        // Centers the map to the camera
        Vector3 offset = new Vector3(map.width / 2.0f, map.height / 2.0f, 0.0f);

        for(int i = 0; i < types_array.Length; ++i)
        {
            Vector3 position = new Vector3(i % map.width, i / (map.height * 2), 0) - offset;

            switch (types_array[i])
            {
                case ImageReader.types.VOID:

                    break;
                case ImageReader.types.WALL:
                    walls.Add(Instantiate(wall_prefab, position, Quaternion.identity, transform));
                    walls[walls.Count - 1].name = "Wall " + i;
                    break;
                case ImageReader.types.PACSPAWN:
                    GameObject.FindGameObjectWithTag("Pac-Man").transform.position =
                        position +
                        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>().offset;
                    break;
                case ImageReader.types.GHOSTSPAWN:

                    break;
                case ImageReader.types.PELLET:
                    Instantiate(pellet_prefab, position, Quaternion.identity, transform);
                    break;
                case ImageReader.types.POWERPELLET:
                    Instantiate(power_pellet_prefab, position, Quaternion.identity, transform);
                    break;
            }
        }

        foreach(GameObject wall in walls)
        {
            wall.GetComponent<WallResprite>().RecalculateWalls();
        }
    }
}
