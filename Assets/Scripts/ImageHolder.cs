using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageHolder : MonoBehaviour
{
    [SerializeField]
    private Sprite image;
    [SerializeField]
    private string discriptionText;
    [SerializeField]
    private string notationText;
    [SerializeField]
    private GameObject Canvas; //TODO Текст подсказка, А после канвас
    [SerializeField]
    private Sprite zeroImage;
    
    private bool canClick = false;
    private bool activeHolder = false;
    private Image canvasImageHolder;
    private Text canvasNotationHolder;
    private Text canvasDiscriptionHolder;
    // Start is called before the first frame update
    void Start()
    {
        canvasImageHolder = Canvas.transform.GetChild(0).GetComponent<Image>();
        canvasDiscriptionHolder = Canvas.transform.GetChild(1).GetComponent<Text>();
        canvasNotationHolder = Canvas.transform.GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canClick && (Input.GetKeyDown(KeyCode.F)) && !activeHolder)
        {
            activeHolder = !activeHolder;
            //SetActive this image and discription and disable notation
            canvasNotationHolder.text = "";
            canvasImageHolder.sprite = image;
            canvasDiscriptionHolder.text = discriptionText;
        }
        else if(canClick && (Input.GetKeyDown(KeyCode.F)) && activeHolder)
        {
            activeHolder = !activeHolder;
            canvasNotationHolder.text = notationText;
            canvasImageHolder.sprite = zeroImage;
            canvasDiscriptionHolder.text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        canClick = true;
        Canvas.SetActive(true); //other notation open
        //set active this notation
        canvasNotationHolder.text = notationText;
    }

    private void OnTriggerExit(Collider other)
    {
        canClick = false;
        Canvas.SetActive(false);
    }
}
