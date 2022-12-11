using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementListener : MonoBehaviour
{
    public static AchivementListener self;

    void Awake()
    {
        self = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Listener());
    }

    IEnumerator Listener()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (PassedFirstLevel.check)
                Trophy.Achive(PassedFirstLevel.ID);
            if (GetFirstKill.check)
                Trophy.Achive(GetFirstKill.ID);
            if (PassedFirstWorld.check)
                Trophy.Achive(PassedFirstWorld.ID);
        }
    }
}

public class PassedFirstLevel
{
    public static string ID = "1";
    public static bool check
    {
        get
        {
            if (Trophy.self.Trophies[ID].is_served)
                return false;
            if (GlobalValue.LevelPass >= 1)
                return true;
            else
                return false;
        }
    }
}
public class GetFirstKill
{
    public static string ID = "3";
    public static bool check
    {
        get
        {
            if (Trophy.self.Trophies[ID].is_served)
                return false;
            if (GlobalValue.KillCount >= 1)
                return true;
            else
                return false;
        }
    }
}
public class PassedFirstWorld
{
    public static string ID = "2";
    public static bool check
    {
        get
        {
            if (Trophy.self.Trophies[ID].is_served)
                return false;
            if (GlobalValue.WorldPass > 1)
                return true;
            else
                return false;
        }
    }
}