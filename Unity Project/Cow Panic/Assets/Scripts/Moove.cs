/* NAME: Moove.cs
 * AUTHOR: Jake Carfagno
 * PURPOSE: Implements all AI for each cow, though not quite enough that they can overthrow us and our moo-tiful puns
 * VERSION NUMBER: 1.3.0
 * DATE MODIFIED: 5/29/16
 * VERSION HISTORY:
 *  ---- 0.1.0 ----
 *      - Created the file
 *      - Added a To-Do section
 *      - Made it so cows are smart enough to moove to the right
 *  ---- 0.2.0 ----
 *      - Added a function stopMoving() (called by the tractor beam GameObject) to stop cows from moving when they touch the tractor beam
 *  ---- 0.3.0 ----
 *      - Added documentation because it seems like this class may actually be here to stay
 *      - Added a POINTS_VALUE variable to store the amount of points the default cows are worth (different versions of this script like ChocolateMoove can have different values)
 *      - Added a MOOVE_SPEED integer that can be set in the Inspector and otherwise defaults to a value in the Start() function
 *      - Added a tractorBeam object whose removeCow() function is called in the Moove destructor
 *  ---- 0.3.1 ----
 *      - Switched POINTS_VALUE and MOOVE_SPEED to public so they can be edited in the Unity Inspector
 *  ---- 0.3.2 ----
 *      - Added a boolean isAbducted that is checked in the destructor to differentiate between abduction and escape on deletion
 *      - Added a function abductCow() to be called by the UFO when it touches a cow, setting isAbducted to true
 *  ---- 1.0.0 ----
 *      - Removed the destructor
 *      - Added a function getPoints() that returns the POINTS_VALUE constant
 *      - Removed the isAbducted boolean and abductCow() because points are now handled by the UFO
 *  ---- 1.0.1 ----
 *      - Added a check for collisions with the UFO that calls the UFO's PointsManager's addPoints() function and destroys the cow
 *      - Added a check for collisions with the tractor beam that calls the tractor beam's addCow function() on detection
 *  ---- 1.0.2 ----
 *      - Removed collision checking code for both the tractor beam and UFO
 *      - Added a check for if the cow has reached the right edge of the screen
 *      - Added a fallDown() method that makes the cow return to its original y value and continue moving if the tractor beam is no longer touching it
 *      - Added a constant STARTING_Y that is the original y position to which the cow should fall back down if released from the tractor beam
 *  ---- 1.0.3 ----
 *      - Removed the offset check in the cows' screen boundary check because the cows do not have an offset on their sprites
 *      - Cows now despawn immediately after exiting screen thanks to the inclusion of transform.localScale to convert the original sprite's width to the scaled sprite's width for accurate coordinate measurements
 *  ---- 1.1.0 ----
 *      - Added a call to PlayerLives.subtractLife when the cow leaves the screen boundaries
 *      - Added a constant LIVES_COST to store the amount of lives deducted when the cow leaves the screen boundaries
 *	---- 1.2.0 ----
 *		- Added cow variants
 *	---- 1.2.1 ----
 *		- Mooved default lives-cost to each part of the switch statement so that cows can have no lives-cost
 *	---- 1.3.0 ----
 *	    - Created a function shrink() that causes the cow to shrink when it is being captured by the UFO (sort of a despawning animation)
*/

using UnityEngine;
using System.Collections;

public class Moove : MonoBehaviour
{
    [HideInInspector]
    // A boolean set to false when the cow is in the tractor beam and instantiated as true so the cow will otherwise moove
    private bool canMove;

    // The integer amount of points added to the score when the cow is abducted and destroyed
    public int POINTS_VALUE;
    // The speed at which the cows move in pixels per second
    public int MOOVE_SPEED;

    // The amount of lives lost when this cow leaves the screen without being abducted
    public int LIVES_COST;

    [HideInInspector]
    // The y position at which the cow spawns and to which the cow should fall down if released from the tractor beam
    private float STARTING_Y;

    [HideInInspector]
    // The change in y between STARTING_Y and where the cow is when the tractor beam disappears (used to calculate falling velocity)
    private float deltaY;

