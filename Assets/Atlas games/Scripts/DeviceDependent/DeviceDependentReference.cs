using System;
using UnityEngine;

[Serializable]
public class DeviceDependentReference
{
    public GameObject mobile_object, pc_object;

    public GameObject Object {
        get {
        return SystemInfo.deviceType == DeviceType.Handheld || UIManager.IsReversed ? mobile_object : pc_object;
        }
    }

    public T type<T>() {
        Object.TryGetComponent<T>(out T component);
        if (component == null) {
            Debug.LogError($"DeviceDependentReference: {typeof(T)} not found on {Object.name}");
            throw new Exception($"DeviceDependentReference: {typeof(T)} not found on {Object.name}");
        }
        return component;
    }
}
