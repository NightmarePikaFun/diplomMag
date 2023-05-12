using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingCreatorUI : EditorWindow
{
    private Texture2D[] textures = new Texture2D[15];
    private Sprite[] sprite = new Sprite[15];

    private GameObject[] prefubs = new GameObject[11];
    private GameObject _parent;
    private GameObject current;

    private int counter = 0;
    private int rotation = 0;

    private Vector2 scrollPos;
    private Vector2 mainScrollPos;

    private Vector3Int start;
    private Vector3Int end;
    private Vector3Int plusPos;
    private Vector3Int currentVector = Vector3Int.zero;

    private Material currentMaterial;
    private Material[] materials = new Material[3];

    private Color[] materialButtonColor = new Color[3];

    bool[] active = new bool[] { true, true, true, true };
    bool[] creatorActive = new bool[]{ true, true, true, true };
    [MenuItem("Builder/Builder Creator")]
    static void EditorInit()
    {
        BuildingCreatorUI editor = GetWindow<BuildingCreatorUI>("Builder creator");
        editor.Show();
    }

    private void OnGUI()
    {
        mainScrollPos = EditorGUILayout.BeginScrollView(mainScrollPos);
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
            if (GUILayout.Button(new GUIContent("T1", textures[0] ,"Wall"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T2", textures[1], "Wall with window"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T3", textures[2], "Wall with door"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T4", textures[3], "Double wall"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T5", textures[4], "Double wall with door"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T6", textures[5], "Double wall with door and window"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T7", textures[6], "Double wall window top"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T8", textures[7], "Double wall window bottom"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T9", textures[8], "Double wall with two window"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T10", textures[9], "Garden stone wall"), GUILayout.Width(40), GUILayout.Height(40)))
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
            if (GUILayout.Button(new GUIContent("T11", textures[10], "Garden stone wall with door"), GUILayout.Width(40), GUILayout.Height(40)))
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
        creatorActive[3] = EditorGUILayout.Foldout(creatorActive[3], "Material");
        if (creatorActive[3])
        {
            EditorGUILayout.BeginHorizontal();
            GUI.contentColor = materialButtonColor[0];
            if(GUILayout.Button(new GUIContent("M1", textures[10], "Material 1"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                SelectMaterial(0);
            }
            GUI.contentColor = materialButtonColor[1];
            if (GUILayout.Button(new GUIContent("M1", textures[10], "Material 1"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                SelectMaterial(1);
            }
            GUI.contentColor = materialButtonColor[2];
            if (GUILayout.Button(new GUIContent("M1", textures[10], "Material 1"), GUILayout.Width(40), GUILayout.Height(40)))
            {
                SelectMaterial(2);
            }
            GUI.contentColor = Color.white;
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
            prefubs[6] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall window top"), prefubs[6], typeof(GameObject), true);
            prefubs[7] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall window bottom"), prefubs[7], typeof(GameObject), true);
            prefubs[8] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with two window"), prefubs[8], typeof(GameObject), true);
            prefubs[9] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall"), prefubs[9], typeof(GameObject), true);
            prefubs[10] = (GameObject)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall with door"), prefubs[10], typeof(GameObject), true);
        }
        active[2] = EditorGUILayout.Foldout(active[2], "Texture");
        if (active[2])
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
            textures[0] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Wall"), textures[0], typeof(Texture2D), true);
            textures[1] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Wall with window"), textures[1], typeof(Texture2D), true);
            textures[2] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Wall with door"), textures[2], typeof(Texture2D), true);
            textures[3] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall"), textures[3], typeof(Texture2D), true);
            textures[4] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with door"), textures[4], typeof(Texture2D), true);
            textures[5] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with door and window"), textures[5], typeof(Texture2D), true);
            textures[6] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with one window top"), textures[6], typeof(Texture2D), true);
            textures[7] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with one window bottom"), textures[7], typeof(Texture2D), true);
            textures[8] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Double wall with two window"), textures[8], typeof(Texture2D), true);
            textures[9] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall"), textures[9], typeof(Texture2D), true);
            textures[10] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Garden stone wall with door"), textures[10], typeof(Texture2D), true);
            textures[11] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Material 1"), textures[11], typeof(Texture2D), true);
            textures[12] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Material 2"), textures[12], typeof(Texture2D), true);
            textures[13] = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(title = "Material 3"), textures[13], typeof(Texture2D), true);
            GUILayout.EndScrollView();
        }
        GUILayout.Space(10);
        if(GUILayout.Button("Load"))
        {
            LoadPresets();
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
        EditorGUILayout.EndScrollView();

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
        if(prefubNumber<9)
        {
            _initObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = currentMaterial;
        }
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
        if (prefubNumber < 9)
        {
            _initObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = currentMaterial;
        }
        counter++;
        _initObject.transform.eulerAngles = new Vector3(0, rotation, 0);
        current = _initObject;
        currentVector = new Vector3Int((int)_initObject.transform.position.x, (int)_initObject.transform.position.y, (int)_initObject.transform.position.z);
    }

    public void LoadPresets()
    {
        var loadObjectsGame = Resources.LoadAll("BuildingPrefubs/");
        for(int i = 0; i < loadObjectsGame.Length; i++)
        {
            prefubs[i] = (GameObject)loadObjectsGame[i];
        }
        var loadObjectIcon = Resources.LoadAll("BuildingIcon/");
        for( int i = 0;i < loadObjectIcon.Length; i++)
        {
            textures[i] = (Texture2D)loadObjectIcon[i];
        }
        var loadObjectMaterial = Resources.LoadAll("BuildingMaterial/");
        for(int i = 0; i< loadObjectMaterial.Length;i++)
        {
            materials[i] = (Material)loadObjectMaterial[i];
        }
        currentMaterial = materials[0];
    }

    public void SelectMaterial(int number)
    {
        for(int i = 0; i < materialButtonColor.Length; i++) 
        {
            materialButtonColor[i] = Color.white;
        }
        materialButtonColor[number] = Color.green;
        currentMaterial = materials[number];
    }

}

