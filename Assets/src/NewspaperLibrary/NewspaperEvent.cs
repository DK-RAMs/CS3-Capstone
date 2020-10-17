using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "newNewspaperEvent", menuName = "Newspaper Events")]
    public abstract class NewspaperEvent : ScriptableObject
    {
        public string mainText;

        public NewspaperEvent(string mainText)
        {
            this.mainText = mainText;
        }
        
        
        public string MainText => mainText;
    }
}