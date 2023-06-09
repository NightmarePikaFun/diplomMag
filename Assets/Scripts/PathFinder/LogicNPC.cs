using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNPC : MonoBehaviour
{

    [SerializeField]
    private bool startPathFind;
    [SerializeField]
    private bool walkableFalse;
    [SerializeField]
    private bool walkableTrue;
    [SerializeField]
    private Vector2Int start;
    [SerializeField]
    private Vector2Int end;
    [SerializeField]
    private GameObject aStar;
    [SerializeField, Range(0.1f, 10.0f)]
    private float speed;
    [SerializeField]
    private int loadSpeed=10;
    [SerializeField]
    GameObject model;

    private Vector3Int vectorMove;
    private PathNode path;
    private PathNode[] pathLine;
    private int jobNumber;
    private int jobTime;
    private int pathIndex;
    private bool canStart = false;

    private void Start()
    {
        Invoke("RandomizeJob", loadSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canStart)
        {
            switch (jobNumber)
            {
                case 0:
                    if (pathIndex < pathLine.Length)
                    {
                        WalkToPoint();
                    }
                    else
                    {
                        start = pathLine[pathLine.Length - 1].pointCoord;
                        RandomizeJob();
                    }
                    break;
                case 1:
                    OtherJob("Job 1");
                    break;
                case 2:
                    OtherJob("Job 2");
                    break;
            }
        }
        if(startPathFind)
        {
            startPathFind = !startPathFind;
            path = aStar.GetComponent<PathCreator>().Astar(start, end);
            CreatePathLine(path);
        }/*
        if(walkableFalse)
        {
            walkableFalse =!walkableFalse;
            aStar.GetComponent<PathCreator>().CheckNowWalkableNodes();
        }
        if(walkableTrue)
        {
            walkableTrue = !walkableTrue;
            aStar.GetComponent<PathCreator>().DeCheckNodes(pathLine);
        }*/
    }

    //reverse pathLine;
    void WalkToPoint()
    {
        //Debug.Log(start + " " +  pathLine[pathIndex].pointCoord);
        //Debug.Log(transform.position + " " + pathLine[pathLine.Length - 1].worldPosition);
        if (CheckPosition(transform.position, pathLine[pathIndex].worldPosition))
        {
            start = pathLine[pathIndex].pointCoord;
            pathIndex++;//TODO обойти как-то через жопу
            CreateVectorMove(pathLine[pathIndex].worldPosition);
        }
        transform.Translate(new Vector3(vectorMove.x*Time.deltaTime,0,vectorMove.z*Time.deltaTime)*speed);
    }

    void OtherJob(string jobName/*animated somthing*/)
    {
        jobTime--;
        if(jobTime<=0)
        {
            RandomizeJob();
        }
        Debug.Log(jobName + " time: " + jobTime);
    }

    private void RandomizeJob()
    {
        jobNumber = 0;//Random.Range(0, 3);
        if(jobNumber==0)
        {
            if(pathLine!=null)
                aStar.GetComponent<PathCreator>().DeCheckNodes(pathLine);
            Debug.Log("A* start calc");
            Vector2Int randomEnd = aStar.GetComponent<PathCreator>().CreateRandomPoint();
            path = aStar.GetComponent<PathCreator>().Astar(start, randomEnd);
            Debug.Log("A* end calc");
            aStar.GetComponent<PathCreator>().CheckNowWalkableNodes();
            pathIndex = 0;
            CreatePathLine(path);
        }
        jobTime = Random.Range(600, 601);
        canStart = true;
    }

    bool CheckPosition(Vector3 current, Vector3 target)
    {
        bool retValue = false;
        if(Mathf.Abs(current.x - target.x)<0.01 && Mathf.Abs(current.y - target.y) < 0.01 && Mathf.Abs(current.z - target.z) < 0.01)
        {
            retValue = true;
            Debug.Log("Next Point Selected");
        }
        return retValue;
    }

    void CreatePathLine(PathNode input)
    {
        List<PathNode> lines = new List<PathNode>();
        var pathElem = input;
        while (pathElem != null)
        {
            lines.Add(pathElem);
            pathElem = pathElem.ParentNode;
        }
        pathLine = new PathNode[lines.Count];
        for (int i = 0; i < lines.Count;i++)
        {
            pathLine[i] = lines[lines.Count-1- i];
        }
        Debug.Log("Path line created");
    }

    void CreateVectorMove(Vector3 targetPosition)
    {
        Vector3 tmp = targetPosition - transform.position;
        Vector3.Normalize(tmp);
        vectorMove = new Vector3Int(RoundForInt(tmp.x), 0, RoundForInt(tmp.z));
        RotatePeople();
    }

    void RotatePeople()
    {
        if(vectorMove.x>0)
        {
            model.transform.Rotate(new Vector3(0,90,0));
        }
        else if(vectorMove.x<0)
        {
            model.transform.Rotate(new Vector3(0, -90, 0));
        }
        else if(vectorMove.z<0)
        {
            model.transform.Rotate(new Vector3(0, -180, 0));
        }
        else if(vectorMove.z>0)
        {
            model.transform.Rotate(new Vector3(0, 0, 0));
        }
    }
    //-90 -x
    //90 +x
    //180 -z
    //0 z
    int RoundForInt(float number)
    {
        int retValue = 0;
        if(number>0.10)
        {
            retValue = 1;
        }
        if(number<-0.10)
        {
            retValue = -1;
        }
        return retValue;
    }
}
