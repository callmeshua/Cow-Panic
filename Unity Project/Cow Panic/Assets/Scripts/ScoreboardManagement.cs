/* NAME: ScoreboardManagement.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Sets and tracks the player's lives
 * VERSION NUMBER: 0.2.0
 * DATE MODIFIED: 5/17/2016
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added code to load all the scores and names from file
 *      - The Text field in the scoreboard scene now is updated to show all ten names and scores
 *
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardManagement : MonoBehaviour
{
    // Stores the top ten scores
    int[] topTenScores;
    // Stores the top ten names
    string[] topTenNames;

    public Text scoreboard;

	// Use this for initialization
	void Start ()
    {
        GameObject pointsManager = GameObject.Find("PointsManager");

        // Checks if there is a Points Manager object in the scene
        // This will be false if there is not a points manager already in place
        // If no Points Manager is found, add the PointsManagement script to the ScoreboardManagement gameobject
        if(pointsManager != null)
        {
            // Save the scores and names for writing to the scoreboard
            topTenScores = pointsManager.GetComponent<PointsManagement>().getTopTenScores();
            topTenNames = pointsManager.GetComponent<PointsManagement>().getTopTenNames();
        }
        else
        {
            // Create a new Game Object and add PointsManagement to it so it can read from the highScores file
            gameObject.AddComponent<PointsManagement>();

            // Tell the Points Manager to load scores from file
            GetComponent<PointsManagement>().loadScores();

            // Save the scores and names for writing to the scoreboard
            topTenScores = GetComponent<PointsManagement>().getTopTenScores();
            topTenNames = GetComponent<PointsManagement>().getTopTenNames();
        }

        // Destroy the PointsManager object if it exists so the score can be reset with each play
        if (pointsManager != null)
        {
            GameObject.Destroy(pointsManager);
        }

        // Set scoreboard as a referene to the scoreboard display Text field in teh scene
        scoreboard = GameObject.Find("Scoreboard").GetComponent<Text>();
        
        // The text to display on the scoreboard
        string display = "";

        for(int i = 0; i < 10; i++)
        {
            display += i + 1;
            display += ". " + topTenScores[i] + "    " + topTenNames[i] + "\n";
        }

        scoreboard.text = display;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
