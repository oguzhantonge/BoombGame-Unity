using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TouchController : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler DragEvent;
    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler TapEvent;


    Vector2 touchMovement;

    [Range(50, 150)]
    public int minDragDistance = 100;

    [Range(50,250)]
    public int minSwipeDistance = 200;

    float tapTimeMax = 0;
    public float tapTimeWindow = 0.1f;

    void OnTap()
    {
        if (TapEvent != null)
        {
            TapEvent(touchMovement);
        }
    }
    void OnDrag()
    {
        if (DragEvent != null)
        {
            DragEvent(touchMovement);
        }
    }

    void OnDragEnd()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(touchMovement);
        }
    }

   // public Text diagnosticText1;
   // public Text diagnosticText2;

    public bool useDiagnostic = false;


    void Start()
    {
     //   Diagnostic("","");   
    }
    /*
    void Diagnostic(string text1, string text2)
    {
        diagnosticText1.gameObject.SetActive(useDiagnostic);
        diagnosticText2.gameObject.SetActive(useDiagnostic);

        if (diagnosticText1 && diagnosticText2)
        {
            diagnosticText1.text = text1;
            diagnosticText2.text = text2;
        }

    }
*/
    string SwipeDiagnostic(Vector2 swipeMovement)
    {

        string direction = "";

        // horizontal
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        }
        // vertical
        else
        {
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        }

        return direction;

    }
   
    void Update()
    {
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                touchMovement = Vector2.zero;
                tapTimeMax = Time.time + tapTimeWindow;
                //Diagnostic("","");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchMovement += touch.deltaPosition;

                if (touchMovement.magnitude > minDragDistance)
                {
                    OnDrag();
                 //  Diagnostic("Drag detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (touchMovement.magnitude > minSwipeDistance)
                {
                    OnDragEnd();
                  //  Diagnostic("Swipe detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));
                }
                else if (Time.time < tapTimeMax)
                {
                    OnTap();
                   // Diagnostic("Tap detected", touchMovement.ToString() + " " + SwipeDiagnostic(touchMovement));

                }

            }
        }


    }






}
