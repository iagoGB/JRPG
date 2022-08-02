using UnityEngine;

public class CameraControl : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        var cameraPosition = transform.position;
        transform.position = new Vector3(x: cameraPosition.x, y: cameraPosition.y, player.transform.position.z - 3);
    }
}
