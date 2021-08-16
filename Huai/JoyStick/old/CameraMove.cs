using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float zDistance, xDistance;
    void Start()
    {
        zDistance = player.position.z - transform.position.z;
        xDistance = player.position.x - transform.position.x;
    }
    void Update()
    {
        transform.position = new Vector3(player.position.x - xDistance, transform.position.y, player.position.z-zDistance);
    }
}
