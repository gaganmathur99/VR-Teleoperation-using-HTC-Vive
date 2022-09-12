using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manus.Hand;
using Manus.Hand.Gesture;

namespace Manus.Hand
{
    public class linear_drive : MonoBehaviour
    {
        public float speed = .02f;
        private List<GestureSimple> m_LoggedGestures;
        public Hand m_Hand;
        private ArticulationBody articulation;
        // Start is called before the first frame update
        void Start()
        {
            articulation = GetComponent<ArticulationBody>();
            //Get hand component data
            m_Hand = GetComponent<Hand>();
        }

        // Update is called once per frame
        void Update()
        {
            
            if (m_LoggedGestures[0].Evaluate(m_Hand))
            {
                var drive = articulation.zDrive;
                drive.target = speed;
                articulation.zDrive = drive;
                new WaitForSeconds(5);
            }
        }
    }
}
