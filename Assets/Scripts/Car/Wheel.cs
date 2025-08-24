using System;
using UnityEngine;

[Serializable]
public enum Axle
{
    Front,
    Rear
}

public class Wheel : MonoBehaviour
{
    [Header("References")]
    public WheelCollider WheelCollider;
    public GameObject WheelObject;
    public Axle Axle;
}
