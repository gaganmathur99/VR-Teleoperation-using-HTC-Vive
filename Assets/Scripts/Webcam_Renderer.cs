using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMsg = RosMessageTypes.Sensor.ImageMsg;


public class Webcam_Renderer : MonoBehaviour
{
    public GameObject cube;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<RosMsg>("webcam", VideoRender);

    }

    void VideoRender(RosMsg data)
    {
       Texture2D image = new Texture2D((int)data.width, (int)data.height, TextureFormat.RGB24, false);
        byte[] pvrtcBytes = data.data;
        //Debug.Log(data);
        image.LoadRawTextureData(pvrtcBytes);
        image.Apply();
        // Assign texture to renderer's material.
        GetComponent<Renderer>().material.mainTexture = image;
        // GetComponent<Renderer>().material.mainTexture = data;*/
    }
}
