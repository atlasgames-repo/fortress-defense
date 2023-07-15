using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Threading.Tasks;

[Serializable]
public class AchievementEventsV2
{
    public AchievementModel model;
    public AchievementEventsV2(AchievementModel _model){
        model = _model;  
    }
    public int Value{
        get{
            Type type = typeof(GlobalValue);
            PropertyInfo field = type.GetProperty(model.fieldName, BindingFlags.Static | BindingFlags.Public);
            return (int)field.GetValue(null);
        }
    }
    public float Proccess{
        get{
            return (float)Value/model.checkpoint;
        }
    }
    public bool IsPassed{
        get{
            bool is_passed = Proccess >= 1;
            if (model.status == TrophyStatus.PAYED) return false; // If already payed, do nothing
            if (is_passed && model.status == TrophyStatus.PENDING) // If the achievement check is passed and the status is pending return true / set the status to achieved
            { Update_status(model._id, TrophyStatus.ACHIEVED); return true; }
            // if (is_passed && model.status == TrophyStatus.ACHIEVED) return true; // You Recived the achievement but didn't get the reward yet.
            if (is_passed && model.status == TrophyStatus.UNKNOWN) // If the achievement check is passed and the status is unknown return false / set the status to Payed, because in this senario it did already payed and the playerprefs did reset
            { Update_status(model._id, TrophyStatus.PAYED); return false; }
            else if (model.status == TrophyStatus.UNKNOWN) // the only way the status gets pass is if its Pending and the only way its get to the pending is this line 
            { Update_status(model._id, TrophyStatus.PENDING); return false; }
            else return false;
        }
    }
    public void Update_status(string id, TrophyStatus new_status){
        _ = Task.Run(() =>
        {
            model.status = new_status;
            BasePlayerPrefs<AchievementModel>.Update(model._id, model);
        });
    }
}
