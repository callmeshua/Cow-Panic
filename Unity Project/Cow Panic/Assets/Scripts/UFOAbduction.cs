/* NAME: UFOAbduction.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Enables the UFO to abduct cows by swiping up from the ground to the UFO
 * VERSION NUMBER: 1.3.0
 * DATE MODIFIED: 5/29/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added a To-Do section
 *      - Added a resetTractorBeam function to be used later in the tractor beam's destructor
 *  ---- 0.2.0 ----
 *      - Successfully made it so pressing 'S' spawns a yellow rectangle
 *      - Basically, the tractor beam now has a working origin at the top-center of the beam
 *      - In addition, it spawns toward the bottom of the UFO with a hitbox and a sprite
 *      - Moved UFO's origin to the bottom where the base of the tractor beam should be
 *  ---- 0.2.1 ----
 *      - Added a function isAbducting() that returns whether the tractorBeam is not null to be used in UFO controller class
 *  ---- 0.3.0 ----
 *      - Removed underscore from the class name
 *      - Added a CollisionCheck object colChecker that is used to see if the UFO is colliding with a cow
 *      - When a collision with a cow is found, the cow is destroyed with Destroy(<the cow>)
 *  ---- 1.0.0 ----
 *      - Added a CollisionCheck component and removed the CollisionCheck object colChecker
 *      - tractorBeam now gets a CollisionCheck component on creation
 *      - UFO now destroys any cows that touch it
 *  ---- 1.0.1 ----
 *      - Added an array of Sprites to be passed into the BeamAnimator for use in the tractor beam's animation
 *      - Added a BeamAnimator component to the tractor beam upon its creation
 *  ---- 1.0.2 ----
 *      - Added a call to GetComponent<Moove>().abductCow() on collision with a cow to set that cow's isAbducted boolean to true
 *  ---- 1.1.0 ----
 *      - Added a call to the PointsManager's addPoints() function, passing the colliding cow's points value as a parameter
 *      - Added the pointsManager object of type PointsManager for referral to the PointsManager script attached to the UFO
 *  ---- 1.1.1 ----
 *      - Removed the UFO's collision check for cows and added it to the Moove script instead
 *      - Created a new layer called "Tractor Beam" and added the tractor beam to that layer upon creation
 *  ---- 1.1.2 ----
 *      - Removed the commenting on the code that gives the tractor beam a CollisionCheck component
 *      - Added check for collisions with cows that also increases the player's score and destroys the cow
 *  ---- 1.2.0 ----
 *      - Removed the PointsManager component from the UFOAbduction script and added a call to the PointsManagement script on the new PointsManager object
 *  ---- 1.3.0 ----
 *      - Added a call to the cows' shrink() function on collision to begin their destroying animation
 *      - Player can now recall the tractor beam using 'W', which causes it to recede before disappearing
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UFOAbduction : MonoBehaviour
{
    [HideInInspector]
    // A GameObject storing the currently deployed tractor beam
    private GameObject tractorBeam;

    [HideInInspector]
    // Stores the gameObject for the PointsManagement object to easily refer to it later
    GameObject pointsManager;

    // Declared in the Inspector and assigned to the tractorBeam to set its sprite on creation
    // Passed into the tractorBeam's BeamAnimator component to give the animation
    // Contains an array of all the frame images on the animation
    public Sprite[] tractorBeamSprites;

	// Use this for initialization
	void Start ()
    {
        pointsManager = GameObject.Find("PointsManager");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    // Check for the player to press 'S' for debugging purposes
        if(Input.GetKeyDown(KeyCode.S) && tractorBeam == null)
        {
            abduct();
        }
        // Debugging remove tractor beam
        if(Input.GetKeyDown(KeyCode.W) && tractorBeam != null)
        {
            // Destroy(tractorBeam);
            tractorBeam.GetComponent<BeamBehavior>().retractorBeam();
        }

        // Check for collisions with cows
        List<GameObject> cows = GetComponent<CollisionCheck>().checkCollision(LayerMask.GetMask("Cow"));
        for(int i = cows.Count - 1; i > -1; i--)
        {
            // Increase the total score by the points values of each cow touching the UFO
            pointsManager.GetComponent<PointsManagement>().addPoints(cows[i].GetComponent<Moove>().getPoints());
            // Destroy the current cow object
            cows[i].GetComponent<Moove>().shrink();
        }
	}

    // PURPOSE: Create the abduction object
    // PARAMETERS: no parameters
    // RETURNS: returns nothing
    void abduct()
    {
        // Creates a tractor beam object
        tractorBeam = new GameObject("prototype_Abduction");
        tractorBeam.layer = LayerMask.NameToLayer("Tractor Beam");
        // Give the tractor beam a hitbox and add the script to give it behavior
        tractorBeam.AddComponent<BeamBehavior>();
        tractorBeam.AddComponent<BoxCollider2D>();
        // Sets the tractor beam's hitbox to be the size of the sprite
        tractorBeam.GetComponent<BoxCollider2D>().size = new Vector2(tractorBeamSprites[0].bounds.size.x / 2, tractorBeamSprites[0].bounds.size.y);
        // Adjusts the hitbox's offset so that it lines up properly with the sprite
        tractorBeam.GetComponent<BoxCollider2D>().offset = new Vector2(0, -tractorBeamSprites[0].bounds.size.y / 2);
        tractorBeam.AddComponent<CollisionCheck>();

        // Set the tractor beam's position
        tractorBeam.transform.position = new Vector2(transform.position.x, transform.position.y);

        // -------- TRACTOR BEAM'S SPRITE HANDLING --------
        // Give the tractor beam object a SpriteRenderer
        tractorBeam.AddComponent<SpriteRenderer>();
        // Add a component of the script BeamAnimator
        tractorBeam.AddComponent<BeamAnimator>();
        tractorBeam.GetComponent<BeamAnimator>().tractorBeamSprites = tractorBeamSprites;
    }

    // PURPOSE: Sets the tractorBeam object to null; called by the tractor beam as it expires so the UFO knows it can spawn another tractor Beam
    // PARAMETERS: no parameters
    // RETURNS: returns nothing
    public void resetTractorBeam()
    {
        tractorBeam = null;
    }

    // PURPOSE: Allows other classes to know if the UFO's tractor beam is currently deployed
    // PARAMETERS: no parameters
    // RETURNS: returns true if tractorBeam does not equal null and false if the tractorBeam equals null
    public bool isAbducting()
    {
        return tractorBeam != null;
    }
}
