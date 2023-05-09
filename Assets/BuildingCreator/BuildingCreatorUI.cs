using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingCreatorUI : EditorWindow
{
    private GameObject[] prefubs = new GameObject[11];
    private GameObject _parent;
    private GameObject current;

    private int counter = 0;
    private int rotation = 0;

    private Vector3Int start;
    private Vector3Int end;
    private Vector3Int plusPos;
    private Vector3Int currentVector = Vector3Int.zero;

    bool[] active = new bool[] { true, true };
    bool[] creatorActive = new bool[]{ true, true, true, true };
    [MenuItem("Builder/Builder Creator")]
    static void EditorInit()
    {
        BuildingCreatorUI editor = GetWindow<BuildingCreatorUI>("Builder creator");
        editor.Show();
    }

    private void OnGUI()
    {
        counter = EditorGUILayout.IntField("Current number", counter);
        GUI.contentColor = Color.green;
        EditorGUILayout.LabelField("Position: "+ currentVector);
        GUI.contentColor = Color.white;
        EditorGUILayout.LabelField("Creation");
        start = EditorGUILayout.Vector3IntField("Start pos", start);
        end = EditorGUILayout.Vector3IntField("End pos", end);
        plusPos = EditorGUILayout.Vector3IntField("Vector move", plusPos);
        EditorGUILayout.Space(10);
        rotation = EditorGUILayout.IntField("Rotation", rotation);
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Creation");
        //EditorGUILayout.BeginHorizontal();
        #region Create button
        creatorActive[0] = EditorGUILayout.Foldout(creatorActive[0], "Wall");
        if (creatorActive[0])
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("T1", "Wall"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(0);
                }
                else
                {
                    CreatorNotParent(0);
                }
            }
            if (GUILayout.Button(new GUIContent("T2", "Wall with window"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(1);
                }
                else
                {
                    CreatorNotParent(1);
                }
            }
            if (GUILayout.Button(new GUIContent("T3", "Wall with door"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(2);
                }
                else
                {
                    CreatorNotParent(2);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        creatorActive[1] = EditorGUILayout.Foldout(creatorActive[1], "Double wall");
        if (creatorActive[1])
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("T4", "Double wall"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(3);
                }
                else
                {
                    CreatorNotParent(3);
                }
            }
            if (GUILayout.Button(new GUIContent("T5", "Double wall with door"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(4);
                }
                else
                {
                    CreatorNotParent(4);
                }
            }
            if (GUILayout.Button(new GUIContent("T6", "Double wall with door and window"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(5);
                }
                else
                {
                    CreatorNotParent(5);
                }
            }
            if (GUILayout.Button(new GUIContent("T7", "Double wall with one window top"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(6);
                }
                else
                {
                    CreatorNotParent(6);
                }
            }
            if (GUILayout.Button(new GUIContent("T8", "Double wall with one window bottom"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(7);
                }
                else
                {
                    CreatorNotParent(7);
                }
            }
            if (GUILayout.Button(new GUIContent("T9", "Double wall with two window"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(8);
                }
                else
                {
                    CreatorNotParent(8);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        creatorActive[2] = EditorGUILayout.Foldout(creatorActive[2], "Garden wall");
        if (creatorActive[2])
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("T10", "Garden stone wall"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(9);
                }
                else
                {
                    CreatorNotParent(9);
                }
            }
            if (GUILayout.Button(new GUIContent("T11", "Garden stone wall with door"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                if (_parent != null)
                {
                    CreatorWithParent(10);
                }
                else
                {
                    CreatorNotParent(10);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
        //EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        active[0] = EditorGUILayout.Foldout(active[0], "Parents");
        if (active[0])
        {
            _parent = (GameObject)EditorGUILayout.ObjectField(_parent, typeof(GameObject), true);
        }
        active[1] = EditorGUILayout.Foldout(active[1], "Objects");
        if (active[1])
        {
            prefubs[0] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Wall"), prefubs[0], typeof(GameObject), true);
            prefubs[1] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Wall with window"), prefubs[1], typeof(GameObject), true);
            prefubs[2] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Wall with door"), prefubs[2], typeof(GameObject), true);
            prefubs[3] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall"), prefubs[3], typeof(GameObject), true);
            prefubs[4] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with door"), prefubs[4], typeof(GameObject), true);
            prefubs[5] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with door and window"), prefubs[5], typeof(GameObject), true);
            prefubs[6] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with one window top"), prefubs[6], typeof(GameObject), true);
            prefubs[7] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with one window bottom"), prefubs[7], typeof(GameObject), true);
            prefubs[8] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with two window"), prefubs[8], typeof(GameObject), true);
            prefubs[9] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall"), prefubs[9], typeof(GameObject), true);
            prefubs[10] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall with door"), prefubs[10], typeof(GameObject), true);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Undo", GUILayout.Height(40)))
        {
            GameObject.DestroyImmediate(current);
            current = null;
            counter--;
            if (counter < 0)
                counter = 0;
        }
        GUILayout.Space(60);
        if (GUILayout.Button("Reset position"))
        {
            currentVector = Vector3Int.zero;
            start = Vector3Int.zero;
            end = Vector3Int.zero;
            _parent = null;
            active[0] = true;
        }
        GUILayout.Space(10);
        GUI.contentColor = Color.red;
        if (GUILayout.Button("Reset All"))
        {
            currentVector = Vector3Int.zero;
            start = Vector3Int.zero;
            end = Vector3Int.zero;
            rotation = 0;
            plusPos = Vector3Int.zero;
            _parent = null;
            active[0] = true;
        }

    }

    public void CreatorWithParent(int prefubNumber)
    {
        GameObject _initObject;
        _initObject = PrefabUtility.InstantiatePrefab(prefubs[prefubNumber], _parent.transform) as GameObject;
        if (plusPos.x > plusPos.z)
        {
            _initObject.transform.position = _parent.transform.position + start + new Vector3(plusPos.x * counter, plusPos.y, plusPos.z);
        }
        else
        {
            _initObject.transform.position = _parent.transform.position + start + new Vector3(plusPos.x, plusPos.y, plusPos.z * counter);
        }
        counter++;
        _initObject.transform.eulerAngles = new Vector3(0, rotation, 0);
        current = _initObject;
        currentVector = new Vector3Int((int)_initObject.transform.position.x, (int)_initObject.transform.position.y, (int)_initObject.transform.position.z);
    }

    public void CreatorNotParent(int prefubNumber)
    {
        GameObject _initObject;
        _initObject = PrefabUtility.InstantiatePrefab(prefubs[prefubNumber]) as GameObject;
        if (plusPos.x > plusPos.z)
        {
            _initObject.transform.position = start + new Vector3(plusPos.x * counter, plusPos.y, plusPos.z);
        }
        else
        {
            _initObject.transform.position = start + new Vector3(plusPos.x, plusPos.y, plusPos.z * counter);
        }
        counter++;
        _initObject.transform.eulerAngles = new Vector3(0, rotation, 0);
        current = _initObject;
        currentVector = new Vector3Int((int)_initObject.transform.position.x, (int)_initObject.transform.position.y, (int)_initObject.transform.position.z);
    }
}

