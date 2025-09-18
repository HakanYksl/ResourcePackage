using System;
using System.Collections.Generic;
using UnityEngine;
public class RewardDeployer : MonoBehaviour
{
    [SerializeField] private List<ResourceReward> _rewards;
    [SerializeField] private List<ResourceSpawner> _resourceSpawners;
    [SerializeField] private RewardsGroupDisplayer _groupDisplayer;
    [SerializeField] private ValueMultiplier _FormulatedMultiplier;
    [SerializeField] private float FormulatedMultiplier=>_FormulatedMultiplier?_FormulatedMultiplier.Value:1;
    public List<ResourceReward> CurrentRewards 
    {
        get => _rewards;
        set => _rewards = value;
    }
    public List<ResourceReward> Rewards=>_rewards;
    public void Refresh()
    {
        if(!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    public void ResetRewardMultipliers()
    {
        foreach(ResourceReward reward in _rewards)
        {
            reward.RewardMultiplier = 1;
        }
    }
    public void SetRewardAmount(Resource resource,int rewardAmount)
    {
        foreach (ResourceReward reward in _rewards)
        {
            if(reward.RewardItem.Resource==resource)
            {
                reward.SetRewardAmount((int)(rewardAmount*FormulatedMultiplier));
            }
        }
    }
    public void DeployRewards(List<ResourceReward> rewards,Action OnRewardCollectionIsCompleted)
    {
        _rewards= rewards;
        DeployRewards(OnRewardCollectionIsCompleted);
    }

    public void DeployRewards(Action OnRewardCollectionIsCompleted)
    {
        if (CurrentRewards!=null)
        {
            foreach (ResourceReward reward in CurrentRewards)
            {
                foreach (ResourceSpawner spawner in _resourceSpawners)
                {
                    if (reward.RewardItem == spawner.Resource)
                    {
                        int rewardAmount= (int)(reward.RewardAmount*FormulatedMultiplier);
                        spawner.SpawnResourceItems(rewardAmount, (isLast) =>
                        {
                            if (isLast)
                            {
                                OnRewardCollectionIsCompleted?.Invoke();
                            }
                        });
                    }
                }
            }
        }
    }


}
