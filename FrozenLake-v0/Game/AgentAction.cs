using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAction : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Act(3);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Act(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Act(0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Act(2);
    }

    public void Reset()
    {
        transform.position = new Vector3(-15, 2.5f, 15);
    }

    public void Act(string _A)
    {
        switch(_A)
        {
            case "left": Act(0); break;
            case "down": Act(1); break;
            case "right": Act(2); break;
            case "up": Act(3); break;
        }
    }

    public void Act(int _A)
    {
        //LEFT = 0
        //DOWN = 1
        //RIGHT = 2
        //UP = 3
        switch(_A)
        {
            case 0:
                transform.position = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
                break;
            case 3:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
                break;
        }
    }

}
