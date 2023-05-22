using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitY : MonoBehaviour
{
    [SerializeField] Rigidbody player;

    public float limit;
    Vector3 spawnpoint;

    private void Start()
    {
        spawnpoint = player.position;
    }

    void LateUpdate()
    {
        if (player.position.y < -limit) player.position = spawnpoint;
    }
    
    public void TP_Spawnpoint()
    {
        player.position = spawnpoint;
    }
}