    [HideInInspector]
    private bool isFalling;

    [HideInInspector]
    // The GameObject of the tractor beam
    // Used to call the tractor beam's removeCow() function in the Moove destructor
    private GameObject tractorBeam;


    // Whether the cow's size should be decreasing in the Update() function
    private bool isShrinking;

    // This int is randomly assigned on start to choose the cow variant
    public int variant;

    private Animator animator;
    enum cowVariants { NORMAL, MINT, CHOCOLATE, CASH, COCONUT, DEVIL, GOLDEN, LEMON, SICKLY, SOY, STRAWBERRY, GLITCH };
    
    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        System.Random randGen = new System.Random();
        variant = randGen.Next() % 100 + 1;

        animator.SetInteger("Variant", variant);

        // Assigns the starting y position
        STARTING_Y = gameObject.transform.position.y;

        // Determines cow variant from random number 'variant'
        cowVariants myVariant = cowVariants.NORMAL; // had to assign default value or else Visual Studio gets angry
        int variantInt = 0;
        if (variant > 0 && variant <= 10)
        {
            myVariant = cowVariants.NORMAL;
            variantInt = 0;
        }
        else if (variant > 10 && variant <= 20)
        {
            myVariant = cowVariants.MINT;
            variantInt = 8;
        }
        else if (variant > 20 && variant <= 30)
        {
            myVariant = cowVariants.CHOCOLATE;
            variantInt = 2;
        }
        else if (variant > 30 && variant <= 40)
        {
            myVariant = cowVariants.CASH;
            variantInt = 1;
        }
        else if (variant > 40 && variant <= 50)
        {
            myVariant = cowVariants.COCONUT;
            variantInt = 3;
        }
        else if (variant > 50 && variant <= 60)
        {
            myVariant = cowVariants.DEVIL;
            variantInt = 4;
        }
        else if (variant > 60 && variant <= 70)
        {
            myVariant = cowVariants.GOLDEN;
            variantInt = 6;
        }
        else if (variant > 70 && variant <= 80)
        {
            myVariant = cowVariants.LEMON;
            variantInt = 7;
        }
        else if (variant > 80 && variant <= 90)
        {
            myVariant = cowVariants.SICKLY;
            variantInt = 9;
        }
        else if (variant > 90 && variant <= 100)
        {
            myVariant = cowVariants.SOY;
            variantInt = 10;
        }
        else if (variant > 100 && variant <= 110)
        {
            myVariant = cowVariants.STRAWBERRY;
            variantInt = 11;
        }
        else if (variant > 110 && variant <= 120)
        {
            myVariant = cowVariants.GLITCH;
            variantInt = 12;
        }

        MOOVE_SPEED = 300; // had to assign default values or else Visual Studio gets angry
        POINTS_VALUE = 100;

        // checks value of enum myVariant and assigns proper MOOVE_SPEED and POINTS_VALUE
        switch (myVariant)
        {
            case cowVariants.NORMAL: //normal cow
                MOOVE_SPEED = 300;
                POINTS_VALUE = 100;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.MINT: //mint cow
                MOOVE_SPEED = 350;
                POINTS_VALUE = 150;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.CHOCOLATE: //chocolate cow
                MOOVE_SPEED = 200;
                POINTS_VALUE = 125;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.CASH: //cash cow
                MOOVE_SPEED = 650;
                POINTS_VALUE = 400;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.COCONUT: //coconut cow
                MOOVE_SPEED = 175;
                POINTS_VALUE = 75;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.DEVIL: //devil cow
                MOOVE_SPEED = 400;
                POINTS_VALUE = 250;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.GOLDEN: //golden calf
                MOOVE_SPEED = 500;
                POINTS_VALUE = 300;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.LEMON: //lemon cow
                MOOVE_SPEED = 325;
                POINTS_VALUE = 125;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.SICKLY: //sickly cow
                MOOVE_SPEED = 100;
                POINTS_VALUE = 25;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.SOY: //soy cow
                MOOVE_SPEED = 325;
                POINTS_VALUE = 125;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.STRAWBERRY: //strawberry cow
                MOOVE_SPEED = 375;
                POINTS_VALUE = 175;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
            case cowVariants.GLITCH:
                MOOVE_SPEED = 700;
                POINTS_VALUE = 600;
                LIVES_COST = 1;
                animator.SetInteger("Variant", variantInt);
                break;
        }

