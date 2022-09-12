using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manus.Hand;
using Manus.Hand.Gesture;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.ProjectMsg;


namespace Manus.Hand
{
    public class GestureAction_Move : MonoBehaviour
    {
        //Setup ROS Connection and initialize topic name
        ROSConnection ros;
        public string topicName = "Hand_Gesture";

        // Publish the cube's position and rotation every N seconds
        public float publishMessageFrequency = 0.5f;

        // Used to determine how much time has elapsed since the last message was published
        private float timeElapsed;

        [SerializeField]
        private List<GestureSimple> m_LoggedGestures; 
        private Hand m_Hand;
        public GameObject cube;
        public ArticulationBody m_GripperL;
        public ArticulationBody m_GripperR;
        private string message;
        private float open = 0.00f;
        private float close = 0.04f;
        void Start()
        {

            // Create a new cube primitive to set the color on
            // start the ROS connection
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<HandCmdMsg>(topicName);
            //Get hand component data
            m_Hand = GetComponent<Hand>();
        }

        void FixedUpdate()
        {
            //Initialize clock
            timeElapsed += Time.deltaTime;
            var commandmessage = new HandCmdMsg();

            var cubeRenderer = cube.GetComponent<Renderer>();
            if (timeElapsed >= publishMessageFrequency)
            {
                if (m_LoggedGestures[0].Evaluate(m_Hand)){
                    cubeRenderer.material.SetColor("_Color", Color.black);
                    message = "Open_Gripper";
                    commandmessage.data = message;
                    // Finally send the message to server_endpoint.py running in ROS
                    ros.Publish(topicName, commandmessage);
                    GripperControl(open);
                    new WaitForSeconds(5);
                }
                else if (m_LoggedGestures[1].Evaluate(m_Hand))
                {
                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    cubeRenderer.material.SetColor("_Color", Color.red);
                    message = "Grasp";
                    commandmessage.data = message;
                    // Finally send the message to server_endpoint.py running in ROS
                    ros.Publish(topicName, commandmessage);
                    GripperControl(close);
                    new WaitForSeconds(5);
                }
                timeElapsed = 0;
            }
            
        }
        void GripperControl(float value)
        {
            var drive_L = m_GripperL.zDrive;
            var drive_R = m_GripperR.zDrive;
            drive_L.target = value;
            drive_R.target = value;
            m_GripperL.zDrive = drive_L;
            m_GripperR.zDrive = drive_R;
        }
    }
}