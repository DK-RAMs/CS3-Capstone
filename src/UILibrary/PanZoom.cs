using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PanZoom : MonoBehaviour
{

    Vector3 touchStart;
    public float zoomOutMin = 50;
    public float zoomOutMax = 120;

    //public Vector2 panLimit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!Navigation.isClicked) // enter if no button is clicked
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            /*if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.2f);
            }
            */

            if (Input.GetMouseButton(0))
            {
                Vector3 direction = (touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Camera.main.transform.position += direction;
            }
            zoom(Input.GetAxis("Mouse ScrollWheel") * 10f);

        }
    }

    void zoom(float increment)
	{
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
	}
}
