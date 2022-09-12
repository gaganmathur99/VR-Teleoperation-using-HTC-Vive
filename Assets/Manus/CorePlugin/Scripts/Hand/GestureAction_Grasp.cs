using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manus.Hand;
using Manus.Hand.Gesture;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.ProjectMsg;


namespace Manus.Hand
{
    public class GestureAction_Grasp : MonoBehaviour
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
        private string message;
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
            if (timeElapsed >= publishMessageFrequency)
            {
                foreach (GestureSimple t_Gesture in m_LoggedGestures)
                {
                    var cubeRenderer = cube.GetComponent<Renderer>();
                    if (t_Gesture != null)
                    {
                        if (t_Gesture.Evaluate(m_Hand))
                        {
                            // Call SetColor using the shader property name "_Color" and setting the color to red
                            cubeRenderer.material.SetColor("_Color", Color.red);
                            message = "Grasp";
                            commandmessage.data = message;
                            // Finally send the message to server_endpoint.py running in ROS
                            ros.Publish(topicName, commandmessage);
                            new WaitForSeconds(5);
                        }
                        else
                        {
                            // Call SetColor using the shader property name "_Color" and setting the color to red
                            cubeRenderer.material.SetColor("_Color", Color.white);
                        }

                    }
                }
                timeElapsed = 0;
            }

        }
    }
}