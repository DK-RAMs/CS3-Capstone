using UnityEngine;
using System;
using System.Collections.Generic;


namespace src.NewspaperLibrary
{
    
    [CreateAssetMenu(fileName = "new Entry", menuName = "Newspaper/Entry")]
    public class NewspaperEntry : ScriptableObject
    {
        public int id;
        public string content;
        public NewspaperEvent newspaperEvent;
        
    }
}