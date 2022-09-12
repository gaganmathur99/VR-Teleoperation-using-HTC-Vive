using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.ProjectMsg;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.UrdfImporter;
using UnityEngine;


public class IKManager : MonoBehaviour
{
    public Joints m_root;

    public Joints m_end;

    public GameObject m_target;

    public float m_threshold = 0.05f;

    public float m_rate = 5.0f;

    public int m_steps = 20;

    //Hardcoded Variables
    const int k_num_joints = 7;
    public static readonly string[] LinkNames =
        { "Base_Link/Link_1", "/Link_2", "/Link_3", "/Link_4", "/Link_5", "/Link_6", "/Link_7" };

    [SerializeField]
    GameObject m_Franka;
    public GameObject Franka { get => m_Franka; set => m_Franka = value; }

    //Articulation Bodies
    ArticulationBody[] m_JointArticulationBodies;

    // Variables required for ROS communication
    ROSConnection ros;
    [SerializeField]
    public string topicName = "/franka_joints";

    // Publish the joint angle detail every N sec
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    float CalculateSlope(Joints _joint)
    {
        float deltaTheta = 0.005f;
        float distance1 = GetDistance(m_end.transform.position, m_target.transform.position);

        _joint.Rotate(deltaTheta);

        float distance2 = GetDistance(m_end.transform.position, m_target.transform.position);

        _joint.Rotate(-deltaTheta);

        return (distance2 - distance1) / deltaTheta;
    }

    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<FrankaJointsMsg>(topicName);

        m_JointArticulationBodies = new ArticulationBody[k_num_joints];
        var linkName = string.Empty;
        for (var i = 0; i < k_num_joints; i++)
        {
            linkName += LinkNames[i];
            m_JointArticulationBodies[i] = m_Franka.transform.Find(linkName).GetComponent<ArticulationBody>();
        }
    }
    void Update()
    {
        //        timeElapsed += Time.deltaTime;
        //        if (timeElapsed > publishMessageFrequency)
        //{
       // Debug.Log(Vector3.Distance(m_end.transform.position, m_target.transform.position) > m_threshold);
        if (Vector3.Distance(m_end.transform.position, m_target.transform.position) > m_threshold)
        {
            for (int i = 0; i < m_steps; ++i)
            {

                
                if (Vector3.Distance(m_end.transform.position, m_target.transform.position) > m_threshold)
                {

                    Joints current = m_root;
                    while (current != null)
                    {
                        float slope = CalculateSlope(current);
                        current.Rotate(-slope * m_rate);
                        current = current.GetChild();   
                        //Early Termination
                        if (GetDistance(m_end.transform.position, m_target.transform.position) < m_threshold)
                        {
                            return;
                        }
                    }

                }
                //            timeElapsed = 0;
            }
        }
        else
        {
            var DestinationMsg = new FrankaJointsMsg();
            timeElapsed += Time.deltaTime;
            for (int j=0;j< k_num_joints; j++)
            {
                DestinationMsg.joints[j] = m_JointArticulationBodies[j].jointPosition[0];
            }
            
            if (timeElapsed > publishMessageFrequency)
            {
                //Debug.Log("IKM: " + (float)DestinationMsg.joints[0] + " 1: " + (float)DestinationMsg.joints[1] + " 2: " + (float)DestinationMsg.joints[2] + " 3: " + (float)DestinationMsg.joints[3] + " 4: " + (float)DestinationMsg.joints[4] + " 5: " + (float)DestinationMsg.joints[5]);
                ros.Publish(topicName, DestinationMsg);
                timeElapsed = 0;
            }
        }
    }

    float GetDistance(Vector3 _point1, Vector3 _point2)
    {
        return Vector3.Distance(_point1, _point2);
    }
}