using System;
using System.Collections;
using SOConfigs;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public Action OnBoost;
    public Action OnDie;

    [SerializeField] private Rigidbody spaceship;
    [SerializeField] private SpaceshipConfig spaceshipConfig;
    [SerializeField] private SmoothFollow mainCamera;
    [SerializeField] private LevelManager levelManager;
    
    private float moveForwardSpeed;
    private float moveRightLimit;
    
    private float startBoostFilling = 3f;
    private float boostFilling = 3f;
    private bool readyForSpeedUp;
    private bool readyForFillBoost;

    private bool IsAlive { get; set; }
    public float BoostFilling => boostFilling;

    private void Start()
    {
        moveForwardSpeed = spaceshipConfig.MoveForwardSpeed;
        moveRightLimit = spaceshipConfig.MoveRightLimit;
        
        readyForSpeedUp = true;
        readyForFillBoost = true;
        IsAlive = true;
    }

    private void FixedUpdate()
    {
        Move();
        SetSpeedBoost();
        FeelBoost();
    }

    private void Move()
    {
        if (!IsAlive) return;
        var inputHorizontal = Input.GetAxis("Horizontal");
        var direction = new Vector3(inputHorizontal, 0, 0);
        spaceship.velocity = Vector3.forward * moveForwardSpeed + direction * spaceshipConfig.MoveRightSpeed;
        TiltTheSpaceship(inputHorizontal);
    }

    private void TiltTheSpaceship(float inputX)
    {
        var clampedPositionX = Mathf.Clamp(transform.position.x, -moveRightLimit, moveRightLimit);
        var currentTransform = transform;
        var currentPosition = currentTransform.position;
        currentPosition = new Vector3(clampedPositionX, currentPosition.y, currentPosition.z);
        currentTransform.position = currentPosition;
        transform.rotation = Quaternion.Euler(0, 0, -inputX * spaceshipConfig.RotateAngleCoefficient);
    }

    private void SetSpeedBoost()
    {
        if (!readyForSpeedUp || !IsAlive) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        levelManager.DoublePoints = true;
        AudioManager.Instance.PlayEffect(spaceshipConfig.AccelerationSound);
        mainCamera.IsZoomBoosted = true;
        StartCoroutine(SetAcceleration(spaceshipConfig.AccelerationTime, spaceshipConfig.AccelerationCoefficient));
    }
    private IEnumerator SetAcceleration(float time, float coefficient)
    {
        moveForwardSpeed *= coefficient;
        readyForSpeedUp = false;
        readyForFillBoost = false;
        yield return new WaitForSeconds(time);
        moveForwardSpeed /= coefficient;
        mainCamera.IsZoomBoosted = false;
        readyForFillBoost = true;
    }
    private void FeelBoost()
    {
        //provided that the player is alive, change the filling of the acceleration scale
        if (!IsAlive) return;
        OnBoost?.Invoke();

        if (!readyForFillBoost)
        {
            boostFilling -= Time.deltaTime;
        }
        else if (readyForFillBoost && boostFilling < startBoostFilling)
        {
            levelManager.DoublePoints = false;
            boostFilling += Time.deltaTime;
            if (boostFilling >= startBoostFilling)
            {                    
                readyForSpeedUp = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //stop the object and apply effects to it at the time of death
        IsAlive = false;
        spaceship.velocity = Vector3.zero;
        AudioManager.Instance.PlayEffect(spaceshipConfig.DieSound);
        Instantiate(spaceshipConfig.DieParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        OnDie?.Invoke();
    }
}
