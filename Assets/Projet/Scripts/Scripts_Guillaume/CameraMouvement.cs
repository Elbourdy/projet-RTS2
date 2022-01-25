using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    private Vector2 dir;
    private Vector3 mousePositionOff;

    public float speed;
    public float offset;
    
    public bool activateMovement = true;


    private void Update()
    {
        if (activateMovement)
        {
            CheckMousePosition();

            if (dir != Vector2.zero)
            {
                MoveCam();
            }
        }
    }

    private void MoveCam()
    {
        var cameraDir = Vector3.forward * dir.y + Vector3.right * dir.x;
        transform.position += cameraDir * speed * Time.deltaTime;
    }


    private void CheckMousePosition()
    {
        dir = Vector2.zero;
        if (isLeft())
        {
            dir += new Vector2(0, 1);
        }
        if (isRight())
        {
            dir += new Vector2(0, -1);
        }
        if (isTop())
        {
            dir += new Vector2(1, 0);
        }
        if (isBottom())
        {
            dir += new Vector2(-1, -0);
        }

        //Debug.Log(dir);
    }

    private bool isLeft()
    {
        mousePositionOff = new Vector3(Input.mousePosition.x - offset, Input.mousePosition.y, 0);
        if (mousePositionOff.x < 0)
        {
            return true;
        }
        return false;
    }

    private bool isRight()
    {
        mousePositionOff = new Vector3(Input.mousePosition.x + offset, Input.mousePosition.y, 0);
        if (mousePositionOff.x > Screen.width)
        {
            return true;
        }
        return false;
    }

    private bool isTop()
    {
        mousePositionOff = new Vector3(Input.mousePosition.x, Input.mousePosition.y + offset, 0);
        if (mousePositionOff.y > Screen.height)
        {
            return true;
        }
        return false;
    }

    private bool isBottom()
    {
        mousePositionOff = new Vector3(Input.mousePosition.x, Input.mousePosition.y - offset, 0);
        if (mousePositionOff.y < 0)
        {
            return true;
        }

        return false;
    }

}
