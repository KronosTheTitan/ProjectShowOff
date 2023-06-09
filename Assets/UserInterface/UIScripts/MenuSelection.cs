using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{

    public GameObject uiToShow;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        uiToShow.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        uiToShow.SetActive(false);
    }
}


