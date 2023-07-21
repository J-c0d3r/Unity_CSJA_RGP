using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public float zoomSpd = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;

    public float currentZoom = 10f;


    public Transform player;
    public Vector3 offset;


    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpd;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);   

    }

    private void LateUpdate()
    {
        transform.position = player.position - offset * currentZoom;
        transform.LookAt(player.position + Vector3.up * pitch);
    }
}
