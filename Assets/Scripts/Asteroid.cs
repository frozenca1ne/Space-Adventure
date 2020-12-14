using UnityEngine;


public class Asteroid : MonoBehaviour
{
    
    [SerializeField] private int points = 5;
    [SerializeField] private int pointsPerAsteroid = 1;
    
    private Quaternion originalRotation;
    private float currentRotateAngle;
    
    private void Start()
    {
        originalRotation = transform.rotation;
    }
    private void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        //independent rotation on two axes
        currentRotateAngle++;
        var rotationY = Quaternion.AngleAxis(currentRotateAngle, Vector3.up);
        var rotationX = Quaternion.AngleAxis(currentRotateAngle, Vector3.right);
        transform.rotation = originalRotation * rotationY * rotationX;
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
