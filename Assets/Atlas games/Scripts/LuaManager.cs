using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;

public class LuaManager : MonoBehaviour
{
    public TextAsset[] scriptsFile;
    private Script[] scripts;

    public void Initialize()
    {
        scripts = new Script[scriptsFile.Length];
        foreach (Script item in scripts)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

