using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class print_position : MonoBehaviour
{
    string myFilePath_x, myFilePath_y, myFilePath_z;
    public string File_Name = "End_Effector_Position";
    // Start is called before the first frame update
    void Start()
    {
        myFilePath_x = Application.dataPath + "/" + File_Name + "_x.txt";
        myFilePath_y = Application.dataPath + "/" + File_Name + "_y.txt";
        myFilePath_z = Application.dataPath + "/" + File_Name + "_z.txt";
    }

    // Update is called once per frame
    void Update()
    {
        File.AppendAllText(myFilePath_x, transform.position.x.ToString() + "\r\n");
        File.AppendAllText(myFilePath_y, transform.position.y.ToString() + "\r\n");
        File.AppendAllText(myFilePath_z, transform.position.z.ToString() + "\r\n");
    }
}
