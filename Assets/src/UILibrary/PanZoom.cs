using UnityEngine;

namespace src.UILibrary
{
    public class PanZoom : MonoBehaviour
    {
        private Vector3 touchStart;
        public float zoomOutMax = 120;
        public float zoomOutMin = 50;

        //public Vector2 panLimit;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Navigation.isClicked) // enter if no button is clicked
            {
                if (Input.GetMouseButtonDown(0)) touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Input.touchCount == 2)
                {
                    var touchZero = Input.GetTouch(0);
                    var touchOne = Input.GetTouch(1);

                    var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    var currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    var difference = currentMagnitude - prevMagnitude;

                    zoom(difference * 0.2f);
                }

                else if (Input.GetMouseButton(0))
                {
                    var direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;
                }

                zoom(Input.GetAxis("Mouse ScrollWheel") * 10f);
            }
        }

        private void zoom(float increment)
        {
            Camera.main.orthographicSize =
                Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        }
    }
}