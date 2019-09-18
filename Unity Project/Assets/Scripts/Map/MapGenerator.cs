using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<Texture2D> maps;
    public Texture2D map;
    public ImageReader.types[,] types_array;

    public Color wall_colour;
    public Color pac_colour;
    public Color ghost_colour;
    public Color pellet_colour;
    public Color power_pellet_colour;

    public GameObject wall_prefab;
    public GameObject pellet_prefab;
    public GameObject power_pellet_prefab;
	public GameObject ghost_spawn;
    public GameObject void_prefab;

    public List<GameObject> walls;

    public Vector3 offset;

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
        offset = new Vector3(map.width / 2.0f, map.height / 2.0f, 0.0f);

        for(int i = 0; i < types_array.Length; ++i)
        {
            int x = i % map.width;
            int y = i / (map.height * 2);

            Vector3 position = new Vector3(x, y, 0) - offset;

            switch (types_array[x, y])
            {
                case ImageReader.types.VOID:

                    break;
                case ImageReader.types.WALL:
                    walls.Add(Instantiate(wall_prefab, position, Quaternion.identity, transform));
                    walls[walls.Count - 1].name = "Wall " + i;

                    walls[walls.Count - 1].GetComponent<WallResprite>().inside =
                        !((x == 0 || x == map.width - 1) ||
                         (y == 0 || y == map.height - 1));

                    break;
                case ImageReader.types.PACSPAWN:
                    GameObject.FindGameObjectWithTag("Pac-Man").transform.position =
                        position +
                        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>().offset;
                    break;
                case ImageReader.types.GHOSTSPAWN:
					GameObject spawn = Instantiate(ghost_spawn, position, Quaternion.identity, transform);

					foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
					{
						if (ghost.GetComponent<GhostMovement>().spawn_location == Vector3.zero)
						{
							ghost.transform.position = position;
							ghost.GetComponent<GhostMovement>().spawn_location = spawn.transform.position + offset;
						} 
					}

					break;
                case ImageReader.types.PELLET:
                    Instantiate(pellet_prefab, position, Quaternion.identity, transform);
                    break;
                case ImageReader.types.POWERPELLET:
                    Instantiate(power_pellet_prefab, position, Quaternion.identity, transform);
                    break;
            }

            if (types_array[x, y] != ImageReader.types.WALL)
            {
                Instantiate(void_prefab, position, Quaternion.identity, transform);
            }
        }

        foreach(GameObject wall in walls)
        {
            wall.GetComponent<WallResprite>().RecalculateWalls();
        }
    }
}
