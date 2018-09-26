﻿/* NAME: BackButton.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Template class for the main menu buttons
 * VERSION NUMBER: 1.0.0
 * DATE MODIFIED: 5/28/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added implementation of the Button.cs code for returning to the main menu
*/
   
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButton : Button
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Activate button function when releasing mouse on button
    void OnMouseUpAsButton()
    {
        // Switch sprite to unclicked when mouse is released
        GetComponent<SpriteRenderer>().sprite = unclicked;
        // Go to game room
        SceneManager.LoadScene(0);
    }
}
