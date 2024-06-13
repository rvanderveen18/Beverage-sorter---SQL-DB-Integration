// Ryan VanderVeen
// Fall 2021 CIS 207
// Console Assignment 4
// This program presents the user with a movie title and year it was released, it will then ask the user to rate the movie from one star to 5 (*, **, ***, ****, *****). The program will keep requesting the user to enter ratings until they request to exit the program by entering in "exit". Once the user requests to exit the program, all ratings made by the user will be saved back to the JSON files.


using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleAssigment4_RV
{
    class Program
    {
        static void Main(string[] args)
        {
            // The Main method takes the JSON data that was passed back from "ReadJsonData" method, stores it into a list of objects called theMovies, then loops through each of the movies, requires the user to rate the movie, then sends the updated JSON data with the rating data updated to the "WriteJsonData" method to have the new data written back to the JSON files. 

            string jsonFolder = "JSON"; // storing the name of the JSON folder within the EXE folder

            Console.WriteLine("to exit the program, type in 'exit'. ");
            List<Movie> theMovies = ReadJsonData(jsonFolder); // Reading and storing JSON file data to memory. Sending the jsonFolder var as an input, receiving movieList back from the method

            foreach (var aMovie in theMovies) // cycling through each movie for a rating
            {
                Console.WriteLine("");
                Console.WriteLine(aMovie.title + " " + "(" + aMovie.year + ")");
                Console.Write("Please rate the movie above on a scale of one to five stars (*, **, ***, ****, *****) ");

                string userInput = Console.ReadLine();
                string lowerCaseUserInput = userInput.ToLower();

                if (lowerCaseUserInput != "exit") // checking to see if the user typed in exit to end the program
                {
                    aMovie.rating = userInput; // if user did not type in exit, the rating is stored
                }
                else
                {
                    break;
                }
 
            }

            WriteJsonData(theMovies, jsonFolder); // sending the updated theMovies list to be written back to JSON files. Also sending the jsonFolder variable for the location of the JSON folder

        }


        static List<Movie> ReadJsonData(string jsonFolder) 
        {
            // this method 

            string[] jsonFiles = Directory.GetFiles(jsonFolder); // opens the folder specified and gets a list of all of the files in it, and returns an array of strings

            List<Movie> movieList = new List<Movie> { }; // list to hold all of the Movie objects, to be pulled from the jsonFiles array
            Movie oneMovie = new Movie(); // placeholder movie object
            string jsonData = ""; // initializing variable for the raw JSON string data read from the file


            // copying all the JSON data into a list of objects
            // one JSON file per element in the list
            
            foreach (var jsonFile in jsonFiles)
            {
                jsonData = File.ReadAllText(jsonFile); // extracting all of the raw data from the JSON file
                oneMovie = JsonSerializer.Deserialize<Movie>(jsonData); // converting the raw JSON string into a Movie object
                movieList.Add(oneMovie); // adding oneMovie object to the list of movies
            }

            return movieList; // returning movieList to the Main method to be used for user input
        }


        static void WriteJsonData(List<Movie> movieList, string jsonFolder)
        {
            // this method takes the updated theMovies list and writes the new data back to the JSON files one by one. Also receives the input jsonFolder from the Main method for the location of the JSON folder


            var jsonObjects = new JsonSerializerOptions { WriteIndented = true }; // using to indent the data to match original formatting

            foreach(var aMovie in movieList) // for each movie in the movieList, write the updated data back to json file
            {
                    string jsonData = JsonSerializer.Serialize(aMovie, jsonObjects);
                    StreamWriter jsonWriter = new StreamWriter(jsonFolder + "/" + aMovie.id + ".json");

                    jsonWriter.Write(jsonData);

                    jsonWriter.Close();
            }

        }

    }

    
    class Movie
    {
        // this Movie class is the class used as a template to store the contents of the JSON files as objects. The formatting and variable types match 100% with the json files
        public string id { get; set; }
        public string title { get; set; }
        public string year { get; set; }
        public string runtime { get; set; }
        public string genre { get; set; }
        public string rating { get; set; }

    }

}
