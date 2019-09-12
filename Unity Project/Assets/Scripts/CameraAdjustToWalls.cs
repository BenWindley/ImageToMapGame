using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustToWalls : MonoBehaviour
{
    MapGenerator generator;
    public Bounds screen_region;

	void Start ()
    {
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<MapGenerator>();

        Vector3 max = Vector3.zero;
        Vector3 min = Vector3.zero;

        foreach(GameObject wall in generator.walls)
        {
            Bounds wall_bounds = wall.GetComponent<BoxCollider>().bounds;

            max = Vector3.Max(max, wall_bounds.max);
            min = Vector3.Min(min, wall_bounds.min);
        }

        Bounds map_bounds = new Bounds();

        map_bounds.max = max;
        map_bounds.min = min;

        float screen_ratio = Screen.width / (float)Screen.height;
        float target_ratio = map_bounds.size.x / map_bounds.size.y;

        if (screen_ratio >= target_ratio)
        {
            Camera.main.orthographicSize = map_bounds.size.y / 2;
        }
        else
        {
            float size_difference = target_ratio / screen_ratio;
            Camera.main.orthographicSize = map_bounds.size.y / 2 * size_difference;
        }

        transform.position = new Vector3(map_bounds.center.x, map_bounds.center.y, transform.position.z);

        SetScreenRegion();
    }

    void SetScreenRegion()
    {
        Bounds region = new Bounds();

        float height = 2.0f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        region.min = new Vector2(Camera.main.transform.position.x - (width * 0.5f) - transform.localScale.x * 0.5f,
                                        Camera.main.transform.position.y - (height * 0.5f) - transform.localScale.y * 0.5f);

        region.max = new Vector2(Camera.main.transform.position.x + (width * 0.5f) + transform.localScale.x * 0.5f,
                                        Camera.main.transform.position.y + (height * 0.5f) + transform.localScale.y * 0.5f);

        screen_region = region;
    }
}
