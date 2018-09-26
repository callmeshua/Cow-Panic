/* NAME: CowSpawn.cs
 * AUTHOR: Drew Graham
 * PURPOSE: Spawns cows in the game at a somewhat random rate
 * VERSION NUMBER: 0.2.0
 * DATE MODIFIED: 3/7/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - created the cowSpawn() function
 *        - it generates random numbers to decide whether or not to spawn a cow
 *      - still need to make it actually spawn the cows

 *  ---- 0.2.0 ----
 *      - it spawns the cows!
 *      - still have to tweak the rate to make them spawn right
 *          - a steady, yet somewhat random rate
 *      - player should be comfortable abducting
 *      - eventually implement increasing rate
 
 */


// this script needs to make cows spawn at the left with the script Moove.cs attached
// to them to make them move to the right

using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]

public class CloudSpawn : MonoBehaviour
{
    public Transform cloud;


    // Use this for initialization
    void Start()
    {
        InvokeRepeating("cloudSpawn", 0, .25F);
        // waits 0 seconds, then calls function cowSpawn every .25 seconds
    }

    void cloudSpawn()
    {
        int x = Random.Range(0, 100); // random number from 0 to 100

        if (x < 50) // 50% chance to spawn cow
        {
            GameObject clone;
            clone = Instantiate(Resources.Load("cloud")) as GameObject;
            // spawns a cloud to the left of the screen with no rotation
            // and casts it (prefab) as a GameObject so it can be spawned in the scene
            // MAKE SURE THERE IS A cloud PREFAB IN THE RESOURCES FOLDER
            // ** MAKE DOUBLE SURE THAT THERE IS IN FACT A RESOURCES FOLDER ** //
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
