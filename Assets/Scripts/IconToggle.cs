using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    public Sprite iconTrue;
    public Sprite iconFalse;

    public bool defaultIconBool = true;

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = (defaultIconBool) ? iconTrue : iconFalse;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ChangeIcon(bool state)
    {
        if ( !iconTrue || !iconFalse || !image  )
        {
            Debug.LogWarning("WARNING! ICONTOGGLE missing iconTrue or iconFalse!");
            return;
        }
        image.sprite = (state) ? iconTrue : iconFalse;

    }
}
