using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "new Quiz Event", menuName = "Newspaper/Events/Quiz Event")]
    public class QuizEvent : NewspaperEvent // Maybe still need to do?
    {
        public string[] answers;
        public int correctAnswer;
        public int[] deltas;
        
        public QuizEvent(string mainText) : base(mainText)
        {
            
        }
    }
}