using System;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using rosmsg = RosMessageTypes.ProjectMsg.FrankaJointsMsg;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.UrdfImporter;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;


public class Joint_State_Subscriber : MonoBehaviour
{
    [SerializeField]
    ArticulationBody[] m_JointArticulationBodies;

    [SerializeField]
    GameObject m_Franka;
    public GameObject Franka { get => m_Franka; set => m_Franka = value; }
    //Hardcoded Variables
    const int k_num_joints = 7;
    public static readonly string[] LinkNames =
       { "Base_Link/Link_1", "/Link_2", "/Link_3", "/Link_4", "/Link_5", "/Link_6", "/Link_7" };

    [SerializeField]
    string m_TopicName = "/franka_robot_joint_transfer";
    private List<float> joint_angle = new List<float>(7);
    // ROS Connector
    ROSConnection m_Ros;
    public string File_Name_ROS_Joint = "ROS_Joint_Test_1";
    public string File_Name_Unity_Joint = "Unity_Joint_Test_1";
    string myFilePath_ros, myFilePath_unity;
    public List<float> joint_offset = new List<float>(7);
    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.Subscribe<rosmsg>(m_TopicName, UpdatePose);
        m_JointArticulationBodies = new ArticulationBody[k_num_joints];
        joint_offset[6] = -45;
        var linkName = string.Empty;
        for (var i = 0; i < k_num_joints; i++)
        {
            linkName += LinkNames[i];
            m_JointArticulationBodies[i] = m_Franka.transform.Find(linkName).GetComponent<ArticulationBody>();
        }
        myFilePath_ros = Application.dataPath + "/" + File_Name_ROS_Joint + ".txt";
        myFilePath_unity = Application.dataPath + "/" + File_Name_Unity_Joint + ".txt";

    }

    // Update is called once per frame
    void UpdatePose(rosmsg msg)
    {
        Debug.Log("here comes the msg: " + msg);
        File.AppendAllText(myFilePath_ros, msg.ToString());
        for (int i = 0; i < 7; i++)
        {
            RotateTo(Mathf.Rad2Deg*(float)msg.joints[i] + joint_offset[i], m_JointArticulationBodies[i]);
            File.AppendAllText(myFilePath_unity, m_JointArticulationBodies[i].jointPosition[0].ToString() + "; ");
            
        }
        File.AppendAllText(myFilePath_unity,"\r\n ");

    }
    void RotateTo(float primaryAxisRotation, ArticulationBody Body_Link)
    {
        var drive = Body_Link.xDrive;
        drive.target = primaryAxisRotation;
        Body_Link.xDrive = drive;
        //Task.Delay(delay);
    }
}
