using UnityEngine;

[CreateAssetMenu(fileName = "NewRewardList", menuName = "ScriptableObjects/RewardList", order = 1)]
public class RewardList : ScriptableObject
{
    public Reward[] rewards;
}