using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RotationScript : MonoBehaviour {

	///// Variables For Bike Rotation on Touch  ////////////////////
 	float f_lastX = 0.0f;
    float f_difX = 0.5f;
    float f_steps = 0.0f;
    int i_direction = 1;
    int steps = 0;

	///////////////// For Bike Rotation ////////////////////////////
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			ShowQuitPanel();
		if (Input.GetMouseButtonDown(0))
        {
            f_difX = 0.0f;
        }
        else if (Input.GetMouseButton(0))
        {
            f_difX = Mathf.Abs(f_lastX - Input.GetAxis ("Mouse X"));
             
            if (f_lastX < Input.GetAxis ("Mouse X"))
            {
                i_direction = -1;
                transform.Rotate(Vector3.up, -f_difX);
            }
             
            if (f_lastX > Input.GetAxis ("Mouse X"))
            {
                i_direction = 1;
                transform.Rotate(Vector3.up, f_difX);
            }
            steps = 0;
            f_lastX = -Input.GetAxis ("Mouse X");
        }
        else
        {
            if (f_difX > 0.5f) f_difX -= 0.001f * Time.deltaTime;
            if (f_difX < 0.5f) f_difX += 0.001f * Time.deltaTime;
 
            if(steps >= 1)
                f_difX = 0.0f;
 
            transform.Rotate(Vector3.up, 1 * i_direction);
            steps++;
        }
	}
}
