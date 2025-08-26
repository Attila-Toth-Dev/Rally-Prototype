using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
enum Drivetrain
{
    Rwd,
    Awd,
    Fwd
}

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Wheel[] wheels;

    [Header("Engine")]
    [SerializeField] private float maxHorsepower;

    [Header("Drivetrain")]
    [SerializeField] private Drivetrain drivetrain = Drivetrain.Awd;

    [Header("Handling")]
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float steeringSensitivity;

    [Header("Braking")]
    [SerializeField] private float brakeForce;

    [Header("Controls")]
    [SerializeField] private InputActionReference accelCarReference;
    [SerializeField] private InputActionReference decelCarReference;
    [SerializeField] private InputActionReference steerCarReference;
    [SerializeField] private InputActionReference handbrakeCarReference;

    [Header("Debugging")]
    [SerializeField] private float accelAmount;
    [SerializeField] private float decelAmount;
    [SerializeField] private float steerAmount;
    [SerializeField] private bool isBraking;
    [SerializeField] private bool isHandbraking;

    private float currentAccel;
    private float currentDecel;
    private float currentSteer;

    private void OnEnable()
    {
        accelCarReference.action.Enable();
        decelCarReference.action.Enable();

        steerCarReference.action.Enable();
        handbrakeCarReference.action.Enable();
    }

    private void OnDisable()
    {
        accelCarReference.action.Disable();
        decelCarReference.action.Disable();

        steerCarReference.action.Disable();
        handbrakeCarReference.action.Disable();
    }

    private void Update()
    {
        GetInputs();

        CalculateMovement();
        CalculateSteering();
    }

    private void GetInputs()
    {
        accelAmount = accelCarReference.action.ReadValue<float>();
        decelAmount = decelCarReference.action.ReadValue<float>();
        steerAmount = steerCarReference.action.ReadValue<float>();

        isBraking = decelCarReference.action.IsInProgress();
        isHandbraking = handbrakeCarReference.action.IsInProgress();
    }

    private void CalculateMovement()
    {
        currentAccel = Mathf.Lerp(currentAccel, maxHorsepower * accelAmount, Time.deltaTime);
        currentDecel = Mathf.Lerp(currentDecel, brakeForce * decelAmount, Time.deltaTime);
    }

    private void CalculateSteering()
    {
        currentSteer = Mathf.Lerp(currentSteer, maxSteerAngle * steerAmount, steeringSensitivity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Moving();
        Braking();

        SteerCar();
        Handbrake();
    }

    private void Moving()
    {
        foreach (Wheel wheel in wheels)
            wheel.WheelCollider.motorTorque = currentAccel;
    }

    private void Braking()
    {
        foreach (Wheel wheel in wheels)
            wheel.WheelCollider.brakeTorque = isBraking ? brakeForce * decelAmount : 0;
    }

    private void Handbrake()
    {
        foreach (Wheel wheel in wheels)
            if (wheel.Axle == Axle.Rear)
                wheel.WheelCollider.brakeTorque = isHandbraking ? 10000 : 0;
    }

    private void SteerCar()
    {
        foreach (Wheel wheel in wheels)
            if (wheel.Axle == Axle.Front)
                wheel.WheelCollider.steerAngle = currentSteer;
    }
}
