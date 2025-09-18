using _ExtentionMethods;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardsGroupDisplayer : MonoBehaviour
{
    [SerializeField] private Transform _rewardDisplayers;
    public void SetRewards(List<ResourceReward> rewards)
    {
        List<RewardDisplayer> rewardDisplayers = _rewardDisplayers.GetComponentsInChildren<RewardDisplayer>().ToList();
        _rewardDisplayers.HideAllChildren();
        foreach(ResourceReward reward in rewards)
        {
            RewardDisplayer rewardDisplayer=null;
            foreach (RewardDisplayer displayer in rewardDisplayers)
            {
                if(displayer.Resource==reward.RewardItem.Resource)
                {
                    displayer.SetReward(reward);
                    break;
                }
            }
            if(!rewardDisplayer)
            {
                Debug.LogAssertion("Reward Displayer Not Found");
            }
        }

    }
}
