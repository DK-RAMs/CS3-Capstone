using System.Collections.ObjectModel;
using src.CitizenLibrary;
using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "new Decision Event", menuName = "Newspaper/Events/Decision Event")]
    public class DecisionEvent : NewspaperEvent
    {
        public string[] decisions;
        public double[] deltaHappiness;
        public double[] deltaHealth;
        public double[] deltaMoney;
        
        public DecisionEvent(string mainText) : base(mainText)
        {
            this.mainText = mainText;
        }

        public void applyDecision(int i, Town town)
        {
            town.applyDecisionEvent(deltaHappiness[i], deltaHealth[i], deltaMoney[i]);
        }
    }
}