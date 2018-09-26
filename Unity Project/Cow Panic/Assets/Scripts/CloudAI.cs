/* NAME: CloudAI.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Clouds are assigned height, size, speed, and opacity upon spawn and move across the scene
 * VERSION NUMBER: 1.1.0
 * DATE MODIFIED: 5/28/16
 * VERSION HISTORY:
 *  ---- 1.0.0 ----
 *      - Created the file
 *      - Added code to make the clouds spawn in with speed, scale, and height randomly assigned
 *      - Clouds move to the right upon spawning based on their speed
 *  ---- 1.1.0 ----
 *      - Added code to assign random opacity
 *      - Rendering layer now determined by the opacity
*/

using UnityEngine;
using System.Collections;

public class CloudAI : MonoBehaviour
{
    [HideInInspector]
    private int CLOUD_SPEED;    // In pixels per second (pixels per frame * 60)

    [HideInInspector]
    private int CLOUD_SCALE;    // The number to be mulitplied by each dimension of resolution to determine the cloud's new size

    [HideInInspector]
    public float CLOUD_HEIGHT; // The cloud's height

    public float CLOUD_OPACITY; // The cloud's randomly assigned opacity

	// Use this for initialization
	void Start ()
    {
        // Create a Random object to generate a number for speed
        System.Random randGen = new System.Random();

        // Set the CLOUD_SPEED to be between 100 and 500 pixels per second
        CLOUD_SPEED = (randGen.Next() % 400) + 100;

        // Choose a random number for the cloud's scale
        CLOUD_SCALE = (randGen.Next() % 3) + 2;

        // Set the cloud's scale
        transform.localScale = new Vector2(CLOUD_SCALE, CLOUD_SCALE);

        // Set the cloud's starting height anywhere between 190 and 550
        CLOUD_HEIGHT = (float)(randGen.Next() % 360.0) + 190;

        transform.position = new Vector2(transform.position.x, CLOUD_HEIGHT);

        // Randomly choose the cloud's opacity between 50% and 100%
        CLOUD_OPACITY = (float)randGen.NextDouble() % .75F + 0.25F;

        // Set the cloud's opacity by creating a custom color (white with varying opacity)
        GetComponent<SpriteRenderer>().color = new Color(1.0F, 1.0F, 1.0F, CLOUD_OPACITY);

        // Set the cloud's layer between -100 and -200 based on opacity (ensure the closer clouds appear more opaque than the faded ones)
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * CLOUD_OPACITY) - 100;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Move the cloud to the right by CLOUD_SPEED pixels per frame (as set by the * Time.deltaTime)
        transform.position = new Vector2(transform.position.x + CLOUD_SPEED * Time.deltaTime, transform.position.y);

        // Check that the cloud is offscreen to delete it
        if (transform.position.x - (GetComponent<BoxCollider2D>().size.x / 2) * transform.localScale.x >= 960)
        {
            // Destroy the cloud when offscreen
            Destroy(gameObject);
        }
    }
}
