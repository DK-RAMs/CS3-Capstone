using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    
    public GameObject buildingPanel; // panel holds building data
    public GameObject other1;
    public GameObject other2;
    public GameObject other3;
    public GameObject other4;

    public static bool isCovered = false;
    public bool isClicked = false;
    public bool isClosed = false;
    public bool checkIfCovered;
	

	private void Start()
	{
        checkIfCovered = isCovered;
	}
	// Update is called once per frame
	void Update()
    {   
        // checks if sprite is clicked, if clicked displays details
        checkIfCovered = isCovered;
        //preventStack();
        if (isClicked == true && isClosed != true && isCovered != true)
        {
            Debug.Log("Sprite Clicked");
            buildingPanel.SetActive(true);
            preventStack();
        }
    }

    // allows building sprites to display details when click or tap is registered 
    private void OnMouseDown()
	{
        if (Input.GetMouseButtonDown(0) && isClicked == true)
        {
            Debug.Log("Down Clicked");
            buildingPanel.SetActive(true);
            preventStack();
            isClosed = false;
            isClicked = true;
            
        }
	}
    
    // allows building sprites to display details when click or tap is not registered
	private void OnMouseUp()
	{
        if (isClosed != true)
        {
            buildingPanel.SetActive(true);
            preventStack();
        }
        
    }

    // allows user to manually close building details
    public void closeStats()
	{
        //isClosed = false;
        isClicked = false;
        Debug.Log("Panel Closed");
        buildingPanel.SetActive(false);
	}

    // prevents building details from stacking on top of each other
    private void preventStack()
	{
        other1.SetActive(false);
        other2.SetActive(false);
        other3.SetActive(false);
        other4.SetActive(false);
    }
    
    
}
