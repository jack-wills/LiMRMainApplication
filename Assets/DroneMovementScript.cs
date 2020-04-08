using System.Collections;
using UnityEngine;


public class DroneMovementScript : MonoBehaviour
{
    Rigidbody ourDrone;
    

    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();

    }

   

    void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        Rotation();
        ClampingSpeedValues();
        Swerve();


        ourDrone.AddRelativeForce(Vector3.up * upForce);
        ourDrone.rotation = Quaternion.Euler(new Vector3(titltAmountForward, currentYRotation, ourDrone.rotation.z));
    }

    public float upForce;

    void MovementUpDown()
    {
        if (Input.GetKey(KeyCode.U))
        {
            upForce = 450;
        }
        else if (Input.GetKey(KeyCode.O))
        {
            upForce = -200;
        }
        else if (!Input.GetKey(KeyCode.U) && !Input.GetKey(KeyCode.O))
        {
            upForce = 98.1f;
        }
    }

    private float movementForwardSpeed = 100.0f;
    private float titltAmountForward = 0;
    private float tiltVelocityFoward ;


    void MovementForward()
    {
        if (Input.GetAxis("Vertical")!=0)
        {
            ourDrone.AddRelativeForce(Vector3.forward *Input.GetAxis("Vertical")* movementForwardSpeed);
            titltAmountForward = Mathf.SmoothDamp(titltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityFoward, 0.1f);
        }

       // if (Input.GetKey(KeyCode.K))
       // {
        //    ourDrone.AddRelativeForce(Vector3.forward * -movementForwardSpeed);
        //    titltAmountForward = Mathf.SmoothDamp(-titltAmountForward, -20, ref tiltVelocityFoward, -0.01f);
       // }
    }

    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationYVelocity;

    void Rotation()
    {
        if (Input.GetKey(KeyCode.N))
        {
            wantedYRotation -= rotateAmountByKeys;
        }

        if (Input.GetKey(KeyCode.M))
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }

    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }

    private float swerveMovementAmount = 300.0f;
    private float swerveTiltAmount;
    private float swerveVelocity;

    void Swerve()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
       {
            ourDrone.AddRelativeForce(Vector3.right* Input.GetAxis("Horizontal") * swerveMovementAmount);
           swerveTiltAmount = Mathf.SmoothDamp(swerveTiltAmount, 20 * Input.GetAxis("Horizontal"), ref swerveVelocity, 0.01f);
        }
       else
       {
           swerveTiltAmount = Mathf.SmoothDamp(swerveTiltAmount, 0, ref swerveVelocity, 0.01f);
       }
    }

}

 //
