using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private GameObject moveHelper;
    [SerializeField]
    private int speed;
    [SerializeField]
    private float _sensitivity;
    [SerializeField]
    public float minVert = -45.0f;
    [SerializeField]
    public float maxVert = 45.0f;

    private Rigidbody playerRB;
    private float rotX = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = this.gameObject.GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 eyeVector = moveHelper.transform.position - transform.position;
        eyeVector.y = 0;
        playerRB.AddForce(Input.GetAxis("Vertical") * speed * eyeVector);
        rotX -= Input.GetAxis("Mouse Y") * _sensitivity;
        rotX = Mathf.Clamp(rotX, minVert, maxVert);
        float rotY = transform.localEulerAngles.y+Input.GetAxis("Mouse X") * _sensitivity;
        //transform.Rotate(0, Input.GetAxis("Mouse X") * _sensitivity, 0);
        //transform.Rotate(rotX, rotY, 0);
        transform.localEulerAngles = new Vector3(rotX, rotY, 0);

        //float delta = Input.GetAxis("Mouse X") * _sensitivity;
        //float rotationY = transform.localEulerAngles.y + delta;

    }
}
