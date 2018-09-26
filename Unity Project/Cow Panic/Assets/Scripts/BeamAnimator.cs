/* NAME: BeamAnimator.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Animates the UFO based on its states isExtended and isRetracting
 * VERSION NUMBER: 1.0.0
 * DATE MODIFIED: 2/22/16
 * VERSION HISTORY:
 * ---- 1.0.0 ----
 *      - Created the file
 *      - Added an array of all Sprites in the tractor beam's animation
 *      - Script now updates the Sprite of the tractor beam based on its state
*/

using UnityEngine;
using System.Collections;
using System;

public class BeamAnimator : MonoBehaviour
{
    public Sprite[] tractorBeamSprites;

    private SpriteRenderer renderer;

    public float index;

	// Use this for initialization
	void Start ()
    {
        // Store the SpriteRenderer for easy access
        renderer = GetComponent<SpriteRenderer>();

        // Set index, the frame of the animation to be displayed, to zero (start at the beginning of the animation)
        index = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If index is less than the amount of frames in the animation, the tractor beam is not fully extended, and the tractor beam is not retracting
        // Then extend the tractor beam
        if(index < tractorBeamSprites.Length && !GetComponent<BeamBehavior>().getExtended() && !GetComponent<BeamBehavior>().getRetracting())
        {
            // Add the amount of time since the last frame update times the total amount of frames to the index variable
            // Since Time.deltaTime is a float representing number of seconds since the last frame, this ensures that the animation will play at one frame per update (and 60 frames per second)
            index += Time.deltaTime * tractorBeamSprites.Length;
            // Set the current sprite to the index'th (rounded to a whole number) Sprite in the array
            renderer.sprite = tractorBeamSprites[(int)index];
            
            // If the index is greater than or equal to the last Sprite's location, tell BeamBehavior the tractor beam has fully extended
            if(index >= tractorBeamSprites.Length - 1)
            {
                GetComponent<BeamBehavior>().setExtended(true);
            }
        }
        else if(index >= tractorBeamSprites.Length - 1 && !GetComponent<BeamBehavior>().getExtended() && !GetComponent<BeamBehavior>().getRetracting())
        {
            GetComponent<BeamBehavior>().setExtended(true);
        }
        // If the tractor beam should be retracting
        else if(GetComponent<BeamBehavior>().getRetracting())
        {
            // Instead subtract the change in time from the current frame of the animation to play
            index -= Time.deltaTime * tractorBeamSprites.Length;
            // Display the current frame
            renderer.sprite = tractorBeamSprites[(int)index];

            // if the index is less than zero, the animation has again reached the beginning, and the tractor beam may be destroyed
            if(index < 0)
            {
                Destroy(gameObject);
            }
        }
	}
}
