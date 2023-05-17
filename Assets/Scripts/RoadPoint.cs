using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPoint : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> points = new List<GameObject>();

    public GameObject GetNextPoint()
    {
        return points[Random.Range(0, points.Count)];
    }
}
