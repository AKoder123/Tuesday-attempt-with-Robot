using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Sensor;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.UrdfImporter;
using UnityEngine;

public class JointStateSubcriber : MonoBehaviour
{
    [SerializeField] private string jointStatesTopic = "/joint_states";

    private ROSConnection rosConnection;

    private Dictionary<string, ArticulationBody> namedArticulationBodies;
    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.Subscribe<JointStateMsg>(jointStatesTopic, JointStateSubscription);

        namedArticulationBodies = new Dictionary<string, ArticulationBody>();

        var articulationBodies = this.GetComponentsInChildren<ArticulationBody>();
        foreach (var articulationBody in articulationBodies)
        {
            var urdfJoint = articulationBody.gameObject.GetComponent<UrdfJoint>();
            if (urdfJoint != null)
            {
                namedArticulationBodies[urdfJoint.jointName] = articulationBody;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void JointStateSubscription(JointStateMsg message)
    {
        for (var i = 0; i < message.name.Length; i++)
        {
            var name = message.name[i];
            var position = message.position[i];
            var velocity = message.velocity[i];
            var force = message.effort[i];

            ArticulationBody articulationBody;
            if (namedArticulationBodies.TryGetValue(name, out articulationBody))
            {
                articulationBody.jointPosition = new ArticulationReducedSpace((float)position);
            }
        }
    }
}
