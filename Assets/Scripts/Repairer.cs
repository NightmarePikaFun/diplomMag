using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class Repairer : EditorWindow
{
    private GameObject _parent;
    private GameObject[] prefubs = new GameObject[11];
    private Dictionary<string,GameObject> spisok = new Dictionary<string,GameObject>();

    private int rotation;

    private Vector3 vecotrPlus;

    private bool[] active = { true, true };
    private bool withNumber;

    [MenuItem("Builder/Builder Repairer")]

    static void EditorInit()
    {
        Repairer editor = GetWindow<Repairer>("Repairer");
        editor.Show();
    }

    private void OnGUI()
    {
        rotation = EditorGUILayout.IntField("Rotation", rotation);
        vecotrPlus = EditorGUILayout.Vector3Field("Vector+", vecotrPlus);
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
            prefubs[6] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall window top"), prefubs[6], typeof(GameObject), true);
            prefubs[7] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall window bottom"), prefubs[7], typeof(GameObject), true);
            prefubs[8] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with two window"), prefubs[8], typeof(GameObject), true);
            prefubs[9] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall"), prefubs[9], typeof(GameObject), true);
            prefubs[10] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall with door"), prefubs[10], typeof(GameObject), true);
        }
        withNumber = EditorGUILayout.Toggle("With number active", withNumber);
        GUILayout.Space(10);
        if (GUILayout.Button("Load"))
        {
            LoadPresets();
            SetSpisok();
        }

        GUILayout.Space(10);
        if(GUILayout.Button("Repair"))
        {
            StartRepair();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Rename"))
        {
            ChangeName();
        }
    }

    public void LoadPresets()
    {
        var loadObjectsGame = Resources.LoadAll("BuildingPrefubs/");
        for (int i = 0; i < loadObjectsGame.Length; i++)
        {
            prefubs[i] = (GameObject)loadObjectsGame[i];
        }
    }

    public void SetSpisok()
    {
        spisok = new Dictionary<string, GameObject>();
        for(int i = 0; i < prefubs.Length; i++)
        {
            spisok.Add(prefubs[i].name, prefubs[i]);
        }
    }

    GameObject[] otcat;

    public void ChangeName()
    {
        GameObject[] repaireItmes = new GameObject[_parent.transform.childCount];
        for (int i = 0; i < repaireItmes.Length; i++)
        {
            repaireItmes[i] = _parent.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < repaireItmes.Length;i++)
        {
            repaireItmes[i].name = repaireItmes[i].name.Split(' ')[0];
        }
    }
    // 6.11992 15.6378
    public void StartRepair()
    {
        Debug.Log(_parent.transform.childCount);
        GameObject[] repaireItmes = new GameObject[_parent.transform.childCount];
        for(int i = 0;i < repaireItmes.Length; i++)
        {
            repaireItmes[i] = _parent.transform.GetChild(i).gameObject;
        }
        otcat = repaireItmes;
        if (withNumber)
        {
            for (int i = 0; i < repaireItmes.Length; i++)
            {
                Debug.Log(repaireItmes[i].name);
                GameObject _initObject;
                _initObject = PrefabUtility.InstantiatePrefab(spisok[repaireItmes[i].name], _parent.transform) as GameObject;
                _initObject.transform.position = repaireItmes[i].transform.position + vecotrPlus;
                _initObject.transform.localEulerAngles = new Vector3(0, rotation, 0);
                //DestroyImmediate(repaireItmes[i]);
            }
        }
        else
        {
            for (int i = 0; i < repaireItmes.Length; i++)
            {
                    Debug.Log(repaireItmes[i].name);
                    GameObject _initObject;
                    _initObject = PrefabUtility.InstantiatePrefab(FindItem(repaireItmes[i].name), _parent.transform) as GameObject;
                    _initObject.transform.position = repaireItmes[i].transform.position + vecotrPlus;
                    _initObject.transform.localEulerAngles = new Vector3(0, rotation, 0);
                    //DestroyImmediate(repaireItmes[i]);
            }
        }     
    }

    private GameObject FindItem(string str)
    {
        GameObject retObject = new GameObject();
        foreach(var variants in spisok)
        {
            if(variants.Key.Remove(0,2)==str)
            {
                retObject = variants.Value;
                break;
            }
        }
        return retObject;
    }
}
#endif