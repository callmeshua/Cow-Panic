/* NAME: Button.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Template class for the main menu buttons
 * VERSION NUMBER: 1.0.0
 * DATE MODIFIED: 5/25/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added skeleton code for button behavior (change sprite on click and activate on mouse release)
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public Sprite unclicked, clicked;

    // Use this for initialization
	void Start ()
    {
        GetComponent<SpriteRenderer>().sprite = unclicked;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // When clicked, set the animator's bool isClicked to true
    void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = clicked;
    }

    // Change animator sprite back to non-clicked when the mouse cursor leaves the button
    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = unclicked;
    }

    // Activate button function when releasing mouse on button
    void OnMouseUp()
    {
        // Change sprite back to unclicked
        GetComponent<SpriteRenderer>().sprite = unclicked;
        // Add code for specific button function
    }
}
