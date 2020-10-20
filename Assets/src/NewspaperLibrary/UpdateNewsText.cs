using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.NewspaperLibrary
{
    public class UpdateNewsText : MonoBehaviour // Turns out, we do need this file
    {
        public TextMeshProUGUI news;
        Text newsText;

        string article1 = "First confirmed case of Covid-19 in town!\n" +
            "The patient is a 38-year-old male who travelled to Italy with his wife. " +
            "They were part of a group of 10 people and they arrived back in town last week." +
            "The patient consulted a private general practitioner, " +
            "with symptoms of fever, headache, malaise, a sore throat and a cough. " +
            "Swabs were taken and delivered to the lab." +
            "The patient has been self-isolating for the last 3 days. The couple also has two children.";

        string article2 = "The pandemic is progressing, and health officials are pointing to studies arguing mask wearing in public " +
            "should be enforced in order to minimise the spread of the virus." +
            "President Peach has not publicly backed this message, " +
            "however as of yet refuses to be seen wearing a mask at official events. " +
            "President Peach won't wear a mask and neither will I! " +
            "Wearing a mask is an infringement on my basic rights!" + "a local citizen is quoted as saying." +
            "Health officials recommend stomping out this behaviour before this dangerous mentality begins to spread.";

        string article3 = "It's become clear around the world that this virus has been underestimated." +
            "Vaccine trials are in their extremely early stages and in the interim scientists are trying to find a medicine that effectively treats the disease." +
            "Rumours emerged of one \"miracle" + "\n" + "drug that helps treat severe cases of Covid-19,"
            + "however scientists were quick to try and clear up any misconceptions by announcing"
           + "that nothing was for sure and treatments were still being researched." +
            "Miracle drug discovered! Everyone take it and you'll be fine! If you can't find any, injecting yourself with disinfectant is the way to go!"
            + "Citizens are getting restless and contemplating engaging in riskier behaviour thinking "
             + "there's a miracle drug there to save them if they fall ill.";

        string article4 = "Evidence is emerging that for the most part, the younger population are much less likely to experience severe symptoms. " +
            "President Peach has backed up this thinking by proclaiming " +
            "many of our cases are young people who would recover in a day, they get the sniffles and it's nothing to worry about." + 
            "While this appears to be true to some extent, some young people are now behaving like they think they're invincible or immune." + 
            "Senior health officials have put out a statement saying that despite this, all members of the population regardless of age group" 
            + "should keep practicing social distancing and good pandemic practices,"
            + " because while they might not be at risk, other people are, and they risk spreading it to them."
            + " Moreover, the long term effects of the virus on the human body are still not fully understood.";

        // Start is called before the first frame update
        void Start()
        {

            news.text = article1;
        }

        // Update is called once per frame
        void Update()
        {
            switch (Game.town.Day) {
                case 7:
                    news.text = article2;
                    break;
                case 14:
                    news.text = article3;
                    break;
                case 21:
                    news.text = article4;
                    break;
            }
        }
    }
}