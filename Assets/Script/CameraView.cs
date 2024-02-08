using RosMessageTypes.Sensor;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine.UI;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    [SerializeField] private string compressedImageTopic = "/camera/color/image_raw/compressed";
    [SerializeField] private string cameraPositionTopic = "camera_position";

    private RawImage rawImage;

    private ROSConnection rosConnection;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = gameObject.GetComponent<RawImage>();
        Debug.Assert(rawImage != null);

        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.Subscribe<CompressedImageMsg>(compressedImageTopic, (compressedImage) =>
        {
            rawImage.texture = compressedImage.ToTexture2D();
        });

        rosConnection.Subscribe<PoseMsg>(cameraPositionTopic, cameraPositionCallback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void cameraPositionCallback(PoseMsg pose)
    {
        var position = pose.position.From<FLU>();
        var orientation = pose.orientation.From<FLU>();

        position.y = 0.025f;
        gameObject.transform.position = position;
        gameObject.transform.rotation = orientation;
    }
}
