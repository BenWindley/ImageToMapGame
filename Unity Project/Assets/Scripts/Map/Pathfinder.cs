﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private bool[,] traversable_map;
    private Texture2D map;

    private void Start()
    {
        map = GetComponent<MapGenerator>().map;

        InitialiseMap(GetComponent<MapGenerator>().wall_colour);
    }
    
    private void InitialiseMap(Color wall_color)
    {
        traversable_map = new bool[map.width, map.height];

        for(int y = 0; y < map.height; ++y)
        {
            for (int x = 0; x < map.width; ++x)
            {
                traversable_map[x, y] = map.GetPixel(x, y) != wall_color;
            }
        }
    }

    public Vector2 CalculateNextMove(Vector2 origin, Vector2 destination)
    {
        // Initialise
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        // Add start node
        open.Add(new Node());

        open[0].position = origin;
        open[0].CalculateF(0, destination);

        Node current_node = open[0];

        if(!traversable_map[(int)current_node.position.x, (int)current_node.position.y])
        {
            Debug.Log("Ghost inside wall");
            return Vector2.zero;
        }

        // Loop until find end
        while (open.Count > 0)
        {
            current_node = open[GetNodeWithLeastF(open)];

            closed.Add(current_node);
            open.Remove(current_node);

            // If node is destination, backtrack to get direction
            if(current_node.position == destination)
            {
                if (current_node.parent != null)
                {
                    while (current_node.parent.parent != null)
                    {
                        current_node = current_node.parent;
                    }
                }

                return current_node.position;
            }

            // Add children to node
            List<Vector2> directions = new List<Vector2>();
            directions.Add(new Vector2( 1,  0));
            directions.Add(new Vector2( 0,  1));
            directions.Add(new Vector2(-1,  0));
            directions.Add(new Vector2( 0, -1));

            List<Node> children = new List<Node>();

            foreach (Vector2 d in directions)
            {
                Node child = new Node();
                child.position = current_node.position + d;
                child.parent = current_node;

                if ((int)child.position.x >= 0 &&
                    (int)child.position.x < map.width &&
                    (int)child.position.y >= 0 &&
                    (int)child.position.y < map.height)
                {
                    if (traversable_map[(int)child.position.x, (int)child.position.y])
                        children.Add(child);
                }
            }

            foreach(Node child in children)
            {
                bool skip = false;
                // Check if child is in closed list
                foreach (Node n in closed)
                {
                    if (child.position == n.position)
                    {
                        skip = true;
                    }
                }

                if(skip)
                {
                    continue;
                }

                child.CalculateF(current_node.f, destination);

                // Check if child is in open list and superior to previous
                foreach (Node n in open)
                {
                    if (child.position == n.position &&
                        child.g > n.g)
                    {
                        skip = true;
                    }
                }

                if (skip)
                {
                    continue;
                }

                open.Add(child);
            }
        }

        return Vector2.zero;
    }

    private int GetNodeWithLeastF(List<Node> open)
    {
        int lowest = 0;

        for(int i = 1; i < open.Count; ++i)
        {
            if (open[lowest].f > open[i].f)
                lowest = i;
        }

        return lowest;
    }
}

public class Node
{
    public float f = 0;
    public float g = 0;
    public float h = 0;

    public Vector2 position = Vector2.zero;

    public Node parent;

    public float CalculateF(float parent_f, Vector2 dest)
    {
        h = parent_f;
        g = Vector2.Distance(position, dest);
        f = h + g;

        return f;
    }
}