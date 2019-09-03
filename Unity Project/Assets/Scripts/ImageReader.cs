using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageReader
{
    public enum types
    {
        VOID,
        WALL,
        PACSPAWN,
        GHOSTSPAWN
    }

    public static types[] GetImageData(
        Texture2D image,
        Color void_col,
        Color wall_col,
        Color pac_col,
        Color ghost_col)
    {
        types[] image_data = new types[image.width * image.height];

        for(int y = 0; y < image.height; ++y)
        {
            for(int x = 0; x < image.width; ++x)
            {
                if(image.GetPixel(x,y) == void_col)
                {
                    image_data[x + y * image.width] = types.VOID;
                }
                else if(image.GetPixel(x, y) == wall_col)
                {
                    image_data[x + y * image.width] = types.WALL;
                }
                else if (image.GetPixel(x, y) == pac_col)
                {
                    image_data[x + y * image.width] = types.PACSPAWN;
                }
                else if (image.GetPixel(x, y) == ghost_col)
                {
                    image_data[x + y * image.width] = types.GHOSTSPAWN;
                }
                else
                {
                    Debug.Log("Unrecognised Colour");

                    image_data[x + y * image.width] = types.VOID;
                }
            }
        }

        return image_data;
    }
}
