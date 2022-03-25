using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isClosed = false;
    public bool isLocked = false;

    Vector3 closedRotation = new Vector3(0, 0, 0);
    Vector3 openRotation = new Vector3(0, -130, 0);

    void Start()
    {
        //Debug.Log("Door is Open");
        if (isClosed)
        {
            transform.eulerAngles = closedRotation;
        }
        else
        {
            transform.eulerAngles = openRotation;
        }
    }

    public bool open()
    {
        Debug.Log("Door is being Opened");
        if (isClosed && !isLocked)
        {
            isClosed = false;
            transform.eulerAngles = openRotation;
            return true;
        }
        return false;
    }

    public bool closed()
    {
        if (!isClosed)
        {
            transform.eulerAngles = closedRotation;
            isClosed = true;
        }
        return true;
    }
}
