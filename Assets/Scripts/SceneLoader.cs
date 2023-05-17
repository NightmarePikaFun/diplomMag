using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string sceneName;


    private bool canAcitve = false;
    // Update is called once per frame
    void Update()
    {
        if (canAcitve && Input.GetKey(KeyCode.F))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter"); 
        if (other.gameObject.tag == "Player")
        {
            canAcitve = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canAcitve = false;
        }
    }
}
