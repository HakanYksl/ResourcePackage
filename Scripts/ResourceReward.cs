using UnityEngine;

[System.Serializable]
public class ResourceReward
{
    public CollectibleResourceItem RewardItem;
    [SerializeField] private int _rewardAmount; 
    public int RewardAmount=>(int)(_rewardAmount*RewardMultiplier);
    public float RewardMultiplier=1;
    public float PhysicalSizeMultiplier=1;
    public int RoundUpTo;
    public LayerMask Layer;
    public bool CanBeMultiplied;

    public void SetRewardAmount(int rewardAmount)
    {
        _rewardAmount=rewardAmount;
    }
}
