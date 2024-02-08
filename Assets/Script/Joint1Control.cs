using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using TMPro;

public class Joint1Control : MonoBehaviour
{

    [SerializeField]
    GameObject m_Panda;

    public TMP_Text feedbackText;

    private ArticulationBody[] articulationChain;
    private ArticulationBody rightFinger;
    private ArticulationBody leftFinger;


    public static readonly string[] LinkNames =
        { "panda_link0/panda_link1", "/panda_link2", "/panda_link3", "/panda_link4", "/panda_link5", "/panda_link6", "/panda_link7"};

    // Variables required for ROS communication
    [SerializeField]
    string m_TopicName = "/joint1_topic";
    string limitTopic = "/joint1_limit_topic";
    // ROS Connector
    ROSConnection m_Ros;

    // Start is called before the first frame update
    void Start()
    {
        articulationChain = m_Panda.GetComponentsInChildren<ArticulationBody>();
        int defDyanmicVal = 10;
        foreach (ArticulationBody joint in articulationChain)
        {
            joint.gameObject.AddComponent<JointControl>();
            joint.jointFriction = defDyanmicVal;
            joint.angularDamping = defDyanmicVal;
            ArticulationDrive currentDrive = joint.xDrive;
            currentDrive.forceLimit = 10000;
            joint.xDrive = currentDrive;
        }
        leftFinger = articulationChain[11];
        rightFinger = articulationChain[12];

        // Get ROS connection static instance
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<StringMsg>(m_TopicName);

        m_Ros.Subscribe<StringMsg>(limitTopic, JointLimitCallback);
    }

    // Update is called once per frame
    void Update()
    {
        float joint1Direction = Input.GetAxisRaw("Horizontal");
        var char_input = new StringMsg();
        float joint6Direction = Input.GetAxis("Vertical");


        if (joint1Direction > 0)
        {
            char_input.data = "R";
            m_Ros.Publish(m_TopicName, char_input);
        }
        else if (joint1Direction < 0)
        {
            char_input.data = "L";
            m_Ros.Publish(m_TopicName, char_input);
        }
        else if (joint6Direction > 0)
        {
            char_input.data = "U";
            m_Ros.Publish(m_TopicName, char_input);
        }
        else if (joint6Direction < 0)
        {
            char_input.data = "D";
            m_Ros.Publish(m_TopicName, char_input);
        }
        else
        {

        }
    }

    private void JointLimitCallback(StringMsg msg)
    {
        feedbackText.text = msg.data;
    }
}
