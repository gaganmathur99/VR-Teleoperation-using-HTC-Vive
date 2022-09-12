using System;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.ProjectMsg;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.UrdfImporter;
using UnityEngine;
using System.IO;

public class pose_publisher : MonoBehaviour
{
    [SerializeField]
    string m_TopicName = "/target_pose";

    [SerializeField]
    GameObject m_Target;

    // ROS Connector
    ROSConnection m_Ros;
    string myFilePath_x, myFilePath_y, myFilePath_z;
    public string File_Name = "target_location";
    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseStampedMsg>(m_TopicName);
      
        myFilePath_x = Application.dataPath + "/" + File_Name + "_x.txt";
        myFilePath_y = Application.dataPath + "/" + File_Name + "_y.txt";
        myFilePath_z = Application.dataPath + "/" + File_Name + "_z.txt";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Pick Pose
        var pick_pose = new PoseStampedMsg();
        pick_pose.pose.position = m_Target.transform.position.To<FLU>();
        pick_pose.pose.orientation = Quaternion.Euler(m_Target.transform.eulerAngles.x, m_Target.transform.eulerAngles.y, m_Target.transform.eulerAngles.z).To<FLU>();
        //pick_pose.pose.orientation = Quaternion.Euler(90, m_Target.transform.eulerAngles.y, 0).To<FLU>();
        //File.WriteAllText(myFilePath, pick_pose.pose.ToString());
        File.AppendAllText(myFilePath_x, pick_pose.pose.position.x.ToString() + "\r\n");
        File.AppendAllText(myFilePath_y, pick_pose.pose.position.y.ToString() + "\r\n");
        File.AppendAllText(myFilePath_z, pick_pose.pose.position.z.ToString() + "\r\n");

        Debug.Log("Position FLU: " + m_Target.transform.position.To<FLU>());
        Debug.Log("Position: " + m_Target.transform.position);
        Debug.Log("orientation FLU: " + Quaternion.Euler(m_Target.transform.eulerAngles.x, m_Target.transform.eulerAngles.y, m_Target.transform.eulerAngles.z).To<FLU>());
        Debug.Log("orientation : " + Quaternion.Euler(m_Target.transform.eulerAngles.x, m_Target.transform.eulerAngles.y, m_Target.transform.eulerAngles.z));

        // Finally send the message to server_endpoint.py running in ROS
        m_Ros.Publish(m_TopicName, pick_pose);
    }
}
