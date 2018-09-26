/* NAME: NewScore.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Obtains the player's initials and score after a game for passing to the ScoreManager
 * VERSION NUMBER: 0.3.0
 * DATE MODIFIED: 5/11/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added the private int playerScore that stores the player's current score
 *      - Added a function addPoints() that takes in a points value and adds it to the playerScore int
 *  ---- 0.2.0 ----
 *      - Added a playerScore int to store the player's score, which is set in the constructor
 *      - Added Text objects nameDisplay and scoreDisplay that show the player's score and initials
 *      - Added ability for player to set initials using ASDW controls
 *  ---- 0.3.0 ----
 *      - Replaced some if-else statements with ternary operators
 *      - Added comments
 *		- The selected initial is now bold and larger font while others are set to a default font
 *		- Added new constants to use for selected and unselected font size
 * TO-DO:
 *  - Save the total points so that the player can enter intials and the points value in the next screen
 *  - Add a scoreboard feature to save the player's score to a text file
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewScore : MonoBehaviour
{
    // The player's final score
    int playerScore;

    // The GameObject for the PointsManager for easy access
    GameObject pointsManager;
    
    // A string of all the chars available for the initials
    private string availableChars;

    // The index of the first, second, and third initials
    // The value of firstChar refers to the index of the first character within the availableChars string, such that
    // if firstChar == 5, the first character will be 'F'
    private int firstChar;
    private int secondChar;
    private int thirdChar;

    // Stores the currently selected initial (0 = firstChar, 1 = secondChar, 2 = thirdChar)
    // The one that's selected is the one that is edited when pressing 'W' and 'S'
    // Change which one is selected by pressing 'A' and 'D'
    private int selected;

    // Create variables to store the Text objects for later reference
    public Text firstDisplay, secondDisplay, thirdDisplay;
    public Text scoreDisplay;

    // The sizes for selected and unselected initials
    public int SELECTED_SIZE;
    public int UNSELECTED_SIZE;

    // Use this for initialization
    void Start ()
    {
        // Saves the PointsManager into an object and then retrieves the player's score
        pointsManager = GameObject.Find("PointsManager");
        playerScore = pointsManager.GetComponent<PointsManagement>().getScore();

        // Default the initials to AAA
        firstChar = 0;
        secondChar = 0;
        thirdChar = 0;

        availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        // Find the text displays for the three initials and score and store them as objects
        firstDisplay = GameObject.Find("/Canvas/First_Display").GetComponent<Text>();
        secondDisplay = GameObject.Find("/Canvas/Second_Display").GetComponent<Text>();
        thirdDisplay = GameObject.Find("/Canvas/Third_Display").GetComponent<Text>();
        scoreDisplay = GameObject.Find("/Canvas/Score_Display").GetComponent<Text>();

        // Display the player's score
        scoreDisplay.text = "" + playerScore;

        // If selectedSize does not have a value assigned, default it to 72
        if(!(SELECTED_SIZE > 0))
        {
            SELECTED_SIZE = 128;
        }
        if(!(UNSELECTED_SIZE > 0))
        {
            UNSELECTED_SIZE = 108;
        }

        // Start with the first character selected, making it bolded and larger
        selected = 0;
        firstDisplay.fontSize = SELECTED_SIZE;
        firstDisplay.fontStyle = FontStyle.Bold;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Move cursor to the left
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Check if the cursor is already at the left-most character
            // Move to the left if not or move to right-most character
            selected = ((selected > 0) ? selected - 1: 2);
        }
        // Move cursor to the right
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Move the cursor to the right, resetting it to first character if already at right
            selected = (selected + 1) % 3;
        }

        // If the selected initial has changed this frame, update display so the new selected is bolded and enlarged
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            // Check which initial is selected and make the selected one larger and bold
            switch (selected)
            {
                // First initial is selected
                case 0:
                    // Increase first initial's font size and bold it
                    firstDisplay.fontSize = SELECTED_SIZE;
                    firstDisplay.fontStyle = FontStyle.Bold;

                    // Shrink second and third initials' font sizes and unbold them
                    secondDisplay.fontSize = UNSELECTED_SIZE;
                    secondDisplay.fontStyle = FontStyle.Normal;

                    thirdDisplay.fontSize = UNSELECTED_SIZE;
                    thirdDisplay.fontStyle = FontStyle.Normal;
                    break;
                // Second initial is selected
                case 1:
                    // Increase second initial's font size and bold it
                    secondDisplay.fontSize = SELECTED_SIZE;
                    secondDisplay.fontStyle = FontStyle.Bold;

                    // Shrink first and third initials' font sizes and unbold them
                    firstDisplay.fontSize = UNSELECTED_SIZE;
                    firstDisplay.fontStyle = FontStyle.Normal;

                    thirdDisplay.fontSize = UNSELECTED_SIZE;
                    thirdDisplay.fontStyle = FontStyle.Normal;
                    break;
                // Third initial is selected
                case 2:
                    // Increase third initial's font size and bold it
                    thirdDisplay.fontSize = SELECTED_SIZE;
                    thirdDisplay.fontStyle = FontStyle.Bold;

                    // Shrink first and second initials' font sizes and unbold them
                    firstDisplay.fontSize = UNSELECTED_SIZE;
                    firstDisplay.fontStyle = FontStyle.Normal;

                    secondDisplay.fontSize = UNSELECTED_SIZE;
                    secondDisplay.fontStyle = FontStyle.Normal;
                    break;
            }
        }

        // Cycle selected character up alphabet (backwards towards 'A')
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (selected)
            {
                // Edit the firstChar value
                case 0:
                    // Decrement firstChar if possible, otherwise set to last character
                    firstChar = ((firstChar > 0) ? firstChar - 1 : availableChars.Length - 1);
                    break;
                // Edit the secondChar value
                case 1:
                    // Decrement secondChar if possible, otherwise set to last character
                    secondChar = ((secondChar > 0) ? secondChar - 1 : availableChars.Length - 1);
                    break;
                // Edit the thirdChar value
                case 2:
                    // Decrement thirdChar if possible, otherwise set to last character
                    thirdChar = ((thirdChar > 0) ? thirdChar - 1 : availableChars.Length - 1);
                    break;
            }
        }

        // Cycle selected character down alphabet (forwards towards 'Z')
        if (Input.GetKeyDown(KeyCode.S))
        {
            switch (selected)
            {
                // Edit the firstChar value
                case 0:
                    firstChar = (firstChar + 1) % availableChars.Length;
                    break;
                // Edit the secondChar value
                case 1:
                    secondChar = (secondChar + 1) % availableChars.Length;
                    break;
                case 2:
                    thirdChar = (thirdChar + 1) % availableChars.Length;
                    break;
            }
        }

        // Set the displays to show their respective characters
        firstDisplay.text = "" + availableChars[firstChar];
        secondDisplay.text = "" + availableChars[secondChar];
        thirdDisplay.text = "" + availableChars[thirdChar];
        

        // Submit the three initials for saving using the 'Enter' key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string name = "";

            // Add the firstChar initial to name
            name += availableChars[firstChar];
            // Add the secondChar intial to name
            name += availableChars[secondChar];
            // Add the thirdChar initial to name
            name += availableChars[thirdChar];

            // Add the new score in the PointsManager
            pointsManager.GetComponent<PointsManagement>().addNewScore(playerScore, name);

            // Switches to the scoreboard display, which has a build index of 1
            SceneManager.LoadScene(2);
        }
    }
}
