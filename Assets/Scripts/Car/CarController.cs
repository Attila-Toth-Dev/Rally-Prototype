using UnityEngine;
using UnityEngine.InputSystem;

enum Drivetrain
{
    Rwd,
    Awd,
    Fwd
}

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Wheel[] wheelColliders;

    [Header("Engine")]
    [SerializeField] private float maxHorsepower;

    [Header("Handling")]
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float steeringSensitivity;

    [Header("Controls")]
    [SerializeField] private InputActionReference moveCarReference;
    [SerializeField] private InputActionReference steerCarReference;

    [Header("Debugging")]
    [SerializeField] private float moveAmount;
    [SerializeField] private float steerAmount;
    [SerializeField] private float currentMove;
    [SerializeField] private float currentSteer;

    private void OnEnable()
    {
        moveCarReference.action.Enable();
        steerCarReference.action.Enable();
    }

    private void OnDisable()
    {
        moveCarReference.action.Disable();
        steerCarReference.action.Disable();
    }

    private void Update()
    {
        CalculateMovement();
        CalculateSteering();
    }

    private void CalculateMovement()
    {
        moveAmount = moveCarReference.action.ReadValue<float>();


    }

    private void CalculateSteering()
    {
        steerAmount = steerCarReference.action.ReadValue<float>();

        currentSteer = Mathf.Lerp(currentSteer, maxSteerAngle, steeringSensitivity);
    }

    private void FixedUpdate()
    {
        MoveCar();
        SteerCar();
    }

    private void MoveCar()
    {
        foreach (Wheel wheel in wheelColliders)
        {
            wheel.WheelCollider.motorTorque = 1000 * moveAmount;
        }
    }

    private void SteerCar()
    {
        foreach (Wheel wheel in wheelColliders)
        {
            if(wheel.Axle == Axle.Front)
                wheel.WheelCollider.steerAngle = 100 * steerAmount;
        }
    }
}
