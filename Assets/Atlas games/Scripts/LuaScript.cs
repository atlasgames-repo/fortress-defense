using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;
using System;
using UnityEngine.Events;

[Serializable]
public class LuaScript
{
    public TextAsset luaTextAsset;
    public LuaAction luaActions;
    public float framerate = 30f;
    public Script script = new Script();

    private UnityEngine.Coroutine LuaUpdateCoroutine;


    void runFunction(string function, params DynValue[] args)
    {
        try
        {
            DynValue value = script.Globals.Get(function);
            value.Function.Call(args);
        }
        catch (System.Exception exception)
        {
            throw exception;

        }
    }
    public void Initialize(ref Achivements achivements)
    {
        luaActions.listener.AddListener(LuaActionListener);
        UserData.RegisterType<GlobalValue>();
        UserData.RegisterType<LuaAction>();
        UserData.RegisterType<Achivements>();
        UserData.RegisterType<Achivement>();
        // create a userdata, again, explicitely.
        DynValue obj2 = UserData.Create(luaActions);
        DynValue obj3 = UserData.Create(achivements);
        // Update the UserDataRegistrator configuration
        script.Globals["GlobalValue"] = typeof(GlobalValue);
        script.Globals.Set("Action", obj2);
        script.Globals.Set("Achivement", obj3);
#if UNITY_EDITOR
        script.Options.DebugPrint = s => Debug.LogError("[LUA] - " + s);
#endif
        script.DoString(luaTextAsset.text);
        runFunction("Awake");
        runFunction("Start");
        LuaUpdateCoroutine = LuaManager.self.StartCoroutine(UpdateCoroutine());
    }
    private async void LuaActionListener(string data)
    {
        await APIManager.instance.Set_Achivements(new SetAchivementModel { id = data, is_achived = true });
    }
    private void StopUpdateCo()
    {
        if (LuaUpdateCoroutine != null)
        {
            LuaManager.self.StopCoroutine(LuaUpdateCoroutine);
            Debug.LogError("[LUA] - update coroutine stopped");
        }
    }
    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            runFunction("Update");
            yield return new WaitForSeconds(1f / framerate);
        }
    }

    void OnApplicationQuit()
    {
        StopUpdateCo();
    }
    ~LuaScript()
    {
        StopUpdateCo();
    }

}
[Serializable]
public class LuaAction
{
    public UnityEvent<string> listener;
    public void Invoke(string id)
    {
        listener.Invoke(id);
    }
}