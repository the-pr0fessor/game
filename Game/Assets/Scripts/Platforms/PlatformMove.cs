using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public bool locked;
    public bool safe;
    public bool finish;
    public bool mustTouchPlayer;
    //public float speed = 1f;
    public float collisionDistance = 0.5f;
    public float horizontalDistance = 0.05f;

    public Material defaultMaterial;
    public Material selectedMaterial;

    string axis;
    GameObject player;
    bool touchingPlayer;

    Vector3 offset;

    Rigidbody rb;


    Vector3 startPosition;
        
    float width;
    float length;
    float height;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        startPosition = transform.position;
        // Get if platform moving up/down or left/right based on the forward vector
        if (transform.forward.x > 0.5f)
        {
            axis = "h";
        }
        else
        {
            axis = "v";
        }

        touchingPlayer = false;

        //GetComponent<Renderer>().materials.SetValue(defaultMaterial, GetComponent<Renderer>().materials.Length - 1);

        width = transform.localScale.x;
        height = transform.localScale.y;
        length = transform.localScale.z;
    }

    // Stop unwanted movement
    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
    }

    // Move the platform along horizontal or vertical axis
    public void Move(string movementAxis, float direction, float speed)
    {
        bool canMove = true;

        if (locked)
        {
            canMove = false;
        }
        if (mustTouchPlayer && !touchingPlayer)
        {
            canMove = false;
        }

        // If the platform isn't locked, there is movement, and the direction matches the platform's direction
        if (canMove && direction != 0 && movementAxis == axis)
        {
            int newDirection;
            if (direction > 0)
            {
                newDirection = 1;
            }
            else
            {
                newDirection = -1;
            }

            rb.velocity = Vector3.zero;


            float hitDistance = GetDistanceToObject();

            Vector3 movement = transform.forward;
            movement = movement.normalized * speed * direction;

            Vector3 oldPos = transform.position;
            transform.position = transform.position + movement;
            
            // If moved inside an object, move so next to object
            if (GetIfObjectClose())
            {
                hitDistance -= length / 2;                
                transform.position = oldPos + (hitDistance * newDirection * transform.forward);      
            }
                
            // Move player with platform
            if (touchingPlayer)
            {
                player.transform.position = transform.position + offset;
            }

        }        
    }

    // When platform clicked on
    public void Select()
    {
        if (touchingPlayer)
        {
            offset = player.transform.position - transform.position;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        Material[] mats = GetComponent<Renderer>().materials;
        mats[mats.Length - 1] = selectedMaterial;
        GetComponent<Renderer>().materials = mats;

    }

    // When something else clicked on
    public void DeSelect()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

        Material[] mats = GetComponent<Renderer>().materials;
        mats[mats.Length - 1] = defaultMaterial;
        GetComponent<Renderer>().materials = mats;
    }

    // Get if player touching platform
    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject == player)
        {
            touchingPlayer = true;         
        }
    }

    void OnCollisionExit(Collision collision)
    {        
        if (collision.gameObject == player)
        {
            touchingPlayer = false;            
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    // True if object close
    bool GetIfObjectClose()
    {
        // Get the centre of the platform
        Vector3 centreUp = transform.position;
        centreUp -= transform.forward * (length / 2);
        centreUp += transform.right * horizontalDistance;
        centreUp += transform.up * (height / 2);

        Vector3 centreDown = transform.position;
        centreDown -= transform.forward * (length / 2);
        centreDown += transform.right * width;
        centreDown -= transform.right * horizontalDistance;
        centreDown += transform.up * (height / 2);

        RaycastHit hit;
        
        // Cast ray in forward direction, if hits something then an object is close in front
        if (Physics.Raycast(centreUp, transform.forward, out hit, (length / 2) + collisionDistance) || Physics.Raycast(centreDown, transform.forward, out hit, (length / 2) + collisionDistance))
        {
            return true;
        }
        // Cast ray in backward direction, if hits something then an object is close behind
        else if (Physics.Raycast(centreUp, -transform.forward, out hit, (length / 2) + collisionDistance) || Physics.Raycast(centreDown, -transform.forward, out hit, (length / 2) + collisionDistance))
        {
            return true;
        }
        // If neither ray hits, nothing close
        else
        {
            return false;
        }
    }

    float GetDistanceToObject()
    {
        Vector3 centreUp = transform.position;
        centreUp -= transform.forward * (length / 2);
        centreUp += transform.up * (height / 2);
        centreUp += transform.right * horizontalDistance;

        Vector3 centreDown = transform.position;
        centreDown -= transform.forward * (length / 2);
        centreDown += transform.right * width;
        centreDown -= transform.right * horizontalDistance;
        centreDown += transform.up * (height / 2);

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        
        Physics.Raycast(centreUp, transform.forward, out hit1, 300);
        Physics.Raycast(centreDown, transform.forward, out hit2, 300);
        Physics.Raycast(centreUp, -transform.forward, out hit3, 300);  
        Physics.Raycast(centreDown, -transform.forward, out hit4, 300);

        return Mathf.Min(hit1.distance, hit2.distance, hit3.distance, hit4.distance);
    }

    public Vector3 GetCentre()
    {
        Vector3 centre = transform.position;
        centre -= transform.forward * (length / 2);
        centre += transform.up * height;
        centre.y += 2.0f;
        centre += transform.right * (width / 2);
        return centre;
    }
}
