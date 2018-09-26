/* NAME: BeamBehavior.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Establishes the behavior for the tractor beam so it grows until it reaches a certain size and despawns; sucks up cows and sends them towards the UFO
 * VERSION NUMBER: 1.1.0
 * DATE MODIFIED: 4/13/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Copied the To-Do List from the UFO_Abduction script
 *  ---- 0.2.0 ----
 *      - Added a function setParentUFO() that takes in a GameObject parameter and assigns the private GameObject parentUFO to the parameter value
 *      - Added UFO tracking so the tractor beam's coordinates are always equal to the parent UFO's coordinates
 *  ---- 0.3.0 ----
 *      - Removed the sync of tractor beam location and UFO because UFO cannot move when the tractor beam is deployed
 *      - Finished rewriting the collision detection using Raycasts so that Rigidbody2D can be avoided, borrowing code from a different project
 *      - Completely removed the onCollisionEnter code
 *  ---- 0.4.0 ----
 *      - Added a List to keep track of each cow currently in the tractor beam
 *      - Added a List of Vector2s that matches with each cow in the tractor beam and contains their velocities towards the UFO
 *      - Removed underscore from the class name
 *      - Removed the CollisionCheck code from this class and moved it to the class CollisionCheck
 *      - Added a CollisionCheck object to be called whenever a collision check is needed
 *      - Added function removeCow to remove a specified GameObject cow from the List cows and its matching velocity from cowVels
 *      - Added a destructor that calls the parentUFO's resetTractorBeam function
 *	---- 0.4.1 ----
 *		- Removed remnants of code from the old collision checking system that was built into this class
 *		- This removal makes the cows move at the right speed when sucked up by the tractor beam (before, they were too fast)
 *  ---- 0.5.0 ----
 *      - Added [HideInInspector] tags to the variables
 *      - Added comments to the Collision Checking in Update() and explained the process undertaken when a cow is found
 *      - Facilitated the removeCow() function with a check for if cows[i] == null in the Update() function when trying to move each cow
 *  ---- 1.0.0 ----
 *      - Made isExtended a private variable
 *      - Added an accessor and mutator for isExtended
 *      - Added an isRetracting variable and created an accessor for it
 *  ---- 1.0.1 ----
 *      - Added a BEAM_DURATION constant that stores the amount of time in seconds that the tractor beam should be fully extended
 *  ---- 1.0.2 ----
 *      - Added a BEAM_SPEED constant that stores the amount of time in frames (60 frames per second) that the cows should take to move from the ground to the UFO when abducted
 *      - Moved collision check with the tractor beam to the cow's Moove script so multiple cows can be moved at once
 *      - Added a addCow() function that takes in a GameObject, adds it to the cows List, and calculates its velocity
 *  ---- 1.1.0 ----
 *      - Added a collision check for cows using the improved version of checkCollision
 *      - The tractor beam now only brings cows up to the UFO and not to the left or right
 *      - Removed the addCow method (not useful anymore)
 *      - Added a call to the cows' fallDown() method in the destructor
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeamBehavior : MonoBehaviour
{
    [HideInInspector]
    // The GameObject UFO that spawns the tractor beam
    // Allows for tracking the UFO's location to follow it and send cows in the right direction
    private GameObject parentUFO;

    [HideInInspector]
    // A List to store all the cow objects the tractor beam is abducting
    private List<GameObject> cows;
    [HideInInspector]
    // A List to store velocities corresponding to each cow as they moove up to the UFO
    private List<Vector2> cowVels;

    [HideInInspector]
    // Is true when the beam is fully extended, meaning the beam's hitbox should be active
    private bool isExtended;
    [HideInInspector]
    // Is set to true when the beam is about to retract so the BeamAnimator can retract it
    private bool isRetracting;
    [HideInInspector]
    // Keeps track of how long in seconds that the beam has been extended
    // Once it has reached three seconds, the BeamAnimator is told to retract
    private float timeExtended;
    // Stores the amount of time in seconds that the tractor beam should stay extended
    private const int BEAM_DURATION = 2;
    // Stores the amount of time in frames (60 in a second) that the cows should take to be sucked up by the tractor beam
    private const int BEAM_SPEED = 60;
	// Use this for initialization
	void Start ()
    {
        // Finds the UFO and sets it to parentUFO so the tractor beam can easily access the UFO's coordinates later
        parentUFO = GameObject.Find("UFO");

        // Instantiate the cows ArrayList
        cows = new List<GameObject>();
        cowVels = new List<Vector2>();

        isExtended = false;
        isRetracting = false;
        timeExtended = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        cows = GetComponent<CollisionCheck>().checkCollision(LayerMask.GetMask("Cow"));

        // If the beam is extended or retracting, move the cows
        if (isExtended || isRetracting)
        {
            for (int i = 0; i < cows.Count; i++)
            {
                // if moo sound isn't already playing, play it!
                if(GameObject.Find("Sound_Moo1(Clone)") == null && GameObject.Find("Sound_Moo2(Clone)") == null && GameObject.Find("Sound_Moo3(Clone)") == null && GameObject.Find("Sound_Moo4(Clone)") == null)
                {
                    GameObject SO = GameObject.Find("SoundObject");
                    SO.GetComponent<SoundManager>().moo();
                }
                
                // Call the cow's stopMoving() function so it does not move
                cows[i].GetComponent<Moove>().stopMoving();
                // Creates a temporary Vector3 of the cow's current position
                Vector3 current = cows[i].transform.position;
                // Detemines the y component of the cow's velocity toward the UFO using 235 as a placeholder value
                float deltaY = 235 * Time.deltaTime;

                // Changes each cow's position each frame by the deltaX and deltaY
                cows[i].transform.position = new Vector3(current.x, current.y + deltaY, current.z);
            }

            // Increase timeExtended by amount of time since last frame
            timeExtended += Time.deltaTime;

            if(timeExtended >= BEAM_DURATION)
            {
                isExtended = false;

                isRetracting = true;
            }
        }
    }

    // -------- DESTRUCTOR --------
    // PURPOSE: Called when the Tractor Beam is destroyed to reset the parentUFO's tractorBeam to null
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns no value
    void OnDestroy()
    {
        parentUFO.GetComponent<UFOAbduction>().resetTractorBeam();

        // Make all cows who were being abducted fall back down
        for(int i = 0; i < cows.Count; i++)
        {
            cows[i].GetComponent<Moove>().fallDown();
        }
    }


    // PURPOSE: Assign the GameObject parentUFO to the inputted parameter for easy tracking of the UFO's position at any time
    // PARAMETERS: takes in a GameObject newUFO to which parentUFO is assigned
    // RETURNS: returns no values
    public void setParentUFO(GameObject newUFO)
    {
        parentUFO = newUFO;
    }

    // PURPOSE: Removes the GameObject cow from the List cows and removes its coresponding velocity from cowVels
    // PARAMETERS: takes in a GameObject cow to remove from the cows List
    // RETURNS: returns no values
    public void removeCow(GameObject cow)
    {
        // Stores the index of cow in the List cows
        int index = -1;

        // Finds the location of cow in cows by looping through the List and comparing each GameObject in cows to cow
        for(int i = 0; i < cows.Count; i++)
        {
            if(cows[i] == cow)
            {
                // Sets index to i if cow is found in cows
                index = i;
            }
        }

        // If cow is in cows, remove both cow and the corresponding velocity
        if(index > -1)
        {
            cows.RemoveAt(index);
            cowVels.RemoveAt(index);
        }
    }

    // PURPOSE: To retrieve the value of the variable isExtended to either true or false; called in the BeamAnimator to check if the beam has already been extended completely in a previous frame
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns the boolean isExtended
    public bool getExtended()
    {
        return isExtended;
    }

    // PURPOSE: To set the value of isExtended; called by BeamAnimator when the animation has completed to tell the beam it can start checking for collisions
    // PARAMETERS: Takes in a bool ext to which isExtended is assigned
    // RETURNS: Returns no value
    public void setExtended(bool ext)
    {
        isExtended = ext;
    }

    // PURPOSE: To retrieve the value of the variable isRetracting to either true or false; called in the BeamAnimator to check if the beam should retract
    // PARAMETERS: Takes in no parameters
    // RETURNS: Returns the boolean isRetracting
    public bool getRetracting()
    {
        return isRetracting;
    }

    // PURPOSE: To retract the tractor beam on demand
    // PARAMETERS: Takes no parameters
    // RETURNS: Returns no value
    public void retractorBeam()
    {
        isExtended = true;
        isRetracting = true;
    }
}