using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[ExecuteInEditMode]
public class AsyncTest : MonoBehaviour
{
    public bool Run;
    // Update is called once per frame
    void Update()
    {
        if (!Run) return;
        Debug.LogError("AsyncTest Start");
        RunAsync();
        Debug.LogError("AsyncTest After Async call");
        RunSync();
        Debug.LogError("AsyncTest After Async call");



        Run = false;
    }

    public async void RunAsync()
    {
        await Task.Delay(1000);
        Debug.LogError("Async call End");

    }
    public void RunSync()
    {
        System.Threading.Thread.Sleep(500);
        Debug.LogError("Sync call End");


    }
}
