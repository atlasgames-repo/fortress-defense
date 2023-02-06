using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class KeyboardManager : MonoBehaviour, IKeyboardCall
{
    public void KeyDown(KeyCode keyCode)
    {

    }
    public KeyCode[] KeyType { get { return new KeyCode[] { Key }; } }
    public int KeyObjectID { get { return gameObject.GetInstanceID(); } }
    public KeyCode Key;
    public int EventLoopMilliseconds = 2;
    public static KeyboardManager self;
    private Dictionary<int, IKeyboardCall> calls = new Dictionary<int, IKeyboardCall>();
    public void Add(IKeyboardCall call)
    {
        calls.Add(call.KeyObjectID, call);
    }
    public void Remove(IKeyboardCall call)
    {
        calls.Remove(call.KeyObjectID);
    }
    public void Awake()
    {
        self = this;
    }
    public void Start()
    {
        var ikeyboardcalls = FindObjectsOfType<MonoBehaviour>().OfType<IKeyboardCall>();
        foreach (IKeyboardCall caller in ikeyboardcalls)
        {
            calls.Add(caller.KeyObjectID, caller);
        }

    }
    void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown)
                Call_keys(Event.current.keyCode);
        }
    }
    private void Call_keys(KeyCode keyCode)
    {
        foreach (KeyValuePair<int, IKeyboardCall> call in calls)
        {
            if (call.Value == null)
                continue;

            foreach (KeyCode item in call.Value.KeyType)
            {
                if (keyCode == item)
                    call.Value.KeyDown(keyCode);
            }
        }
    }
}
