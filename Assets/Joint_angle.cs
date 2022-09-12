using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint_angle : MonoBehaviour
{
    private ArticulationBody articulation;
    // Start is called before the first frame update
    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("asda: " + articulation.jointPosition[0] + ": " + articulation.jointVelocity[0]);
    }
}
