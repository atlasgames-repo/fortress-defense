using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AchievementScheduler : BasePlayerPrefs<AchievementScheduleModel>
{
    public AchievementsData data;

    public float InitialDelaySeconds;
    public float ListenerTickSeconds;
    public AnimationCurve Probability;
    public AchievementScheduleModel[] Schedules;
    private Coroutine Dispacher;

    void Start()
    {
        
        Dispacher = StartCoroutine(Listener());
    }
    IEnumerator Listener()
    {
        
        yield return new WaitForSeconds(InitialDelaySeconds);
        while (true)
        {
            foreach (AchievementScheduleModel schedules in Schedules)
            {
                // check if the schedule is expired or active
                AchievementScheduleModel foundSchedule = DictArray.Where(a => a.type == schedules.type && a.status == ScheduleStatus.PENDING).FirstOrDefault();
                // Executes Only of the schedule is expired or its one time and done or expired
                if (foundSchedule != null)
                {
                    int expired = DateTime.Compare(DateTime.Now, foundSchedule.ExpireDate);
                    if (expired < 0 && foundSchedule.type != ScheduleType.ONETIME) continue;  // if the schedule isn't expired

                    int successfullTasks = 0;
                    foreach (AchievementModel item in BasePlayerPrefs<AchievementModel>.DictArray.Where(a => a.Schedul_id == foundSchedule._id).ToArray()) // deactivate the expired Tasks
                    {
                        item.isActive = expired < 0 && item.isActive && item.status != TrophyStatus.PAYED;
                        
                        if (item.status == TrophyStatus.PAYED) successfullTasks++;
                        BasePlayerPrefs<AchievementModel>.Update(item._id, item);
                    }
                    foundSchedule.status = successfullTasks < foundSchedule.NumberOfMissions ? expired >= 0 ? ScheduleStatus.EXPIRED: ScheduleStatus.PENDING : ScheduleStatus.DONE;
                    Update(foundSchedule._id, foundSchedule);
                    if (foundSchedule.status == ScheduleStatus.PENDING) continue;
                }
                // add new schedule of this type
                AchievementScheduleModel new_schedule = new AchievementScheduleModel(schedules.type, schedules.NumberOfMissions, schedules.name);

                Add(new_schedule._id, new_schedule);
                
                for (int i = 0; i < schedules.NumberOfMissions; i++)
                {
                    AchievementModel new_achievement = GetRandomAchievemntByType(GetAchievementType);
                    new_achievement._id = Guid.NewGuid();
                    new_achievement.Schedul_id = new_schedule._id;
                    AchievementTasksV2.self.AddNewAchievements(new AchievementModel[1]{new_achievement});
                } 
            }
            yield return new WaitForSeconds(ListenerTickSeconds);
        }
    }
    public AchievementModel GetRandomAchievemntByType(AchievementType _type){
        return data.models.Where(achiv => achiv.type == _type).OrderBy(x => UnityEngine.Random.value).First();
    }
    public AchievementType GetAchievementType{
        get{
            float P = UnityEngine.Random.Range(0f,1f);
            float C = Probability.Evaluate(P);
            int i = Mathf.RoundToInt(C);
            return (AchievementType)i;
        }
    }
    
}

