using System.IO;
using System.Reflection;
using UnityEngine;

public class DeviceDependentMonoBehaviour : MonoBehaviour {
    protected GameObject selected_target_object;
    protected virtual void Awake() {
        FieldInfo[] fileds = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo item in fileds) {
            object[] attrs = item.GetCustomAttributes(typeof(DeviceDependentAttribute), true);
            if (attrs.Length > 0) {
                DeviceDependentReference reference = (DeviceDependentReference)item.GetValue(this);
                GameObject target = reference.Object;
                selected_target_object = target;
                item.SetValue(this, new DeviceDependentReference{
                    mobile_object = target,
                    pc_object = target
                });
            }
        }
    }
}
