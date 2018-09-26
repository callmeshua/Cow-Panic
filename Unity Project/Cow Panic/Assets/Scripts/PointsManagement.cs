/* NAME: PointsManagement.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Track the player's score, display it in-game, and save it after the round ends
 * VERSION NUMBER: 1.0.0
 * DATE MODIFIED: 5/17/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added the private int playerScore that stores the player's current score
 *      - Added a function addPoints() that takes in a points value and adds it to the playerScore int
 *  ---- 0.2.0 ----
 *      - Renamed to PointsManagement
 *      - Added a call to DontDestroyOnLoad so the PointsManager object is retained when going from the in-game scene to the after-game scene
 *  ---- 0.2.1 ----
 *      - Added a getScore() method that returns the player's score
 *  ---- 1.0.0 ----
 *      - Added an int array topTenScores to store the top ten scores
 *      - Added a string array topTenNames to store the names corresponding to top ten scores
 *      - Added a method isTopTen to check if the player's score is in the top ten (to determine if the game should proceed to new score room after game ends or not)
 *      - Added a function loadFile that loads in the top ten scores and the names going with them from a file called highScores.txt
 *      - Added a function sortScores that sorts the topTenScores array and shifts the topTenNames with it to keep them synced
 *      - Added a function addNewScore that takes in a score and name and adds both to the topTen arrays
 *      - Added two accessor functions for topTenScores and topTenNames
 *      - Added a check in the loadScores() function to ensure that topTenScores and topTenNames arrays have been declared before adding values to them
 *  ---- 1.1.0 ----
 *      - Added code to update score in the gameCowPanic scene (since Sourcetree refuses to just give me the version Dan made)
 *      - Added a check to ensure it only tries to update the score when in the gameCowPanic scene
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointsManagement : MonoBehaviour
{
    // Stores the player's current score
    public int playerScore;

    // Stores the top ten scores after being loaded in from file
    public int[] topTenScores;

    // Stores the names corresponding to the top ten scores after being loaded in from a file
    public string[] topTenNames;

    // The Text object showing the player's current score during the game
    private Text scoreDisplay;

	// Use this for initialization
	void Start ()
    {
        // Ensures the gameObject PointsManager is not destroyed when the round ends
        DontDestroyOnLoad(gameObject);

        // Sets the player's current score to 0 as default
        playerScore = 0;

        // Declare both arrays with 10 spaces
        topTenScores = new int[10];
        topTenNames = new string[10];

        loadScores();

        if(SceneManager.GetActiveScene().name == "gameCowPanic")
        {
            scoreDisplay = GameObject.Find("/Canvas/Score").GetComponent<Text>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Check that the current room is the gameCowPanic scene and update the score text if it is
        if(scoreDisplay != null)
        {
            scoreDisplay.text = "Score: " + playerScore;
        }
	}

    // PURPOSE: Add the parameter points to the current total score
    // PARAMETERS: Takes in an integer points to add to the current playerScore value
    // RETURNS: Returns no value
    public void addPoints(int points)
    {
        playerScore += points;
    }

    // PURPOSE: To retrieve the player's score for saving
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns the playerScore integer
    public int getScore()
    {
        return playerScore;
    }

    // PURPOSE: To determine if the player's score at the end of the round qualifies as a high score on the leaderboard
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns true if the player's score is greater than at least one score and false otherwise
    public bool isTopTen()
    {
        // Assuming the top scores array is sorted, compare playerScore to the last (lowest) score in the array and return true if the playerScore is greater
        if(playerScore > topTenScores[9])
        {
            return true;
        }

        // Return false otherwise
        return false;
    }

    // PURPOSE: To load scores from file
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns true if successful at reading from file and false otherwise
    public bool loadScores()
    {
        // Just in case of the errors that will probably appear
        try
        {
            // Creates an object representing the highScores.txt file to read in data from
            StreamReader inFile;

            // Checks if highScores.txt already exists and creates it if no file is found
            if (!File.Exists(Application.dataPath + "/Resources/highScores.txt"))
            {
                createBlankScores();
                inFile = File.OpenText(Application.dataPath + "/Resources/highScores.txt");
            }
            else
            {
                // Otherwise stores the file as inFile for reading
                inFile = File.OpenText(Application.dataPath + "/Resources/highScores.txt");
            }

            // A string to store the current line being read in from the file
            string line;

            if(topTenScores == null)
            {
                topTenScores = new int[10];
            }
            if(topTenNames == null)
            {
                topTenNames = new string[10];
            }

            for(int i = 0; i < 10; i++)
            {
                line = inFile.ReadLine();

                // Split the line into strings divided at the space
                // parts[0] is the score (needs to be parsed)
                // parts[1] is the three initials
                string[] parts = line.Split(' ');

                // Convert the score from string to integer
                int score = int.Parse(parts[0]);
                topTenScores[i] = score;
                topTenNames[i] = parts[1];
            }

            // Close the file
            inFile.Close();

            // Sort the two arrays
            sortScores();
            
            // Return true because all went well
            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log("Error");
            // Well... crap
            return false;
        }
    }

    // PURPOSE: To sort the score and name arrays
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns no value
    private void sortScores()
    {
        // By default, assumes the arrays are sorted
        bool isSorted = true;

        do
        {
            // Check for any swaps that can be made first
            for (int i = 0; i < topTenScores.Length - 1; i++)
            {
                // Check if the current score is less than the next score
                if (topTenScores[i] < topTenScores[i + 1])
                {
                    // Swap the current score and name with the next score and name using a temporary int and string
                    int tempScore = topTenScores[i];
                    string tempName = topTenNames[i];

                    topTenScores[i] = topTenScores[i + 1];
                    topTenNames[i] = topTenNames[i + 1];

                    topTenScores[i + 1] = tempScore;
                    topTenNames[i + 1] = tempName;
                }
            }

            isSorted = true;

            // Check if the list is sorted from greatest score first to lowest score last
            for (int i = 0; i < topTenScores.Length - 1; i++)
            {
                // Set isSorted to false if the arrays are still out of order so the loop runs again
                if (topTenScores[i] < topTenScores[i + 1])
                {
                    isSorted = false;
                }
            }
        } while (!isSorted);
    }

    // PURPOSE: To add a new score and name to the scoreboard
    // PARAMETERS: int newScore: the new score to be added to topTenScores; string newName: the name to be added to topTenNames
    // RETURNS: Returns no values
    public void addNewScore(int newScore, string newName)
    {
        // Replace the last (lowest) score in the list with the new score
        topTenScores[9] = newScore;
        // Replace the last name (corresponding with the previous lowest score) with the new name
        topTenNames[9] = newName;

        // Sort the lists
        sortScores();

        writeScores();
    }

    // PURPOSE: To write all the scores to file
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns no values
    public void writeScores()
    {
        // Create a StreamWriter to write the scores to the file
        StreamWriter outFile = File.CreateText(Application.dataPath + "/Resources/highScores.txt");


        for(int i = 0; i < 10; i++)
        {
            outFile.WriteLine(topTenScores[i] + " " + topTenNames[i]);
        }

        outFile.Close();
    }

    // PURPOSE: Creates highScores.txt with default scores 0 and names AAA
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns no values
    public void createBlankScores()
    {
        StreamWriter newFile = File.CreateText(Application.dataPath + "/Resources/highScores.txt");

        // Fills in the default file with 10 lines of 0 AAA, the default value of scores
        for(int i = 0; i < 10; i++)
        {
            newFile.WriteLine("0 AAA");
        }

        newFile.Close();
    }

    // PURPOSE: To provide access for scoreboard object to the top ten scores
    // PARAMETERS: Takes in no parameters
    // RETURNS: returns an int[] of the top ten scores
    public int[] getTopTenScores()
    {
        return topTenScores;
    }

    // PURPOSE: To provide access for scoreboard object to the names paired with top ten scores
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns a string[] of the top ten names
    public string[] getTopTenNames()
    {
        return topTenNames;
    }
}
