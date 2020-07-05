using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public Rigidbody rb;

    Animator anim;
    private Vector3 movement;
    int floorMask;                                                      //layer mask that tells where the cam should raycast onto
    float camRayLength = 100f;                                           //length of the casting ray

    private void Awake()                                                //Awake() function is called regardless of whether the script is enabled or not
    {
        floorMask = LayerMask.GetMask("Floor");                         //If you rem, we added the layer named "Floor" to the floor quad
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");                       //Normal axes have a value in range of -1 to 1 but Raw axes have only -1,0 or 1
        float v = Input.GetAxis("Vertical");                         //This means there is no "acceleration". the character starts with full speed when you press the button

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move (float h, float v)
    {
        movement.Set(h, 0f, v);                                         //Y-axis is vertical. 
        //Therefore, 0. Also, h and v (vectors) have a length of 1. But if both are used at once, the length comes to 1.4 (NORMALIZATION)
        movement = movement.normalized * speed * Time.deltaTime;        //We want to played to move at the speed we assigned at the timerate/frame
        rb.MovePosition(transform.position + movement);
    }

    void Turning()                                                      //For turning with respect to where the camera is looking (raycast) (mouse)
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); //ScreenpointToRay casts a ray to the screen from the point on the camera(here the position of the mouse)
        RaycastHit floorHit;                                            //gives info back from the ray cast

        if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))  //checking if the raycast hit something
        {
            Vector3 playerToMouse = floorHit.point - transform.position;    //creates a line from the player to the point the ray cast hit 
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            rb.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;                            //checks if we pressed either horizontal or vertical axes (to play moving animation)
        anim.SetBool("IsWalking", walking);
    }
}
