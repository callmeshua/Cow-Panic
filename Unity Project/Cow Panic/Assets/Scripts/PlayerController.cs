/* NAME: PlayerController.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Gives the UFO movement controls 'A' and 'D' as well as abduction controls
 * VERSION NUMBER: 1.1.0
 * DATE MODIFIED: 5/5/16
 * VERSION HISTORY:
 * ---- 1.0.0 ----
 *  - Created the file
 *  - Added support for moving left and right with a check that the UFO is not currently abducting
 *  - Added abduction when pressing 'S' and a debug button 'W' to destroy the tractor beam
 *  - Removed all code for ducking and diving and jumping and stuff (cause this ain't no platformer!)
 * ---- 1.1.0 ----
 *  - Added a check that the UFO is not going offscreen by moving left or right
 *  - Created a constant float UFO_SPEED to store the UFO's movement speed for easy changing
 *  - Removed framerate dependence using Time.deltaTime
 * ---- 1.1.1 ----
 *  - UFO now stays on the same altitude when cows hit it
 */ 
 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // A constant holding the UFO speed when moving left or right
    // Units are pixels per second (which is why it's insanely large 600 by default)
    public float UFO_SPEED;

    // Keeps the UFO from floating up magically all the freaking time whenever a cow touches it because Unity's Rigidbodies are evil
    private float startingHeight;

	// Use this for initialization
	void Start ()
	{
        // Set the UFO_SPEED value to 600 if not already assigned a value greater than 0
        // This means the UFO will move ten pixels per frame or so, which is around what it was before
		if(UFO_SPEED <= 0)
        {
            UFO_SPEED = 600;
        }

        // Set startingHeight to the starting height of the UFO
        startingHeight = transform.position.y;
	}

	// Update is called once per frame
	void Update ()
	{
        // Checks that the player is pressing 'A', is not currently abducting, and will not move out of the screen boundaries before allowing the player to move left]
        // Transform.localScale ensures that any scaling done to the UFO will be taken into account and the UFO will still stay right within screen borders
        if (Input.GetKey(KeyCode.A) && !GetComponent<UFOAbduction>().isAbducting()
            && gameObject.transform.position.x - (gameObject.GetComponent<BoxCollider2D>().size.x / 2) * transform.localScale.x
            - gameObject.GetComponent<BoxCollider2D>().offset.x - UFO_SPEED * Time.deltaTime >= -960)
        {
            transform.position = new Vector2(transform.position.x - UFO_SPEED * Time.deltaTime, transform.position.y);
        }
        if (Input.GetKey(KeyCode.D) && !GetComponent<UFOAbduction>().isAbducting()
            && gameObject.transform.position.x + (gameObject.GetComponent<BoxCollider2D>().size.x / 2) * transform.localScale.x
            + gameObject.GetComponent<BoxCollider2D>().offset.x + UFO_SPEED * Time.deltaTime <= 960)
        {
            transform.position = new Vector2(transform.position.x + UFO_SPEED * Time.deltaTime, transform.position.y);
        }

        // Maintain the height with each update
        transform.position = new Vector2(transform.position.x, startingHeight);
    }
}