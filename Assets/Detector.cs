using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameObject optionPanel;
    // Start is called before the first frame update
    void Start(){
        optionPanel = GameObject.Find("OptionPanel");
        Debug.Log(optionPanel);
        optionPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space")){
            optionPanel.SetActive(true);
        }
    }
}
