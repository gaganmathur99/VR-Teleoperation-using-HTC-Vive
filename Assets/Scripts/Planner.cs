using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.ProjectMsg;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.UrdfImporter;
using UnityEngine;

public class Planner : MonoBehaviour
{
    //Hardcoded Variables
    const int k_num_joints = 7;
    public static readonly string[] LinkNames =
        { "Base_Link/Link_1", "/Link_2", "/Link_3", "/Link_4", "/Link_5", "/Link_6", "/Link_7" };

    // Variables required for ROS communication
    ROSConnection ros;
    [SerializeField]
    public string topicName = "/franka_joints";

    [SerializeField]
    GameObject m_Franka;

    //Articulation Bodies
    ArticulationBody[] m_JointArticulationBodies;


    // Publish the joint angle detail every N sec
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            var DestinationMsg = new FrankaJointsMsg();

            for (var i = 0; i < k_num_joints; i++)
            {
                DestinationMsg.joints[i] = m_JointArticulationBodies[i].jointPosition[0];

            }
            Debug.Log("Planer: " + (float)DestinationMsg.joints[0] + " 1: " + (float)DestinationMsg.joints[1] + " 2: " + (float)DestinationMsg.joints[2] + " 3: " + (float)DestinationMsg.joints[3] + " 4: " + (float)DestinationMsg.joints[4] + " 5: " + (float)DestinationMsg.joints[5]);
            ros.Publish(topicName, DestinationMsg);

            timeElapsed = 0;
        }
    }
}
