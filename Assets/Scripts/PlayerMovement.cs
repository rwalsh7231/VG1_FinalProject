using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    int sprintMult = 1;
    public Image stamBar;

    public float currStam, maxStam;
    public float sprintCost;
    public float stamRecharge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //sprinting capability
        if (Input.GetKeyDown(KeyCode.LeftShift) && currStam > 0)
        {
            sprintMult = 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || currStam < 0)
        {
            sprintMult = 1;
        }


        //go up
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime*sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        //go down
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(0, -0.5f, 0) * Time.deltaTime*sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        //go left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-0.5f, 0, 0) * Time.deltaTime*sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        //go right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(0.5f, 0, 0) * Time.deltaTime * sprintMult;
            if (sprintMult == 2)
            {
                currStam -= sprintCost * Time.deltaTime;
                stamBar.fillAmount = currStam / maxStam;
            }
        }

        if (sprintMult == 1) {
            currStam += stamRecharge * Time.deltaTime;
            stamBar.fillAmount = currStam / maxStam;
        }
    }

    
}
