using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheker : MonoBehaviour
{
    public int radius = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, radius))
        {
            Debug.Log("Check");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
