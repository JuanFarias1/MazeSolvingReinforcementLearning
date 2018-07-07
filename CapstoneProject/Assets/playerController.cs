using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : CubeController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        Vector3 currentPosition = transform.position;

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 back = transform.TransformDirection(Vector3.back);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (Physics.Raycast(transform.position, left, 2) == false)
        {


            if (Input.GetKeyDown(KeyCode.A))
            {
                movementSwitch = true;

                //Debug.Log("DANGER ZONE 1");

                transform.position += 3 * Vector3.left;

               // distToGoal.Add(distanceFunction());


               // counter += 1;

            }
        }



        if (Physics.Raycast(transform.position, right, 2) == false)
        {


            if (Input.GetKeyDown(KeyCode.D))
            {
                movementSwitch = true;

                // Debug.Log("DANGER ZONE 2");

                transform.position += 3 * Vector3.right;

               // distToGoal.Add(distanceFunction());
               
                //counter += 1;

            }
        }


        if (Physics.Raycast(transform.position, fwd, 2) == false)
        {


            if (Input.GetKeyDown(KeyCode.W))
            {
                movementSwitch = true;

                //Debug.Log("DANGER ZONE 3");

                transform.position += 3 * Vector3.forward;

               // distToGoal.Add(distanceFunction());

                //counter += 1;

            }
        }

        if (Physics.Raycast(transform.position, back, 2) == false)
        {


            if (Input.GetKeyDown(KeyCode.S))
            {
                movementSwitch = true;

                // Debug.Log("DANGER ZONE 4");

                transform.position += 3 * Vector3.back;

                //distToGoal.Add(distanceFunction());

                //counter += 1;


            }
        }

	}
}
