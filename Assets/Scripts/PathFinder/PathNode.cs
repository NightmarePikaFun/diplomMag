using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode //: MonoBehaviour
{
    public bool walkable;           //  �������� ��� �����������
    public Vector3 worldPosition;   //  ������� � ���������� �����������
    private GameObject objPrefab;   //  ������ �������
    public GameObject body;         //  ������ ��� ���������
    public Vector2Int pointCoord; //������� � ��������� �����������

    private PathNode parentNode = null;               //  ������ ������

    /// <summary>
    /// ������������ ������� - �������������� ������� � ���� �� ��������� � �������
    /// </summary>
    public PathNode ParentNode
    {
        get => parentNode;
        set => SetParent(value);
    }

    private float distance = float.PositiveInfinity;  //  ���������� �� ��������� �������

    /// <summary>
    /// ���������� �� ��������� ������� �� ������� (+infinity ���� ��� �� �����������)
    /// </summary>
    public float Distance
    {
        get => distance;
        set => distance = value;
    }

    /// <summary>
    /// ������������� �������� � ��������� ���������� �� ���� �� ������� �������. ������������ - ������ ���������� ���������
    /// </summary>
    /// <param name="parent">������������ ����</param>
    private void SetParent(PathNode parent)
    {
        //  ��������� ��������
        parentNode = parent;
        //  ��������� ����������
        if (parent != null)
            distance = parent.Distance + Dist(this, parent);//Vector3.Distance(body.transform.position, parent.body.transform.position);
        else
            distance = float.PositiveInfinity;
    }

    /// <summary>
    /// ����������� �������
    /// </summary>
    /// <param name="_objPrefab">������, ������� ��������������� � �������</param>
    /// <param name="_walkable">��������� �� �������</param>
    /// <param name="position">������� ����������</param>
    public PathNode(GameObject _objPrefab, bool _walkable, Vector3 position, Vector2Int coord)
    {
        objPrefab = _objPrefab;
        walkable = _walkable;
        worldPosition = position;
        body = GameObject.Instantiate(objPrefab, worldPosition, Quaternion.identity);
        pointCoord = coord;
    }

    /// <summary>
    /// ���������� ����� ��������� - ������� �� ������ ����������� �������������
    /// </summary>
    /// <param name="a">������ ����</param>
    /// <param name="b">������ ����</param>
    /// <returns></returns>
    public static float Dist(PathNode a, PathNode b)
    {
        return Vector3.Distance(a.worldPosition, b.worldPosition);
        //Vector3.Distance(a.body.transform.position, b.body.transform.position) + 40 * Mathf.Abs(a.body.transform.position.y - b.body.transform.position.y);
    }

    public void Illuminate()
    {
        body.GetComponent<Renderer>().material.color = Color.green;
    }
    /// <summary>
    /// ����� ��������� � ������� - ����������� � �����
    /// </summary>
    public void Fade()
    {
        body.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void Gray()
    {
        body.GetComponent<Renderer>().material.color = Color.gray;
    }

    public void Red()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
    }

}
