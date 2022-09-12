using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IK_Articulated : MonoBehaviour
{
    //Articulation Bodies
    ArticulationBody[] m_JointArticulationBodies;

    public GameObject m_target;

    [SerializeField]
    GameObject m_Franka;
    public GameObject Franka { get => m_Franka; set => m_Franka = value; }

    public float m_threshold = 0.05f;

    public float m_rate = 5.0f;

    public int m_steps = 20;
    public int delay = 1000;
    public float delta_theta = 5.0f;
    //Hardcoded Variables
    const int k_num_joints = 7;
    public static readonly string[] LinkNames =
       { "Base_Link/Link_1", "/Link_2", "/Link_3", "/Link_4", "/Link_5", "/Link_6", "/Link_7" };

    
    // Start is called before the first frame update
    void Start()
    {
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
        for (int j = 0; j < m_steps; ++j)
        {
            if (Vector3.Distance(m_JointArticulationBodies[k_num_joints - 1].transform.position, m_target.transform.position) > m_threshold)
            {
                Debug.Log(Vector3.Distance(m_JointArticulationBodies[k_num_joints - 1].transform.position, m_target.transform.position));
                for (var i = 0; i < k_num_joints; i++)
                {
                    float slope = CalculateSlope(m_JointArticulationBodies[i]);
                    RotateTo(-slope * m_rate, m_JointArticulationBodies[i]);
                    if (Vector3.Distance(m_JointArticulationBodies[k_num_joints - 1].transform.position, m_target.transform.position) < m_threshold)
                    {
                        return;
                    }
                }
            }
        }
    }
    float CalculateSlope(ArticulationBody Body_Link)
    {
        //float deltaTheta = speed* Time.fixedDeltaTime;  // in degrees    0.05f;
        float distance1 = Vector3.Distance(m_JointArticulationBodies[k_num_joints - 1].transform.position, m_target.transform.position);

        float rotationGoal = (Mathf.Rad2Deg * Body_Link.jointPosition[0]) + delta_theta;
        RotateTo(rotationGoal, Body_Link);
        float distance2 = Vector3.Distance(m_JointArticulationBodies[k_num_joints - 1].transform.position, m_target.transform.position);
        rotationGoal = (Mathf.Rad2Deg * Body_Link.jointPosition[0]) - delta_theta;
        RotateTo(rotationGoal, Body_Link);
        

        return (distance2 - distance1) / delta_theta;
    }

    void RotateTo(float primaryAxisRotation, ArticulationBody Body_Link)
    {
        var drive = Body_Link.xDrive;
        drive.target = primaryAxisRotation;
        Body_Link.xDrive = drive;
        Task.Delay(delay);
    }

}

