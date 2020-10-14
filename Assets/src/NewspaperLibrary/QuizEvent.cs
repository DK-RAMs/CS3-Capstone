using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "new State Event", menuName = "Newspaper/Events/State Event")]
    public class QuizEvent : NewspaperEvent
    {
        public string[] answers;
        public int correctAnswer;
        public double[] deltas;
        
        public QuizEvent(string mainText) : base(mainText)
        {
            
        }
    }
}