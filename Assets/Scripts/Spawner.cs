using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;


    void Start()
    {
        Vector2 originalVector = new Vector2(4.3f, 1.3f);
        Vector2 newVector = Vector2Int.RoundToInt(originalVector);
    }

    Shape GetRandomShape()
    {
        int i = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[i])
        {
            return m_allShapes[i];
        }
        else
        {
            Debug.Log("Warning! Invalid shape in spawner!");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
         shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        //shape = GetQueuedShape();
        shape.transform.position = transform.position;
        shape.transform.localScale = Vector3.one;

        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.Log("Warning! Invalid shape in spawner!");
            return null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
