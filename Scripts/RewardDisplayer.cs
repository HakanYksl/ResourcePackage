using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardAmountText;
    [SerializeField] private Image _rewardImage;
    [SerializeField] private ResourceReward _reward;
    public Resource Resource=>_reward.RewardItem.Resource;

    public void SetReward(ResourceReward reward)
    {
        _reward = reward;
        gameObject.SetActive(true);
        _rewardImage.sprite=_reward.RewardItem.Resource.Sprite;
        _rewardAmountText.text=reward.RewardAmount.ToString();
    }
}
