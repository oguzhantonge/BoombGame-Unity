using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool isRotate = true;

    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }
    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));

    }

    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));

    }
    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }



    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

   
    public void RotatingRight()
    {
        if (isRotate)
        {
            transform.Rotate(0, 0, -90);
        }

    }

    public void RotatingLeft()
    {
        if (isRotate)
        {
            transform.Rotate(0, 0, 90);
        }

    }

    public void RotateClockwise(bool clockwise)
    {
        if (clockwise)
        {
            RotatingRight();
        }
        else
        {
            RotatingLeft();
        }


    }

   
}
