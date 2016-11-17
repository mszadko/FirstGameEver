using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{ 
    public GameObject[] players;
    Vector3 desiredPosition;
    Vector3 velocity;
    float maxSize = 4;
    float minSize = 1;
    float edgeBuffer = 1;
    float zoomSpeed;
    new Camera camera;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Length==0)
        {
            return;
        }
        Move();
        Zoom();
        
    }
    void Move()
    {
        FindPosition();
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f);
    }
    void Zoom()
    {
        float size = FindSize();
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, size, ref zoomSpeed, 1f);

    }
    void FindPosition()
    {
        Vector3 avg = new Vector3(0,0,0);
        int activePlayersCount = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<Player>().active)
            {
                avg += players[i].transform.position;
                activePlayersCount++;
            }
        }
        if (activePlayersCount!=0)
        {
            avg /= activePlayersCount;
        }
        avg.z = transform.position.z;
        desiredPosition = avg;
    }
    float FindSize()
    {
        float size = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<Player>().active)
                continue;
            size = Mathf.Max(size, Mathf.Abs(transform.position.y - players[i].transform.position.y));
            size = Mathf.Max(size, (transform.position.x - players[i].transform.position.x) / camera.aspect);
            size = Mathf.Clamp(size, minSize, maxSize);
        }
        size += edgeBuffer;
        return size;
     }
}
