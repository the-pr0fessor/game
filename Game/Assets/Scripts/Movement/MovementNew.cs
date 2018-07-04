using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNew : MonoBehaviour {
    public float speed = 6f;

    public Camera mainCamera;

    Animator animator;

    bool moved;

    bool backwards;
    bool justSwappedBackwards;
    bool justSwappedForwards;

    public Vector3 virtualForward;

    public string direction;

    private void Start()
    {
        moved = false;
        backwards = false;
        justSwappedBackwards = false;
        justSwappedForwards = false;
        direction = "f";
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        virtualForward = new Vector3();
        virtualForward = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool sprinting = Input.GetKey("left shift");   

        if ((v == 0) && (h == 0))
        {
            moved = false;
            sprinting = false;
        }
        else
        {
            moved = true;
        }


        backwards = v < 0;


        if (!backwards)
        {
            justSwappedBackwards = true;
            direction = "f";
        }
        else
        {
            justSwappedForwards = true;
            direction = "b";
        }

        // TODO: turning animation when just swapped

        if (backwards && justSwappedBackwards)
        {
            virtualForward = transform.forward;
            transform.forward = -1 * transform.forward;
            justSwappedBackwards = false;
        }
        else if (!backwards && justSwappedForwards)
        {
            transform.forward = -1 * transform.forward;
            virtualForward = transform.forward;
            justSwappedForwards = false;
        }
        else if (backwards)
        {
            //virtualForward = -1 * transform.forward;
            virtualForward = -1 * transform.forward;
        }
        else
        {
            virtualForward = transform.forward;
        }


        Animate(h, v, sprinting);
    }

    void MoveVertical(float v)
    {
        // Set the movement vector based on the axis input.
        Vector3 movement = new Vector3();
        movement.Set(transform.forward.x, 0f, transform.forward.z);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime * v;

        //transform.position = transform.position + movement;

    }

    void MoveHorizontal(float h)
    {     
        // Set the movement vector based on the axis input.
        Vector3 movement = new Vector3();
        movement.Set(transform.right.x, 0f, transform.right.z);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime * h;

        //transform.position = transform.position + movement;
    }

    public bool MovedThisFrame()
    {
        return moved;
    }

    void Animate(float h, float v, bool sprinting)
    {
        bool walking = v != 0;


        animator.SetBool("WalkingForward", walking);
        animator.SetBool("Sprinting", sprinting);

    }
}
