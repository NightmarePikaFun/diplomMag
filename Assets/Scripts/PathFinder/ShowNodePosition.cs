using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNodePosition : MonoBehaviour
{
    [SerializeField]
    private Vector2Int position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector2Int inputPosition)
    {
        position = inputPosition;
    }
}
