using _ExtentionMethods;
using Essentials.Managers;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemPile : MonoBehaviour
{
    [SerializeField] private Transform _itemPositionsContainer;
    [SerializeField] private ParticleSystem _sparkles;

    private int _itemCount;
    private void Awake()
    {
        _itemPositionsContainer.HideAllChildren();
        _sparkles.gameObject.SetActive(false);
    }
    public void DetachFromParent()
    {
        if (!transform.parent) return;
        transform.SetParent(LevelManager.Instance.levelArea);
    }
    private void ProcessSparkles()
    {
        if (_itemCount > 0)
        {
            if (!_sparkles.gameObject.activeInHierarchy)
            {
                _sparkles.gameObject.SetActive(true);
            }

            var emission = _sparkles.emission;
            emission.rateOverTime = _itemCount * 5;
        }
        else
        {
            if (_sparkles.gameObject.activeInHierarchy)
            {
                _sparkles.gameObject.SetActive(false);
            }
        }

    }
    public void AddItemToPile(CollectibleResourceItem item)
    {
        _itemCount++;
        _sparkles.transform.position = item.transform.position;
        ProcessSparkles();
    }
    public void CoinCollectedFromPile()
    {
        _itemCount--;
        ProcessSparkles();
    }
    public PileItemPosition ItemDropPoint
    {
        get
        {
            Transform emptyPoint = null;
            int minCount = 900;

            foreach (Transform child in _itemPositionsContainer)
            {
                int count = child.childCount;
                if (count < minCount)
                {
                    minCount = count;
                    emptyPoint = child;
                }
            }

            if (!emptyPoint)
            {
                emptyPoint = _itemPositionsContainer.PickRandom();
            }
            if(!emptyPoint.gameObject.activeInHierarchy)
            {
                emptyPoint.gameObject.SetActive(true);
            }
            return emptyPoint.GetComponent<PileItemPosition>();
        }
    }


}
