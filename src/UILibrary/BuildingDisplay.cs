using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour
{
    public BuildingDetails buildingDetails;
    public TextMeshProUGUI nametext;
    public TextMeshProUGUI status;
    public TextMeshProUGUI occupancy;
    public Image artwork;

    // Start is called before the first frame update
    void Start()
    {
        nametext.text = buildingDetails.name;
        status.text = buildingDetails.status;
        occupancy.text = buildingDetails.occupancy.ToString();
        artwork.sprite = buildingDetails.artwork;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
