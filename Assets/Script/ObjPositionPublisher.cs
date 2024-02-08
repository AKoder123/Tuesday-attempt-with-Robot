using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine.EventSystems;

public class ObjPositionPublisher : MonoBehaviour
{

    [SerializeField] 
    string positionTopic = "/obj_position_topic";

    ROSConnection rosConnection;


    // Start is called before the first frame update
    void Start()
    {
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RegisterPublisher<Vector3Msg>(positionTopic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void publishPosition(GameObject gameObject)
    {
        Debug.Log("clicked");
        var position = new Vector3Msg();
        position = gameObject.transform.position.To<FLU>();
        rosConnection.Publish(positionTopic, position);
    }
}
