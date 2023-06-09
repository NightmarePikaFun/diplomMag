using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject boxModel;

    [SerializeField]
    private GameObject nodeModel;
    [SerializeField]
    private Terrain landscape = null;
    [SerializeField]
    private int gridDelta = 100;
    [SerializeField]
    private float range = 1;    

    private PathNode[,] grid = null;
    private int[,] mapa = null;

    private int sizeX;
    private int sizeZ;
    private int obstaclesLayerMask;

    void Start()
    {
        Vector3 terrainSize = landscape.terrainData.bounds.size;
        sizeX = (int)(terrainSize.x / gridDelta);
        sizeZ = (int)(terrainSize.z / gridDelta);
        obstaclesLayerMask = 1 << LayerMask.NameToLayer("Obstacles");

        InitStart();
        CheckWalkableNodes();
        //mapa = this.gameObject.GetComponent<WallCreator>().GetMapa();

    }
    /// <summary>
    /// Get data for generation wall
    /// </summary>
    /// <returns>[sizeX, sizeZ, gridDelta]</returns>
    public int[] GetSize()
    {
        return new int[] { sizeX, sizeZ, gridDelta };
    }

    void InitStart()
    {
        Vector3 terrainSize = landscape.terrainData.bounds.size;
        sizeX = (int)(terrainSize.x / gridDelta);
        sizeZ = (int)(terrainSize.z / gridDelta);
        grid = new PathNode[sizeX, sizeZ];
        //mapa = this.gameObject.GetComponent<WallCreator>().GetMapa();
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                grid[x, z] = new PathNode(nodeModel, false, new Vector3(x * gridDelta, landscape.SampleHeight(new Vector3(x*gridDelta, 0, z*gridDelta))+1, z * gridDelta)+landscape.transform.position, new Vector2Int(x,z));
                grid[x, z].ParentNode = null;
                grid[x, z].Gray();
                grid[x, z].body.GetComponent<ShowNodePosition>().SetPosition(new Vector2Int(x, z));
                //Instantiate(nodeModel, new Vector3(x+1*x+1, landscape.transform.position.y+2, z+1*z+1), Quaternion.identity);
            }
        }
    }

    private void CheckWalkableNodes()
    {
        //mapa = this.gameObject.GetComponent<WallCreator>().GetMapa();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].walkable = true;
                grid[i, j].Gray();
                
                if (Physics.CheckSphere(grid[i,j].worldPosition, range))
                {
                    grid[i, j].walkable = false;
                    grid[i, j].Red();
                }
                
            }
        }
    }

    public void CheckNowWalkableNodes()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i,j].walkable)
                {
                    grid[i, j].Red();
                }
            }
        }
    }

    public void DeCheckNodes(PathNode[] nodeInput)
    {
        for (int i = 0; i < nodeInput.Length;i++)
        {
            nodeInput[i].Gray();
            nodeInput[i].walkable = true;
        }
    }

    private List<Vector2Int> GetNeighbours(Vector2Int current)
    {
        List<Vector2Int> nodes = new List<Vector2Int>();
        int x = current.x;
        int y = current.y;
        if (x - 1 >= 0)
        {
            nodes.Add(new Vector2Int(x - 1, y));
        }
        if (y - 1 >= 0)
        {
            nodes.Add(new Vector2Int(x, y - 1));
        }
        if (y + 1 < grid.GetLength(1))
        {
            nodes.Add(new Vector2Int(x, y + 1));
        }
        if (x + 1 < grid.GetLength(0))
        {
            nodes.Add(new Vector2Int(x + 1, y));
        }
        //8 соседий
        /*for (int x = current.x - 1; x <= current.x + 1; ++x)
            for (int y = current.y - 1; y <= current.y + 1; ++y)
                if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1) && (x != current.x || y != current.y))
                    nodes.Add(new Vector2Int(x, y));*/
        return nodes;
    }

    public PathNode Astar(Vector2Int startNode, Vector2Int finishNode)
    {
        //InitStart();
        int ds = 0;
        CheckNowWalkableNodes();
        //  Очищаем все узлы - сбрасываем отметку родителя, снимаем подсветку
        foreach (var node in grid)
        {
            node.ParentNode = null;
        }
        //CheckWalkableNodes();
        PathNode start = grid[startNode.x, startNode.y];
        start.ParentNode = null;
        start.Distance = 0;
        PriorityQueue<float, Vector2Int> pq = new PriorityQueue<float, Vector2Int>();
        pq.Enqueue(1, startNode);
        while (pq.Count != 0)
        {
            Vector2Int current = pq.Dequeue();
            float len;
            if (current == finishNode)
                break;
            var neighbours = GetNeighbours(current);
            foreach (var pathPoint in neighbours)
            {
                if (grid[pathPoint.x, pathPoint.y].walkable && grid[pathPoint.x, pathPoint.y].Distance > grid[current.x, current.y].Distance + PathNode.Dist(grid[pathPoint.x, pathPoint.y], grid[current.x, current.y]))
                {
                    grid[pathPoint.x, pathPoint.y].ParentNode = grid[current.x, current.y];
                    len = grid[pathPoint.x, pathPoint.y].Distance + PathNode.Dist(grid[pathPoint.x, pathPoint.y], grid[finishNode.x, finishNode.y]);
                    pq.Enqueue(len, pathPoint);
                }
                ds++;
            }
        }
        var pathElem = grid[finishNode.x, finishNode.y];
        Vector3 outElem = pathElem.worldPosition;
        List<Vector3> outPath = new List<Vector3>();
        while (pathElem != null)
        {
            if (pathElem.ParentNode != null || ds == 0)
            {
                outElem = pathElem.worldPosition;
            }
            pathElem.Fade();
            pathElem.walkable = false;
            outPath.Add(pathElem.worldPosition);
            pathElem = pathElem.ParentNode;
        }
        //AproximatePath(PathToArray(grid[finishNode.x,finishNode.y]));
        //grid = null;
        /*if ((outElem.x / gridDelta == finishNode.x && outElem.x / gridDelta != startNode.x)
            && (outElem.z / gridDelta == finishNode.y && outElem.z / gridDelta != startNode.y))
            outElem = new Vector3(startNode.x * gridDelta, 0, startNode.y * gridDelta);*/
        return grid[finishNode.x, finishNode.y];//outElem;
    }

    public Vector2Int CheckCoordsRight(Vector3 input)
    {
        Vector2Int output = new Vector2Int((int)input.x, (int)input.z);
        if (input.x - output.x > 0.5)
        {
            output.x += 1;
        }
        if (input.z - output.y > 0.5)
        {
            output.y += 1;
        }
        return output;
    }

    PathNode[] PathToArray(PathNode inputNode)
    {
        PathNode[] outputPath;
        List<PathNode> line = new List<PathNode>();
        var selectNode = inputNode;
        while(selectNode != null)
        {
            line.Add(selectNode);
            selectNode = selectNode.ParentNode;
        }
        outputPath = new PathNode[line.Count];
        for(int i = 0; i < line.Count;i++)
        {
            outputPath[i] = line[i];
        }
        return outputPath;
    }

    void AproximatePath(PathNode[] nodeLine)
    {
        PositionHolder[] newNodeLine = new PositionHolder[nodeLine.Length];
        bool vectorLine = true;//true --- x , false --- y
        for(int i = 0; i < nodeLine.Length;i++)
        {
            newNodeLine[i] = new PositionHolder(nodeLine[i].pointCoord, nodeLine[i].worldPosition);
        }
        for (int i = 2; i< newNodeLine.Length;i++)
        {
            if(vectorLine)
            {
                if (newNodeLine[i].worldPosition.z != newNodeLine[i-1].worldPosition.z)
                {
                    vectorLine = false;
                    newNodeLine[i].worldPosition = (newNodeLine[i].worldPosition+ newNodeLine[i-2].worldPosition)/2; //change position need new class for this shit
                    DeactivateNearbyNodes(newNodeLine[i].gridPosition, newNodeLine[i-2].gridPosition);
                }
            }
            else
            {
                if (newNodeLine[i].worldPosition.x != newNodeLine[i - 1].worldPosition.x)
                {
                    vectorLine = true;
                    newNodeLine[i].worldPosition = (newNodeLine[i].worldPosition + newNodeLine[i - 2].worldPosition) / 2; //change position
                    DeactivateNearbyNodes(newNodeLine[i].gridPosition, newNodeLine[i - 2].gridPosition);
                }
            }
        }
    }

    void DeactivateNearbyNodes(Vector2Int first, Vector2Int second)
    {
        grid[first.x, first.y].walkable = false;
        grid[first.x, second.y].walkable = false;
        grid[second.x, first.y].walkable = false;
        grid[second.x, second.y].walkable = false;
    }

    public bool GridIsNull()
    {
        bool retValue = false;
        if(grid == null)
            retValue = true;
        return retValue;
    }

    public Vector2Int CreateRandomPoint()
    {
        int posX, posZ;
        while (true)
        {
            posX = (int)Random.RandomRange(0, sizeX);
            posZ = (int)Random.RandomRange(0, sizeZ);
            Debug.Log(posX + " " + posZ);
            Debug.Log(grid);
            if (grid[posX, posZ].walkable)
                break;
        }
        return new Vector2Int(posX,posZ);
    }
}
