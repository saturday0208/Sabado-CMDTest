using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier = 0.5f;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;

        transform.position += new Vector3(delta.x * parallaxMultiplier, delta.y * parallaxMultiplier, 0f);

        lastCamPos = cam.position;
    }
}
