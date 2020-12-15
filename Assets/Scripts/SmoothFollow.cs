using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float boostDistance = 3f;
    [SerializeField] private float normalDistance = 4f;
    
    [SerializeField] private float cameraHeight = 5.0f;
    [SerializeField] private float heightDamping = 2.0f;
    [SerializeField] private float rotationDamping = 3.0f;
    [SerializeField] private Transform target;

    public bool IsZoomBoosted { get; set; }

    private void FixedUpdate()
    {
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        SetCameraHeight();
        CalculateCameraPosition(IsZoomBoosted ? boostDistance : normalDistance);
    }

    private void CalculateCameraPosition(float distance)
    {
        if (!target) return;
        var targetPosition = target.position;
        var currentTransform = transform.position;
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var newPosition = targetPosition - Vector3.forward * distance;
        transform.position = Vector3.Lerp(currentTransform,newPosition,0.3f) ;
    }
    private void LateUpdate()
    {
        SetCameraRotation();
        // Always look at the target
        transform.LookAt(target);
    }

    private void SetCameraRotation()
    {
        if (!target) return;
        var wantedRotationAngle = target.eulerAngles.y;
        var selfRotationAngle = transform.eulerAngles.y;
        // Damp the rotation around the y-axis
        var newRotationAngle = Mathf.LerpAngle(selfRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        // Convert the angle into a rotation
        var newRotation = Quaternion.Euler(0, newRotationAngle, 0);
        transform.rotation = newRotation;
    }

    private void SetCameraHeight()
    {
        if (!target) return;
        var position = transform.position;
        var wantedHeight = target.position.y + cameraHeight;
        var selfHeight = position.y;
        // Damp the height
        var newHeight  = Mathf.Lerp(selfHeight, wantedHeight, heightDamping * Time.deltaTime);
        position.y = newHeight;
        transform.position = position;
    }
    
}