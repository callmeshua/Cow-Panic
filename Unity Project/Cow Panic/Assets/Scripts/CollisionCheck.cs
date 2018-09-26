/* NAME: CollisionCheck.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Provides a method checkCollision that checks for overlap with other objects
 * VERSION NUMBER: 1.1.0
 * DATE MODIFIED: 4/9/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added a GameObject obj to keep track of the GameObject calling the checkCollision function
 *  ---- 1.0.0 ----
 *      - Removed constructor and the obj GameObject so that the CollisionCheck is now treated like a component and not its own object
 *      - Updated the version number since the Version History said one thing, the Version Number said another, and I had a completely different idea
 *      - Made the script now 1.0.0 because the script already does everything it needs to do, so I have no idea why it wasn't already 1.0.0
 *  ---- 1.0.1 ----
 *      - Added a comment on the return statement
 *  ---- 1.1.0 ----
 *      - checkCollision now returns a List<GameObject> of all objects overlapping the implicit parameter using the Physics2D function OverlapAreaAll
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionCheck : MonoBehaviour
{
    // Meant for easy reference to the object's hitbox
    BoxCollider2D collider;

	// Use this for initialization
	void Start ()
    {
        // Set collider to the object's hitbox
        collider = GetComponent<BoxCollider2D>();
	}

    // Update is called once per frame
    void Update()
    {

    }

    // PURPOSE: To check collisions with another object on a specified layer using Raycasts on every pixel inside the object
    // PARAMTERS: Takes in a LayerMask on which the Raycasts will be cast
    // RETURNS: Returns a List<GameObject> of all objects found
    public List<GameObject> checkCollision(LayerMask mask)
    {
        // -------- COLLISION CHECK --------
        // Save the hitbox's size into a two-dimensional vector
        Vector2 size = collider.size;
        // Save the hitbox's origin
        Vector2 center = transform.position;
        // Save the hitbox's origin's offset from the hitbox's center
        Vector2 offset = collider.offset;
        // Determine the actual center of the hitbox
        center += offset;
        // Find the left side's x coordinate by subtracting half the hitbox's width from the center coordinate
        float left = center.x - size.x / 2;
        // Find the right side's x coordinate by subtracting half the hitbox's width from the center coordinate
        float right = center.x + size.x / 2;
        // Store the top side of the hitbox
        float top = center.y + size.y / 2;
        //Store the bottom side of the hitbox
        float bottom = center.y - size.y / 2;
        
        // Obtain an array of all colliders overlapping the area between (left, top) and (right, bottom) on the LayerMask mask
        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(left, top), new Vector2(right, bottom), mask);

        // Add the GameObjects corresponding to each collider in colliders to a list to be returned
        List<GameObject> results = new List<GameObject>();
        for(int i = 0; i < colliders.Length; i++)
        {
            results.Add(colliders[i].gameObject);
        }

        return results;
    }

}
