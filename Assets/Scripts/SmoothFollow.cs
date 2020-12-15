﻿using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float boostDistance = 3f;
    [SerializeField] private float normalDistance = 4f;
    
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float height = 5.0f;
    [SerializeField] private float heightDamping = 2.0f;
    [SerializeField] private float rotationDamping = 3.0f;
    [SerializeField] private Transform target;

    public void BoostZoom()
    {
        distance = boostDistance;
       
    }
    public void SetNormalZoom()
    {
        distance = normalDistance;
    }
    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target) return;
        // Calculate the current rotation angles
        var wantedRotationAngle = target.eulerAngles.y;
        var wantedHeight = target.position.y + height;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var pos = transform.position;
        pos = target.position - currentRotation * Vector3.forward * distance;
        pos.y = currentHeight;
        transform.position = pos;

        // Always look at the target
        transform.LookAt(target);
    }
    
}