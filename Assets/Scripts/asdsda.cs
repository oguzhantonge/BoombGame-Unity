using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdsda : MonoBehaviour
{
    void Start()
    {
        int i = 0;
        while (i < 10)
        {
            Debug.Log(i);
            i++;
            if (i == 4)
            {
                break;
            }
        }
    }
}
