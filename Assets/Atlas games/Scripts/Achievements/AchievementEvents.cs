using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementEvents
{

}
public class BaseEvent
{
    public static bool CheckPassed(bool value, string ID)
    {
        if (Trophy.Trophies[ID].status == TrophyStatus.PAYED) return false; // If already payed, do nothing

        if (value && Trophy.Trophies[ID].status == TrophyStatus.PENDING) // If the achievement check is passed and the status is pending return true / set the status to achieved
        { Trophy.SetStatus(ID, TrophyStatus.ACHIEVED); return true; }

        if (value && Trophy.Trophies[ID].status == TrophyStatus.UNKNOWN) // If the achievement check is passed and the status is unknown return false / set the status to Payed, because in this senario it did already payed and the playerprefs did reset
        { Trophy.SetStatus(ID, TrophyStatus.PAYED); return false; }

        else if (Trophy.Trophies[ID].status == TrophyStatus.UNKNOWN) // the only way the status gets pass is if its Pending and the only way its get to the pending is this line 
        { Trophy.SetStatus(ID, TrophyStatus.PENDING); return false; }

        else return false;
    }
}
public class PassedFirstLevel : BaseEvent, IAchievement
{
    public static string ID = "1";
    public static bool Check
    {
        get
        {
            bool result = GlobalValue.LevelPass >= 1;
            return CheckPassed(result, ID);
        }
    }
}
public class GetFirstKill : BaseEvent, IAchievement
{
    public static string ID = "3";
    public static bool Check
    {
        get
        {
            bool result = GlobalValue.KillCount >= 1;
            return CheckPassed(result, ID);
        }
    }
}
public class PassedFirstWorld : BaseEvent, IAchievement
{
    public static string ID = "2";
    public static bool Check
    {
        get
        {
            bool result = GlobalValue.WorldPass > 1;
            return CheckPassed(result, ID);
        }
    }
}