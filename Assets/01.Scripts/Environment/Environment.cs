using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentType
{
    Green,
    Autumn,
    Desert,
    Tundra,
}
public class Environment : MonoBehaviour
{
    public EnvironmentType environmentType;
}
