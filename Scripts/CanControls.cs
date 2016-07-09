using UnityEngine;
using System.Collections;

public class CanControls : MonoBehaviour {

    Vector3 touchPos;
    Vector3 canPos;
    bool stationarytouch;
    int lane;
    bool speedy;
    bool swiping;
    float speed;
    bool upHigh;
    float jumpTimer;
    bool jumping;
    //public variables, to be defined in each level
    public float maxSpeed;
    public float minSpeed;
    public float levelSpeed;
    public float airTime;
    public int maxLanes;
    public bool hasCollided;
    

    enum MovementState
    {
        NONE,
        FORWARD,
        LEFT,
        RIGHT,
        JUMP,
        STOPPED,
        CRUSHED
    };
    MovementState movementState;

	void Start () 
    {
        stationarytouch = false;
        movementState = MovementState.FORWARD;
        speedy = false;
        swiping = false;
        hasCollided = false;
        upHigh = false;
        jumpTimer = 0;
        jumping = false;
        //in the middle lanes to begin with
        if(maxLanes % 2 == 0) //if even number of lanes in left most middle lane
        {
            lane = maxLanes / 2;
        }
        if(maxLanes % 2 != 0) //if odd number of lanes in the middle lane
        {
            lane = maxLanes / 2 + 1;
        }
	}

    bool CheckPress()
    {
        if(Input.anyKey)
        {
            return true;
        }
        else 
        {
            return false;
        }       
    }

    bool CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	
    void Direction()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //move left
            movementState = MovementState.LEFT;
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //move right
            movementState = MovementState.RIGHT;
        }
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //jump
            movementState = MovementState.JUMP;
        }
    }

    void TouchDirection()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                touchPos = t.position;
            }
            if (t.phase == TouchPhase.Moved)
            {
                stationarytouch = false;
                if (!swiping)
                {
                    if (t.position.x > touchPos.x) //going left
                    {
                        movementState = MovementState.LEFT;
                        swiping = true;
                    }
                    if (t.position.x < touchPos.x) //going right
                    {
                        movementState = MovementState.RIGHT;
                        swiping = true;
                    }
                    //if distance is positive or negative going either left or right - entering these in twice
                }
            }
            if (t.phase == TouchPhase.Stationary)
            {
                //set a bool to check if jumping to true
                stationarytouch = true;
                swiping = false;
            }
            if (t.phase == TouchPhase.Ended)
            {
                swiping = false;
                //if that bool to check if jumping is true (so was stationary before) then can jump
                if (stationarytouch)
                {
                    movementState = MovementState.JUMP;
                    //then set that bool to false again
                    stationarytouch = false;
                }
                else
                {
                    movementState = MovementState.FORWARD;
                }
            }
            if (t.phase == TouchPhase.Canceled)
            {
                stationarytouch = false;
                swiping = false;
            }
        }
    }

    void Movement()
    {
        canPos =  gameObject.GetComponent<Rigidbody>().transform.position;
        speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        switch(movementState)
        {
            case MovementState.NONE:
                movementState = MovementState.FORWARD;
                break;
            case MovementState.FORWARD:
                if(jumpTimer >= airTime)
                {
                    gameObject.GetComponent<Rigidbody>().transform.position = new Vector3(canPos.x, (canPos.y - 10), canPos.z);
                    upHigh = false;
                    jumpTimer = 0;
                    jumping = false;
                }
                if(speed > maxSpeed)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity * 0.9f;
                    speedy = true;
                }
                if(speed < minSpeed)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
                    speedy = false;
                }
                if(!speedy)
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(levelSpeed, 0, 0));
                }
                break;
            case MovementState.LEFT:
                if(lane > 1)
                {
                    //can move left
                    gameObject.GetComponent<Rigidbody>().transform.position = new Vector3(canPos.x, canPos.y, (canPos.z - 15));
                    lane -= 1;
                }
                movementState = MovementState.FORWARD;
                break;
            case MovementState.RIGHT:
                if(lane < maxLanes)
                {
                    //can move right
                    gameObject.GetComponent<Rigidbody>().transform.position = new Vector3(canPos.x, canPos.y, (canPos.z + 15));
                    lane += 1;
                }
                movementState = MovementState.FORWARD;
                break;
            case MovementState.JUMP:
                if(!jumping)
                {
                    gameObject.GetComponent<Rigidbody>().transform.position = new Vector3(canPos.x, (canPos.y + 10), canPos.z);
                    upHigh = true;
                    jumping = true;
                }
                //gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5000, 0), ForceMode.Acceleration);
                movementState = MovementState.FORWARD;
                break;
            case MovementState.STOPPED: //end game, can start the level over for both of these, separated for animations
                gameObject.GetComponent<Rigidbody>().Sleep(); //stop the movement
                break;
            case MovementState.CRUSHED:
                break;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.GetComponent<Collider>().tag == "Blocker") //has hit an object that can block its way
        {
            //bring up fail screen
            hasCollided = true;
        }
    }
    //on trigger enter -when can collides with an object bring up fail screen, when can collides with end level trigger bring up winner screen(play the animation?)
    //set a bool 

	void Update () 
    {
	    if(CheckPress())
        {
            Direction();
        }
        if(CheckTouch())
        {
            TouchDirection();
        }
        Movement();
        if(upHigh)
        {
            jumpTimer += Time.deltaTime;
        }
	}
}
