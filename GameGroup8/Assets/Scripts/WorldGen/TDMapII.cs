﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TDMapII {

    int[][] tiles;
    int[][] forrests;
    int[] villages;
    int[] BasePos;
    int size_x;
    int size_y;

    int baseSize = 16;
    int nrForr = 10;

    /// <summary>
    /// Create a new map texture with dimensions (height x width) and a given tile size
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    public TDMapII(int height, int width)
    {
        this.size_y = height;
        this.size_x = width;

        tiles = new int[size_y][];
        for (int i = 0; i < size_y; i++)
        {
            tiles[i] = new int[size_x];
        }

        forrests = new int[nrForr][];

        MakeLand();
        BasePos = PlaceBase();
        MakeWater();
        
        villages = PlaceVillage(BasePos);

        List<Vector2> connections = ConnectCityGrid(villages[0], villages[1]);
        ConnectRoadGrid(villages[0], villages[1], connections[0], connections[1]);
        
        for (int i = 0; i < nrForr; i++)
        {
            MakeForrests(i, villages);
        }
    }
    
    /// <summary>
    /// Returns the value of an tile (the data of the tile) on (x,y)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int getTile(int x, int y)
    {
        return tiles[y][x];
    }

    /// <summary>
    /// Sets the value of an tile on (x,y)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void setTile(int x, int y, int value)
    {
        tiles[y][x] = value;
    }

    /// <summary>
    /// Returns all the info for building a forrest in an array [x, y, max to left, max to right]
    /// </summary>
    /// <returns></returns>
    public int[][] getForrests()
    {
        return forrests;
    }

    /// <summary>
    /// Returns all the info for the city [x, y, max to left, max to right]
    /// </summary>
    /// <returns></returns>
    public int[] getVillages()
    {
        return villages;
    }

    /// <summary>
    /// Returns the position of the base [x, y]
    /// </summary>
    /// <returns></returns>
    public int[] getBasePosition()
    {
        return BasePos;
    }

    /// <summary>
    /// Creates a green land texture with 3 different grass tiles
    /// </summary>
    void MakeLand()
    {
        for (int y = 0; y < size_y; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
                float texture = Random.Range(0f, 10f);
                if (texture <= 5)
                {
                    tiles[y][x] = 1;
                }
                else if (texture > 5 && texture <= 7.5)
                {
                    tiles[y][x] = 2;
                }
                else if (texture >= 7.5)
                {
                    tiles[y][x] = 3;
                }
            }
        }
    }

    /// <summary>
    /// Generates the location of the base, accross the map to the city
    /// </summary>
    /// <returns></returns>
    int[] PlaceBase()
    {
        int x_pos = Random.Range(30, size_x-30);
        int y_pos = Random.Range(30, size_y-30);

        for (int y = 0; y < baseSize; y++)
        {
            for (int x = 0; x < baseSize; x++)
            {
                tiles[y + y_pos - baseSize/2][x + x_pos - baseSize/2] = 6;
            }
        }

        return new int[2] { x_pos, y_pos };
    }

    /// <summary>
    /// Generates the water at the edges of the map, including the beach
    /// </summary>
    void MakeWater()
    {
        int bottom_offset = 2;
        for (int y = 0; y < size_y; y++)
        {
            bottom_offset += Random.Range(-1, 2);
            if (bottom_offset < 2)
                bottom_offset = 2;
            if (bottom_offset > 10)
                bottom_offset = 9;

            tiles[y][size_x - bottom_offset - 1] = 11;
            tiles[y][size_x - bottom_offset - 2] = 11;
            for (int x = size_x - bottom_offset; x < size_x; x++)
            {
                tiles[y][x] = 0;
            }
        }

        int top_offset = 2;
        for (int y = 0; y < size_y; y++)
        {
            top_offset += Random.Range(-1, 2);
            if (top_offset < 2)
                top_offset = 2;
            if (top_offset > 10)
                top_offset = 9;

            tiles[y][top_offset + 1] = 11;
            tiles[y][top_offset] = 11;
            for (int x = 0; x < top_offset; x++)
            {
                tiles[y][x] = 0;
            }
        }

        int right_offset = 2;
        for (int x = 0; x < size_x; x++)
        {
            right_offset += Random.Range(-1, 2);
            if (right_offset < 2)
                right_offset = 2;
            if (right_offset > 10)
                right_offset = 9;

            if (tiles[right_offset + 1][x] != 0)
                tiles[right_offset + 1][x] = 11;
            if (tiles[right_offset][x] != 0)
                tiles[right_offset][x] = 11;
            for (int y = 0; y < right_offset; y++)
            {
                tiles[y][x] = 0;
            }
        }

        int left_offset = 2;
        for (int x = 0; x < size_x; x++)
        {
            left_offset += Random.Range(-1, 2);
            if (left_offset < 2)
                left_offset = 2;
            if (left_offset > 10)
                left_offset = 9;

            if (tiles[size_y - left_offset - 1][x] != 0)
                tiles[size_y - left_offset - 1][x] = 11;
            if (tiles[size_y - left_offset - 2][x] != 0)
                tiles[size_y - left_offset - 2][x] = 11;
            for (int y = size_y - left_offset; y < size_y; y++)
            {
                tiles[y][x] = 0;
            }
        }
    }

    /// <summary>
    /// Generates places and sizes of the forrests.
    /// It uses the index i to change the info on the forrest in the forrests list, and the position of the base to make shure they don't overlap
    /// </summary>
    /// <param name="i"></param>
    /// <param name="BasePos"></param>
    void MakeForrests(int i, int[] BasePos)
    {
        int x_pos = 0;
        int y_pos = 0;
        int max = Random.Range(12, 25);
        int min = -1 * max;
        Vector2 VillagePos = new Vector2(BasePos[0], BasePos[1]);

        int misses = 20;
        bool correct = false;
        while (!correct)
        {
            if (misses <= 0)
                break;

            x_pos = Random.Range(25, 275);
            y_pos = Random.Range(25, 275);
            Vector2 ForrestPos = new Vector2(x_pos, y_pos);
            
            if (Vector2.Distance(VillagePos,ForrestPos) > 60)
            {
                forrests[i] = new int[4] { x_pos, y_pos, min, max };
                correct = true;
            }
            else
            {
                misses--;
            }
        }

        /*
        for (int y = min; y < max; y++)
        {
            for (int x = min; x < max; x++)
            {
                if((tiles[y + y_pos][x + x_pos] != 0) && (tiles[y + y_pos][x + x_pos] != 5))
                    tiles[y + y_pos][x + x_pos] = 3;
            }
        }
        */
    }

    /// <summary>
    /// Generates the place of the city
    /// </summary>
    /// <param name="BasePos"></param>
    /// <returns></returns>
    int[] PlaceVillage(int [] BasePos)
    {
        int x_pos = 75;
        int y_pos = 75;

        if (BasePos[0] < 150)
            x_pos = 225;

        if (BasePos[1] < 150)
            y_pos = 225;

        int max = 60;
        int min = -60;
        
       return new int[4] { x_pos, y_pos, min, max };
    }

    /// <summary>
    /// This makes the road grid across the map, only the points to where the roads lead to.
    /// The input is to connect the roads to the city grid and to make shure the roads lead away from the city. 
    /// </summary>
    /// <param name="xV"></param>
    /// <param name="yV"></param>
    /// <param name="con_1"></param>
    /// <param name="con_2"></param>
    /// <returns></returns>
    List<Vector2>[] MakeRoadGrid(int xV, int yV, Vector2 con_1, Vector2 con_2)
    {
        List<Vector2> path1 = new List<Vector2>();
        List<Vector2> path2 = new List<Vector2>();
        Vector2 BasePos = new Vector2(300 - xV, 300 - yV);
        Vector2 BasePos_x = new Vector2(xV, 300 - yV);
        Vector2 BasePos_y = new Vector2(300 - xV, yV);

        path1.Add(con_1);
        path2.Add(con_2);

        int offset = 50;

        Vector2 point_offset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
        Vector2 point = point_offset + BasePos_x;
        path1.Add(point);
        point_offset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
        point = point_offset + BasePos;
        path1.Add(point);
        int misses1 = 10;
        while (path1.Count != 4)
        {
            if (misses1 <= 5)
                break;

            point_offset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
            point = point_offset + BasePos_x;
            if (checkDistance(point, path2, 30) && checkDistance(point, path1, 30))
            {
                path1.Add(point);
            }
            else
            {
                misses1--;
            }
        }

        point_offset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
        point = point_offset + BasePos_y;
        path2.Add(point);

        int misses2 = 10;
        while (path2.Count != 3)
        {
            if (misses2 <= 5)
                break;

            point_offset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
            point = point_offset + BasePos;
            if (checkDistance(point, path2, 30) && checkDistance(point, path1, 30))
            {
                path2.Add(point);
            }
            else
            {
                misses2--;
            }
        }

        while (path2.Count != 4)
        {
            if (misses2 <= 0)
                break;

            point_offset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
            point = point_offset + BasePos_y;
            if (checkDistance(point, path2, 30) && checkOffset(point, path2[1], 30))
            {
                path2.Add(point);
            }
            else
            {
                misses2--;
            }
        }

        List<Vector2>[] paths = new List<Vector2>[2] { path1, path2 };
        return paths;
    }

    /// <summary>
    /// Generates the road grit to connect the city to the other roads, and generates the square city road grid in points
    /// </summary>
    /// <param name="xB"></param>
    /// <param name="yB"></param>
    /// <returns></returns>
    List<Vector2> MakeCityGrid(int xB, int yB)
    {
        List<Vector2> junctions = new List<Vector2>();

        if (xB == 225)
            junctions.Add(new Vector2(Random.Range(200, 250), 150));

        if (xB == 75)
            junctions.Add(new Vector2(Random.Range(50, 100), 150));

        int x_city = Random.Range(-10, 10) + xB;
        int y_city = Random.Range(-10, 10) + yB;
        junctions.Add(new Vector2(x_city, y_city));

        if (yB == 225)
            junctions.Add(new Vector2(150, Random.Range(200, 250)));

        if (yB == 75)
            junctions.Add(new Vector2(150, Random.Range(50, 100)));
        
        for (int x = 0; x <60; x++)
        {
            for (int y = 0; y < 60; y++)
            {
                if (x == 0 || x == 19 || x == 39 || x == 59)
                    tiles[y + yB - 30][x + xB - 30] = 7;

                if (y == 0 || y == 19 || y == 39 || y == 59)
                    tiles[y + yB - 30][x + xB - 30] = 7;
            }
        }
            
        return junctions;
    }

    /// <summary>
    /// Checkes if the generated point for the road has a proper distance to the other points, only in x AND y direction.
    /// The input is the new point, the list as to where to compare the point with and the distance
    /// </summary>
    /// <param name="point"></param>
    /// <param name="junctions"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    bool checkOffset(Vector2 point, List<Vector2> junctions, int dist)
    {
        if (junctions.Count != 0)
        {
            foreach (Vector2 point2 in junctions)
            {
                if (Mathf.Abs(point2.x - point.x) < dist || Mathf.Abs(point2.y - point.y) < dist)
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    /// <summary>
    /// Checks if the generated point is not close to an other point
    /// </summary>
    /// <param name="point"></param>
    /// <param name="point2"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    bool checkOffset(Vector2 point, Vector2 point2, int dist)
    {
                if (Mathf.Abs(point2.x - point.x) < dist || Mathf.Abs(point2.y - point.y) < dist)
                {
                    return false;
                }

        return true;
    }

    /// <summary>
    /// Checks if the generated point is further away than the given distance
    /// </summary>
    /// <param name="point"></param>
    /// <param name="junctions"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    bool checkDistance(Vector2 point, List<Vector2> junctions, int dist)
    {
        if (junctions.Count != 0)
        {
            foreach (Vector2 point2 in junctions)
            {
                if (Vector2.Distance(point2 , point)  < dist)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Connects the main roads grid with roads, the road is only one point wide.
    /// This method is build to make the roads not straight, but let them meander slightly 
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    void makePrimaryRoads(int x1, int y1, int x2, int y2)
    {
        int rand = 0;
        while (x1 != x2 || y1 != y2)
        {
            int diff = Mathf.Abs(x1 - x2) - Mathf.Abs(y1 - y2);
            while (x1 != x2 && diff > 0)
            {
                tiles[y1][x1] = 7;

                switch (rand)
                {
                    case 0:
                    case -1:
                    case 1:
                        x1 += x1 < x2 ? 1 : -1;
                        rand += Random.Range(-1, 2);
                        break;
                    case 2:
                        y1++;
                        rand += Random.Range(-2, 1);
                        break;
                    case -2:
                        y1--;
                        rand += Random.Range(0, 3);
                        break;
                }

            }

            rand = 0;
            while (y1 != y2 && diff < 0)
            {
                tiles[y1][x1] = 7;

                switch (rand)
                {
                    case 0:
                    case -1:
                    case 1:
                        y1 += y1 < y2 ? 1 : -1;
                        rand += Random.Range(-1, 2);
                        break;
                    case 2:
                        x1++;
                        rand += Random.Range(-2, 1);
                        break;
                    case -2:
                        x1--;
                        rand += Random.Range(0, 3);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Connects the points of the city grid with straight roads, it also looks to the offset of the grid points
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    void makeVillageRoads(int x1, int y1, int x2, int y2)
    {
        while(x1 != x2 || y1 != y2)
        {
            int diff = Mathf.Abs(x1 - x2) - Mathf.Abs(y1 - y2);
            if (x1 != x2 && diff > 0)
            {
                tiles[y1][x1] = 7;
                x1 += x1 < x2 ? 1 : -1;
            }
            else if (y1 != y2 && diff < 0)
            {
                tiles[y1][x1] = 7;
                y1 += y1 < y2 ? 1 : -1;
            }
            else if (diff == 0)
            {
                tiles[y1][x1] = 7;
                x1 += x1 < x2 ? 1 : -1;
                y1 += y1 < y2 ? 1 : -1;
            }
        }
    }

    /// <summary>
    /// Creates the roads for the map. It uses the grid, connect and broaden methods
    /// </summary>
    /// <param name="xB"></param>
    /// <param name="yB"></param>
    /// <param name="con_1"></param>
    /// <param name="con_2"></param>
    void ConnectRoadGrid(int xB, int yB, Vector2 con_1, Vector2 con_2)
    {
        List<Vector2>[] con_Points = MakeRoadGrid(xB, yB, con_1, con_2);
        List<Vector2> con_Points1 = con_Points[0];
        List<Vector2> con_Points2 = con_Points[1];

        for (int i = 0; i < con_Points1.Count -2; i++)
        {
            Vector2 point1 = con_Points1[i];
            Vector2 point2 = con_Points1[i + 1];
            makePrimaryRoads((int)point1.x,(int)point1.y,(int)point2.x,(int)point2.y);
        }
        Vector2 point3 = con_Points1[1];
        Vector2 point4 = con_Points1[con_Points1.Count - 1];
        makePrimaryRoads((int)point3.x, (int)point3.y, (int)point4.x, (int)point4.y);

        for (int i = 0; i < con_Points2.Count - 2; i++)
        {
            Vector2 point1 = con_Points2[i];
            Vector2 point2 = con_Points2[i + 1];
            makePrimaryRoads((int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y);
        }
        point3 = con_Points2[1];
        point4 = con_Points2[con_Points2.Count - 1];
        makePrimaryRoads((int)point3.x, (int)point3.y, (int)point4.x, (int)point4.y);

        BroadenRoads(7);
    }

    /// <summary>
    /// Creates the roads for the city. It uses the grid, connect and broaden methods.
    /// The return of this method are the connection points for the main roads
    /// </summary>
    /// <param name="xB"></param>
    /// <param name="yB"></param>
    /// <returns></returns>
    List<Vector2> ConnectCityGrid(int xB, int yB)
    {
        List<Vector2> con_Points = MakeCityGrid(xB, yB);

        for (int i = 0; i < 2; i++)
        {
            Vector2 point1 = con_Points[i];
            Vector2 point2 = con_Points[i + 1];
            makeVillageRoads((int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y);
        }

        int times = (int)Mathf.Floor((con_Points.Count - 3)/3);

        for (int j = 0; j < times; j++)
        {
            for (int i = 3 * j; i < 3 * j + 2; i++)
            {
                Vector2 point1 = con_Points[i + 3];
                Vector2 point2 = con_Points[i + 4];
                makeVillageRoads((int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y);
            }
        }

        BroadenRoads(7);

        return new List<Vector2> { con_Points[0], con_Points[2] };
    }

    /// <summary>
    /// Broaden the roads created with the connect methods from 1 to 3 tiles.
    /// The input is a texture similar to the roadtexture, but with a different index as to not make all the map into roads
    /// </summary>
    /// <param name="tileTex"></param>
    void BroadenRoads(int tileTex)
    {
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                if ((tiles[j][i] == 1 || tiles[j][i] == 2 || tiles[j][i] == 3) && HasFloorConnect(i, j, tileTex))
                {
                    tiles[j][i] = tileTex + 1;
                }
            }
        }
    }

    /// <summary>
    /// Method used in the BroadenRoads method. 
    /// This method checks for the tile with given coordnates (i,j) if this tile has an roadtile connected to it
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="tileTex"></param>
    /// <returns></returns>
    private bool HasFloorConnect(int i, int j, int tileTex)
    {
        if (j < size_y - 1 && tiles[j + 1][i] == tileTex)
            return true;
        if (j > 0 && tiles[j - 1][i] == tileTex)
            return true;
        if (i < size_x - 1 && tiles[j][i + 1] == tileTex)
            return true;
        if (i > 0 && tiles[j][i - 1] == tileTex)
            return true;

        if (i > 0 && j > 0 && tiles[j - 1][i - 1] == tileTex)
            return true;
        if (i > 0 && j < size_y - 1 && tiles[j + 1][i - 1] == tileTex)
            return true;
        if (i < size_x - 1 && j > 0 && tiles[j - 1][i + 1] == tileTex)
            return true;
        if (i < size_x - 1 && j < size_y - 1 && tiles[j + 1][i + 1] == tileTex)
            return true;

        return false;
    }
}
