using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    public float forwardSpeed = 5;
    public float horizontalSpeed = 5;
    public float horizontalJumpSpeed = 1f;
    public float forwardJumpSpeed = 1f;

    public float sprintMultiplier = 2f;
    public float jumpForce = 1f;
    public float drag = 0.05f;
    public float airResistance;
    public float gravity = 0f;
    public float rbDrag = 0.01f;
    
    InterfaceController ic;

    public bool alive;
    public bool finished;
    public bool easyMode;

    float vSpeed = 0;
    float hSpeed = 0;
    Vector3 velocity;

    bool topDownView;
    bool jumping;

    Camera firstPersonCamera;        
    Rigidbody rb;

    PlatformMove lastPlatform;

    int platformMask;

    private void Awake()
    {
        firstPersonCamera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        ic = GameObject.FindGameObjectWithTag("Controllers").GetComponent<InterfaceController>();
        platformMask = LayerMask.GetMask("Platform");
    }

    private void Start()
    {
        jumping = false;
        topDownView = false;
        alive = true;
        finished = false;
        ic.HideMenu();
    }

    void FixedUpdate()
    {
        if (!topDownView && !ic.ShowingUI())
        {
            // Store the input axes.
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            bool sprinting = false;

            // Move faster
            if (Input.GetKey("left shift"))
            {
                sprinting = true;
            }

            if (!jumping)
            {
                // Jump
                if (Input.GetKey("space"))
                {
                    Jump();
                }
            }

            if (OnPlatform())
            {    
                vSpeed = forwardSpeed;
                hSpeed = horizontalSpeed;
            }
            else
            {
                vSpeed = forwardJumpSpeed;
                hSpeed = horizontalJumpSpeed;
            }

            // Forwards/backwards/sideways
            Move(v, h, sprinting);

            Drag();

            firstPersonCamera.transform.position = transform.position;

            AboveUnsafe();
        }        
    }

    void Move(float v, float h, bool sprinting)
    {
        Vector3 movement;

        // Vertical movement
        Vector3 movementV = new Vector3();
        movementV.Set(transform.forward.x, 0f, transform.forward.z);
        movementV.Normalize();

        // Horizontal movement
        Vector3 movementH = new Vector3();
        movementH.Set(transform.right.x, 0f, transform.right.z);
        movementH.Normalize();

        if (h == 0)
        {
            // forwards
            if (v > 0)
            {
                movement = movementV.normalized * vSpeed * Time.deltaTime * v;
            }
            // backwards
            else
            {
                movement = movementV.normalized * hSpeed * Time.deltaTime * v;
            }
            
        }
        // strafe
        else if (v == 0)
        {
            movement = movementH.normalized * hSpeed * Time.deltaTime * h;
        }
        // forward and strafe
        else if (v > 0)
        {
            movementH *= h;
            movementV *= v;

            movement = movementH + movementV;
            movement = movement.normalized * vSpeed * Time.deltaTime;
        }
        // backward and strafe
        else
        {
            movementH *= h;
            movementV *= v;

            movement = movementH + movementV;
            movement = movement.normalized * hSpeed * Time.deltaTime;
        }

        if (sprinting && v > 0)
        {
            movement *= sprintMultiplier;
        }

        rb.AddForce(movement);

        //Vector3 vec3 = rb.velocity;
        //vec3.y = 0;
        //Debug.Log(vec3.magnitude);
    }

    private void Drag()
    {
        Vector3 hVelocity = rb.velocity;
        hVelocity.y = 0;

        if (hVelocity.magnitude > 0.1f)
        {
            if (!jumping)
            {
                rb.AddForce(hVelocity * -drag);
            }
            else
            {
                rb.AddForce(hVelocity * -airResistance);
            }
        }

        // Extra gravity
        if (jumping)
        {
            Vector3 down = new Vector3(0, -1, 0);
            rb.AddForce(down * gravity);
        }
    }

    void Jump()
    {
        if (OnPlatform())
        {
            Vector3 up = new Vector3(0, 1, 0);
            up *= jumpForce * Time.deltaTime;
            rb.AddForce(up);
            jumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumping = false;

        if (collision.gameObject.tag == "Platform")
        {
            if (collision.gameObject.GetComponent<PlatformMove>().safe == false)
            {
                alive = false;
            }
            else if (collision.gameObject.GetComponent<PlatformMove>().finish == false && OnPlatform())
            {
                lastPlatform = collision.gameObject.GetComponent<PlatformMove>();
            }
        }
        else if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            alive = false;
        }
        else if (collision.gameObject.tag == "Finish")
        {
            finished = true;
        }
        else if (collision.gameObject.tag == "InvisibleWall")
        {
            GameObject.FindGameObjectWithTag("Controllers").GetComponent<LifeController>().RestartLevel();
        }
    }
    

    public void StartControl()
    {
        topDownView = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.velocity = velocity;
    }

    public void StopControl()
    {
        velocity = rb.velocity;
        topDownView = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;        
    }

    public void Reset(Vector3 position, Vector3 forward, bool forceReset)
    {
        if (easyMode && !forceReset)
        {
            position = lastPlatform.GetCentre();
            forward = transform.forward;
        }
       
        transform.position = position;
        transform.forward = forward;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);

        alive = true;
    }

    public void AboveUnsafe()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 10f, platformMask))
        {
            if (hit.collider.gameObject.tag == "Platform")
            {
                if (hit.collider.gameObject.GetComponent<PlatformMove>().safe == false)
                {
                    alive = false;
                }
            }
        }
    }

    public bool OnPlatform()
    {     
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 2.1f, platformMask))
        {            
            return true;
        }        

        return false;
    }

    public bool OnPlatformJump()
    {
        RaycastHit hit;
        Vector3 position;

        // Centre
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2.1f, platformMask))
        {
            return true;
        }

        // Behind
        position = transform.position;
        position -= transform.forward * 0.25f;
        if (Physics.Raycast(position, -transform.up, out hit, 2.1f, platformMask))
        {
            return true;
        }

        // Right
        position = transform.position;
        position += transform.right * 0.25f;
        if (Physics.Raycast(position, -transform.up, out hit, 2.1f, platformMask))
        {
            return true;
        }

        // Left
        position = transform.position;
        position -= transform.right * 0.25f;
        if (Physics.Raycast(position, -transform.up, out hit, 2.1f, platformMask))
        {
            return true;
        }


        return false;
    }
}