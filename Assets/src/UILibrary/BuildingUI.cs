using src.CitizenLibrary;
using TMPro;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public TextMeshProUGUI bname;
    // general 
    //public string name;
    //public string status;
    //public string occupancy;
    //public string revenue;
    //public string upgrades;

    //public GameObject building;
    public GameObject buildingPanel;

    public Building building;
    
    public bool isClicked;
    //public TextMeshProUGUI bstatus;
    //public TextMeshProUGUI brevenue;
    //public TextMeshProUGUI bupgrades;

    private float posx;

    private float posy;
    //specific

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (isClicked)
        {
            buildingPanel.SetActive(true);
            Vector3 mousepos;
            mousepos = Input.mousePosition;
            mousepos = Camera.main.ScreenToViewportPoint(mousepos);

            buildingPanel.transform.localPosition = new Vector3(mousepos.x - posx, mousepos.y - posy, 0);
            // change text
            bname.text = "Town Hall\nOccupancy: 200\nRevenue: R 400,000\nStatus: Open\nUpgrades: None";
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepos;
            mousepos = Input.mousePosition;
            mousepos = Camera.main.ScreenToViewportPoint(mousepos);

            posx = mousepos.x - buildingPanel.transform.localPosition.x;
            posy = mousepos.y - buildingPanel.transform.localPosition.y;


            isClicked = true;
        }
    }

    private void OnMouseUp()
    {
        //isClicked = false;
    }
}