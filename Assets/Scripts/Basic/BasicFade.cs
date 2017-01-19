using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BasicFade : MonoBehaviour
{
    // please attach this to a ui.panel object in the scene with image component that has no source image
     
    public Image objectFade;
    public bool fadeOut;
    public float smooth;

    void Start()
    {
        objectFade = GetComponent<Image>();
    }

    void Update()
    {
        if (fadeOut == true)    // if you want to fade out =
        {
            objectFade.color = Color.Lerp(objectFade.color, Color.clear, Time.deltaTime *smooth);
                                                                    
        }
        if (fadeOut == false)   // if you want to fade in =         
        {
            objectFade.color = Color.Lerp(objectFade.color, Color.black, Time.deltaTime * smooth);
        }
    }
}
