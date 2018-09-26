/* NAME: PlayerLife.cs
 * AUTHOR: Daniel Sipe
 * PURPOSE: Sets and tracks the player's lives
 * VERSION NUMBER: 0.3.0
 * DATE MODIFIED: 5/25/2016
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *  ---- 0.2.0 ---- (Jake Carfagno)
 *      - Renamed script file to PlayerLife
 *      - Renamed the subtractlife function to subtractLife
 *      - Added a check when the player's lives run out for whether a new top-ten score has been set (move to the score-submission screen if true and the scoreboard scene if false)
 *      - Modified the format
 *      - Changed documentation of the subtractLives method
 *      - Added switch to scoreboard or score submission screen using SceneManager after lives run out
 * ---- 0.3.0 ----
 * Added the script to print current lives to the screen
 * ---- 0.4.0 ----
 * Now uses heart Images instead of text!
*/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{

    // Stores the Player's current Life
    public int currentLives;
    //Stores the Heart Sprites
    public Sprite Heart5, Heart4, Heart3, Heart2, Heart1;
    //Stores the image object 
    Image Lives;

	void Start ()
    {
        //declares the image box
        Lives = GameObject.Find("/Canvas/Lives").GetComponent<Image>();
        //Sets the player's current life to 5
        currentLives = 5;
       

    }

    // Update is called once per frame
    void Update ()
    {
        //Updates The lives image Continueously
        if (currentLives == 5)
        {
            Lives.sprite = Heart5;
        }
        if (currentLives == 4)
        {
            Lives.sprite = Heart4;
        }
        if (currentLives == 3)
        {
            Lives.sprite = Heart3;
        }
        if (currentLives == 2)
        {
            Lives.sprite = Heart2;
        }
        if (currentLives == 1)
        {
            Lives.sprite = Heart1;
        }
        // Ends the game if the player's life reaches 0
        if (currentLives <= 0)
        {
            // Check if a high score has been achieved
            // If a new top-ten score has been achieved, move to the new-score scene to enter initials and submit the score
            // Else move to the scoreboard scene
            if(GameObject.Find("PointsManager").GetComponent<PointsManagement>().isTopTen())
            {
                // Move to the new-score scene (whose index is 2 in the Build Settings)
                SceneManager.LoadScene(3);
            }
            else
            {
                // Move to the scoreboard scene (whose index is 1 in the Build Settings)
                SceneManager.LoadScene(2);
            }
        }

	}
    // PURPOSE: Subtract lives from the player's total when a cow leaves the screen
    // PARAMTERS: int lives: the number of lives to subtract from CurrentLife
    // RETURNS: Returns no values
    public void subtractLife(int lives)
    {
        currentLives -= lives;
    }
}
