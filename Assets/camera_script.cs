using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camera_script : MonoBehaviour
{
    WebCamTexture tex;
    public RawImage display;
    public int CamIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        Start_Cam();
    }

    // Update is called once per frame
    public void Start_Cam()
    {
        WebCamDevice device = WebCamTexture.devices[CamIndex];
        tex = new WebCamTexture(device.name);
        display.texture = tex;
        tex.Play();
    }
}
