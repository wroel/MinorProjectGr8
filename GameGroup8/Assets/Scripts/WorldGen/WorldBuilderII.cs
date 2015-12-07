﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldBuilderII : MonoBehaviour {

    public int map_x;
    public int map_z;

    public Texture2D World_texture;
    public int tileRes;

    public GameObject Tree;
    public int nrTrees;
    public GameObject House;
    public int nrHouses;

    private List<Vector3> TreePos = new List<Vector3>();
    private List<Vector3> HousePos = new List<Vector3>();

    // Initialization on play
    void Start()
    {
        BuildTexture();
    }

    Color[][] LoadTexture()
    {
        int nrTiles_row = 6;
        int nrRows = 2;

        Color[][] tiles = new Color[nrTiles_row * nrRows][];

        for (int i = 0; i < nrRows; i++)
        {
            for (int j = 0; j < nrTiles_row; j++)
            {
                tiles[i * nrTiles_row + j] = World_texture.GetPixels(j * tileRes, i * tileRes, tileRes, tileRes);
            }
        }

        return tiles;
    }

    public void BuildTexture()
    {
        TDMapII map = new TDMapII(map_x, map_z);

        int texWidth = tileRes * map_x;
        int texHeight = tileRes * map_z;
        Texture2D texture = new Texture2D(texWidth, texHeight);

        Color[][] textures = LoadTexture();

        for (int i = 0; i < map_z; i++)
        {
            for (int j = 0; j < map_x; j++)
            {
                Color[] c = textures[map.getTile(i, j)];
                texture.SetPixels(j * tileRes, i * tileRes, tileRes, tileRes, c);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture;

        ApplyAssets(map);
    }

    public void ApplyAssets(TDMapII map)
    {

    }

    void PlaceHouses()
    {
        while (HousePos.Count < nrHouses)
        {
            Vector3 place = getRandPos(2, 3);
            addHousePos(HousePos, place, 5);

        }

        for (int i = 0; i < HousePos.Count; i++)
        {
            GameObject temp = (GameObject)Instantiate(House, HousePos[i], Quaternion.identity);
            temp.transform.Rotate(new Vector3(90, Random.Range(0, 360), 0));
            //placeWalls(HousePos[i], i);
        }
    }

    void PlaceTrees()
    {
        while (TreePos.Count < nrTrees)
        {
            Vector3 place = getRandPos(0, 2);
            if (!TreePos.Contains(place) && (!HousePos.Contains(place)))
            {
                TreePos.Add(place);
            }
        }

        for (int i = 0; i < TreePos.Count; i++)
        {
            Instantiate(Tree, TreePos[i], Quaternion.identity);
        }
    }

    Vector3 getRandPos(int dist, int offset)
    {
        float x = Mathf.Round(Random.Range(0, offset));
        float z = Mathf.Round(Random.Range(dist, offset));
            
        Vector3 place = new Vector3(x, 0, z);
        return place;
    }

    void addHousePos(List<Vector3> T, Vector3 place, int dist)
    {
        if (!T.Contains(place))
        {
            if (T.Count == 0)
            {
                T.Add(place);
            }
            else if (T.Count >= 1)
            {
                bool add = true;
                for (int i = 0; i < T.Count; i++)
                {
                    if (Vector3.Distance(place, T[i]) < dist)
                    {
                        add = false;
                    }
                }
                if (add)
                {
                    T.Add(place);

                }
            }
        }
    }
    /*
    void placeWalls(Vector3 place, int j)
    {
        bool walls = true;
        for (int i = 0; i < HousePos.Count; i++)
        {
            if ((i != j) && (Vector3.Distance(place, HousePos[i]) < 30))
            {
                walls = false;
            }
        }
        if (walls)
        {
            GameObject NorthWall = (GameObject)Instantiate(Wall, place + new Vector3(5, 0.5f, -1.5f), Quaternion.identity);
            GameObject EastWall = (GameObject)Instantiate(Wall, place + new Vector3(-2.5f, 0.5f, 5), Quaternion.identity);
            GameObject SouthWall = (GameObject)Instantiate(Wall, place + new Vector3(-10, 0.5f, -2.5f), Quaternion.identity);
            GameObject WestWall = (GameObject)Instantiate(Wall, place + new Vector3(0, 0.5f, -10), Quaternion.identity);
            NorthWall.transform.Rotate(new Vector3(0, 90, 0));
            NorthWall.transform.localScale = new Vector3(13, 1, 0.5f);
            EastWall.transform.Rotate(new Vector3(0, 0, 0));
            EastWall.transform.localScale = new Vector3(15, 1, 0.5f);
            SouthWall.transform.Rotate(new Vector3(0, 90, 0));
            SouthWall.transform.localScale = new Vector3(15, 1, 0.5f);
            WestWall.transform.Rotate(new Vector3(0, 0, 0));

        }
    }*/
}
