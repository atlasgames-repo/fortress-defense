using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaManager : MonoBehaviour
{
    public static LuaManager self;
    public Achivements achivements;
    public LuaScript lua;
    void Awake()
    {
        if (self == null) self = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    async void Start()
    {
        achivements = new Achivements(await APIManager.instance.Get_Achivements());
        lua.Initialize(ref achivements);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
