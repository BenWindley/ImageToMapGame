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
        GHOSTSPAWN,
        PELLET,
        POWERPELLET
    }

    public static types[,] GetImageData(
        Texture2D image,
        Color wall_col,
        Color pac_col,
        Color ghost_col,
        Color pellet_col,
        Color power_pellet_col)
    {
        types[,] image_data = new types[image.width, image.height];

        for(int y = 0; y < image.height; ++y)
        {
            for(int x = 0; x < image.width; ++x)
            {
                if(image.GetPixel(x, y) == wall_col)
                {
                    image_data[x, y] = types.WALL;
                }
                else if (image.GetPixel(x, y) == pac_col)
                {
                    image_data[x, y] = types.PACSPAWN;
                }
                else if (image.GetPixel(x, y) == ghost_col)
                {
                    image_data[x, y] = types.GHOSTSPAWN;
                }
                else if (image.GetPixel(x, y) == pellet_col)
                {
                    image_data[x, y] = types.PELLET;
                }
                else if (image.GetPixel(x, y) == power_pellet_col)
                {
                    image_data[x, y] = types.POWERPELLET;
                }
                else
                {
                    image_data[x, y] = types.VOID;
                }
            }
        }

        return image_data;
    }
}