        // Allows the cow to move by default
        canMove = true;

        // Set isFalling to false because the cow is not falling by default
        isFalling = false;

        // Default to false because the cow should not shrink to start
        isShrinking = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Only moves when not being abducted
        if(canMove)
            // Add to the cow's x position the MOOVE_SPEED * TIme.deltaTime
            // MOOVE_SPEED * Time.deltaTime ensures the cows move at a set rate per second, independent of frame rate
            transform.position = new Vector2(transform.position.x + MOOVE_SPEED * Time.deltaTime, transform.position.y);

        // Check if the cow has reached the right edge of the screen
        // 960 is the edge of the screen because the screen is 1920 large, and 960 is half that in the positive direction
        // GetComponent<BoxCollider2D>().size.x / 2 finds half the sprite's size, and subtracting it from x finds the left-most edge of the cow
        // Multiplying the half-sprite size by localScale also takes into account that the cows are smaller than the original sprite
        if(transform.position.x - (GetComponent<BoxCollider2D>().size.x / 2) * transform.localScale.x >= 960)
        {
            // Reduce the total lives count by this Cow's value
            GameObject.Find("UFO").GetComponent<PlayerLife>().subtractLife(LIVES_COST);

            // Destroy the cow
            Destroy(gameObject);
        }

        if(isFalling)
        {
            // Bring the cow's y position down
            // deltaY * Time.deltaTime takes into account time in seconds between each frame update, and * 2 speeds it up so it takes a quarter second for the cow to return to the ground
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (deltaY * Time.deltaTime * 4));
            
            // Check if the cow has returned to its original position
            if(gameObject.transform.position.y <= STARTING_Y)
            {
                // If the cow is back to STARTING_Y, let the cow start moving again and tell it to stop falling
                isFalling = false;
                canMove = true;
            }
        }

        // If isShrinking, subtract 0.01F from the localScale in both dimensions each update (using Time.deltaTime so it takes about half a second to disappear completely)
        if(isShrinking)
        {
            Debug.Log(transform.localScale);
            transform.localScale = new Vector2(transform.localScale.x - Time.deltaTime * 2, transform.localScale.y - Time.deltaTime * 2);
            if(transform.localScale.x <= 0 || transform.localScale.y <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // PURPOSE: Tells the cow to stop moving when called by the tractor beam that collides with it
    // PARAMETERS: Takes no parameters
    // RETURNS: Returns no values
    public void stopMoving()
    {
        // Set canMove to false so the cow can no longer move
        // This line ought to be pretty self-explanatory
        canMove = false;
    }

    // PURPOSE: Tells the cow to fall back to the ground and start moving again
    // PARAMETERS: Takes no parameters
    // RETURNS: Returns no values
    public void fallDown()
    {
        // Calculate the total distance the cow must fall
        deltaY = STARTING_Y - gameObject.transform.position.y;

        // Tell the cow to start falling
        isFalling = true;
    }

    // PURPOSE: Takes in a GameObject and assigns the GameObject variable tractorBeam to it for easy acces in the destructor
    // PARAMETERS: GameObject tractor: the tractor beam passes itself as this parameter when the function is called to assign the tractor beam GameObject to tractorBeam
    // RETURNS: Returns no values
    public void setTractorBeam(GameObject tractor)
    {
        tractorBeam = tractor;
    }

    // PURPOSE: Returns the points value of this cow; called by the UFO when destroying the cow to increase points
    // PARAMETERS: Takes no parameters
    // RETURNS: Returns the constant int POINTS_VALUE, the amount of points this cow is worth
    public int getPoints()
    {
        return POINTS_VALUE;
    }

    // PURPOSE: Begin shrinking the cow
    // PARAMETERS: Takes no parameters
    // RETURNS: Returns no values
    public void shrink()
    {
        isShrinking = true;
    }
}
