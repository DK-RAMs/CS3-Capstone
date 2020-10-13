using UnityEngine;
using UnityEngine.UI;


namespace srd
{
    public class ToggleScript : MonoBehaviour
    {

        Toggle t1;
        public Text m_Text;

        void Start()
        {
            //Fetch the Toggle GameObject
            t1 = GetComponent<Toggle>();
            //Add listener for when the state of the Toggle changes, to take action
            t1.onValueChanged.AddListener(delegate { ToggleValueChanged(t1); });

            //Initialise the Text to say the first state of the Toggle
            m_Text.text = "First Value : " + t1.isOn;
        }

        //Output the new state of the Toggle into Text
        void ToggleValueChanged(Toggle change)
        {
            m_Text.text = "New Value : " + t1.isOn;
        }
    }
}