using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Threading;
using MoonSharp.Interpreter.Interop;
using Newtonsoft.Json;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine.Events;

public class LuaManager : MonoBehaviour
{
    public TextAsset luaTextAsset;
    public LuaAction luaActions;
    public float framerate = 30f;
    public Script script = new Script();

    private Task updateCoTask;
    private CancellationTokenSource cancellationTokenSource;


    void runFunction(string function, params DynValue[] args)
    {
        try
        {
            DynValue value = script.Globals.Get(function);
            value.Function.Call(args);
        }
        catch (System.Exception exception)
        {
            Debug.LogError(exception);
        }
    }
    void Initialize()
    {
        UserData.RegisterType<GlobalValue>();
        UserData.RegisterType<LuaAction>();
        // create a userdata, again, explicitely.
        DynValue obj = UserData.Create(new GlobalValue());
        // create a userdata, again, explicitely.
        DynValue obj2 = UserData.Create(luaActions);
        // create a userdata, again, explicitely.
        // Update the UserDataRegistrator configuration
        script.Globals.Set("GV", obj);
        script.Globals.Set("Action", obj2);
#if UNITY_EDITOR
        script.Options.DebugPrint = s => { Debug.LogError(s); };
#endif
        script.DoString(luaTextAsset.text);
    }
    void Awake()
    {
        Initialize();
        runFunction("Awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        runFunction("Start");
        StartUpdateCo();
    }
    private void StartUpdateCo()
    {
        cancellationTokenSource = new CancellationTokenSource();
        updateCoTask = Task.Run(UpdateCo, cancellationTokenSource.Token);
    }

    private void StopUpdateCo()
    {
        if (updateCoTask != null && !updateCoTask.IsCompleted)
        {
            cancellationTokenSource.Cancel();
            updateCoTask.Wait();
        }
    }

    private async Task UpdateCo()
    {
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {

            runFunction("Update");
            await Task.Delay((int)(1f / framerate * 1000f));
            // Check cancellation token to handle stop request
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                Debug.Log("Cancellation");
                break;
            }
        }

    }
    void OnApplicationQuit()
    {
        StopUpdateCo();
    }
    ~LuaManager()
    {
        StopUpdateCo();
    }

}
[Serializable]
public class LuaAction
{
    public UnityEvent listener;
    public void Invoke()
    {
        listener.Invoke();
    }
}