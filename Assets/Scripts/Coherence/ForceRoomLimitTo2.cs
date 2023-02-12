using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Coherence.UI;
using UnityEngine;
using UnityEngine.UI;

public class ForceRoomLimitTo2 : MonoBehaviour
{
    public InputField roomLimitInputField;
    private PropertyInfo x;
    private void Start()
    {
        var t = GetComponentInChildren<RoomsConnectDialog>();
        x = t.GetType().GetProperty("RoomLimit");
        x.SetValue(t, 2);
        Debug.Log($"Set value = 2~");
    }

    private void Update()
    {
        if (x != null)
        {
            var t = GetComponentInChildren<RoomsConnectDialog>();
            x.SetValue(t, 2);
        }
    }
}
