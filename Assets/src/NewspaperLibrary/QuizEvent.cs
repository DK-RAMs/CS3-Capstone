using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "new Quiz Event", menuName = "Newspaper/Events/Quiz Event")]
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