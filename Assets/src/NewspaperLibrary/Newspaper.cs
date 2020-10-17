﻿using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
 using UnityEngine.UI;

 namespace src.NewspaperLibrary
 {
     public class Newspaper : MonoBehaviour
     {
         // The script will be linked to the newspaper. When a new article is selected, 
         public static Text content; // list of strings to store all the different articles, so only have to read file once at start of game seeing as article content and size would only change with an update on our side, and not mid game - Z
         public static bool newspaperReady = false;
         private static NewspaperEntry newspaperEvent;
         private static int pos;

         public static NewspaperEntry[] events;

         public void Start()
         { 
             while (!Game.TOWNREADY)
             {
                 
             }
             
             if (Game.ISNEWGAME)
             {
                 newspaperEvent = events[0];
             }
             else
             {
                 
             }
         }



         /*
         public void readArticles()
         {
             // side note, should I make a method writeArticle which adds one to the bottom of the text file so we can just do it from command line? Would that work? - Z
             try
             {
                 StreamReader
                     inp_stm = new StreamReader(
                         "Assets/news_articles.txt"); // initialise stream reader, giving .txt file name - Z
                 string article = ""; // blank string - Z
                 //int count = 0; // counter for adding the article to the correct index - Z

                 while (!inp_stm.EndOfStream)
                 {
                     string line = inp_stm.ReadLine(); // read next line in file - Z
                     if (line.Equals("###break###"))
                     {
                         content.Add(
                             article); // if reached this line, article is over and add the complete article to the list at its index - Z
                         //count++; // increment the counter because article added - Z
                     }
                     else
                     {
                         article += line; // if not at break line, add the line to the article string - Z
                     }
                 }

                 inp_stm.Close(); // close the stream reader - Z
             }
             catch (System.Exception e)
             {
                 // Let the user know what went wrong.
                 System.Console.WriteLine("The file could not be read:");
                 System.Console.WriteLine(e.Message);
             }

             //content.ForEach(System.Console.WriteLine); // testing to make sure read them all in. not working. how the fuck do i do this in unity - Z
         }*/
         
         public static void triggerNewspaperEvent(int id)
         {
             pos = id;
             newspaperEvent = events[id];
         }

         // need to add some form of question mechanism, can code it easily enough, just not sure how to design it - Z

         // Start is called before the first frame update

     }
 }