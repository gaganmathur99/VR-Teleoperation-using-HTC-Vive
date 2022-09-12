using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class position : MonoBehaviour
{
    public float speed = 300.0f;

    private ArticulationBody articulation;
    public ArticulationBody m_EndEffector;
    public GameObject m_target;
    public float m_threshold = 0.05f;
    public float m_rate = 1000.0f;
    public int m_steps = 100;
    public float m_deltaTheta = 10.00f;
    public float time_delay = 2.0f;
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
            for (int j = 0; j < m_steps; ++j)
            {
                if (Vector3.Distance(m_EndEffector.transform.position, m_target.transform.position) > m_threshold)
                {
                    //Debug.Log(Vector3.Distance(m_EndEffector.transform.position, m_target.transform.position));
                    float slope = CalculateSlope();
                    Debug.Log("Update Slope: " + -slope);
                    RotateTo(-slope * m_rate);
                    //if (Vector3.Distance(m_EndEffector.transform.position, m_target.transform.position) < m_threshold)
                    //{
                    //    return;
                    //}

                }
            }
           
    }

    float CalculateSlope()
    {
     //   float deltaTheta = speed * Time.fixedDeltaTime;  // in degrees    0.05f;
        float distance1 = Vector3.Distance(m_EndEffector.transform.position, m_target.transform.position);
        float rotationGoal = (Mathf.Rad2Deg * articulation.jointPosition[0]) + m_deltaTheta;
        Debug.Log("Calculate Slope 1: " + rotationGoal);

        RotateTo(rotationGoal);

        float distance2 = Vector3.Distance(m_EndEffector.transform.position, m_target.transform.position);
        rotationGoal = (Mathf.Rad2Deg * articulation.jointPosition[0]) - m_deltaTheta;
        RotateTo(rotationGoal);
        
        Debug.Log("Calculate Slope 2: " + rotationGoal);

        return (distance2 - distance1) / m_deltaTheta;
    }

    void RotateTo(float primaryAxisRotation)
    {
        
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        
        articulation.xDrive = drive;
        Task.Delay(1000);
    }


}
