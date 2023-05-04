using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaterTMP : MonoBehaviour
{
    private Material water;

    // Start is called before the first frame update
    void Start()
    {
        water = GetComponent<Renderer>().material;
    }

    int x = 2, y = 1;

    // Update is called once per frame
    void Update()
    {
        water.mainTextureOffset = new Vector2(x, y);
    }
}
