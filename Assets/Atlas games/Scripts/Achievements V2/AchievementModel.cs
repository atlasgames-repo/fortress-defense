using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType
{
    NORMAL,
    RARE,
    EPIC,
    LEGENDARY
}

[Serializable]
public class AchievementModel : BaseModel
{
    public string _id;
    public string name;
    public string description;
    public DateTime expire_date;
    public int reward;
    public int checkpoint;
    public string fieldName;
    public AchievementType type = AchievementType.NORMAL;
    public TrophyStatus status = TrophyStatus.UNKNOWN;

}

[Serializable]
public class AchievementUpdateModel : BaseModel
{
    public string id;
    public int status;
    public AchievementUpdateModel(){
        id = "0";
        status = 0;
    }
    public AchievementUpdateModel(string _id, int _status){
        id = _id;
        status = _status;
    }

}
