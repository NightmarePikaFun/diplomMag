using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHolder
{
    public Vector2Int gridPosition;
    public Vector3 worldPosition;

    public PositionHolder(Vector2Int inputGridPos, Vector3 inputWorldPos)
    {
        gridPosition = inputGridPos;
        worldPosition = inputWorldPos;
    }
}
