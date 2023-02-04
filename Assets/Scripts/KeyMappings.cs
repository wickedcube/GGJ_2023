using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Key_Mapping", menuName = "Key Mapping/Map")]
public class KeyMappings : ScriptableObject
{
    public enum AxisInput
    {
        Horizontal,
        Vertical
    }

    public enum MouseInput
    {
        LMB = 0,
        RMB = 1
    }

    private static KeyMappings mappings;
    private static KeyMappings Mappings
    {
        get
        {
            if (mappings == null)
                mappings = Resources.Load<KeyMappings>("Key_Mapping");

            return mappings;
        }
    }
    
    [SerializeField]
    private AxisInput InputXAxis = AxisInput.Horizontal;
    [SerializeField]
    private AxisInput InputYAxis = AxisInput.Vertical;

    [SerializeField]
    private MouseInput MouseShootSqrt = MouseInput.LMB;
    [SerializeField]
    private MouseInput MouseShootCbqrt = MouseInput.RMB;

    [SerializeField]
    private KeyCode Grenade = KeyCode.G;
    [SerializeField]
    private KeyCode ChronoTrigger = KeyCode.Q;

    public static float GetAxis(AxisInput axisInput) => Input.GetAxis(axisInput.ToString());
    
    public static bool GetMouseButton(MouseInput input) => Input.GetMouseButton((int)input);
    public static bool GetMouseButtonDown(MouseInput input) => Input.GetMouseButtonDown((int)input);
    
    public static bool GetGrenadeKeyDown() => Input.GetKeyDown(Mappings.Grenade);
    public static bool GetChronoKey() => Input.GetKey(Mappings.ChronoTrigger);
}
