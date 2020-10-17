using System;
using System.Collections.Generic;
using System.IO;

public static class Newspaper
{
    public static int
        id; // id as identifier for article, when id is incremented then article is changed to the next one - Z

    public static List<string>
        content = new List<string>(); // list of strings to store all the different articles, so only have to read file once at start of game seeing as article content and size would only change with an update on our side, and not mid game - Z

    static Newspaper() // default constructor - Z
    {
        id = 0; // id, i.e. article starts at 0 to make it simpler to access the correct element in the list - Z
        readArticles(); // call method to read articles from .txt file and populate the list - Z
    }

    public static void readArticles()
    {
        // side note, should I make a method writeArticle which adds one to the bottom of the text file so we can just do it from command line? Would that work? - Z
        try
        {
            var inp_stm =
                new StreamReader("Assets/news_articles.txt"); // initialise stream reader, giving .txt file name - Z
            var article = ""; // blank string - Z
            //int count = 0; // counter for adding the article to the correct index - Z

            while (!inp_stm.EndOfStream)
            {
                var line = inp_stm.ReadLine(); // read next line in file - Z
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
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }

        //content.ForEach(System.Console.WriteLine); // testing to make sure read them all in. not working. how the fuck do i do this in unity - Z
    }

    public static void incrementID()
    {
        id++;
        // either need to call change of article text in UI from here or town - Z
    }
    // need to add some form of question mechanism, can code it easily enough, just not sure how to design it - Z

    // Start is called before the first frame update
}