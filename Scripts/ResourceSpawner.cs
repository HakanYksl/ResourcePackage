using _ExtentionMethods;
using DG.Tweening;
using Essentials.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Spawns ResourceItems and activates them.
/// Spawns the items at same position but throws them at random positions
/// Only spawns one kind of Resource
/// </summary>
public class ResourceSpawner:MonoBehaviour
{
    [SerializeField] private CollectibleResourceItem _resourceItem;
    [SerializeField] private float _sizeMultiplier=1;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPositions;
    [SerializeField] private List<ResourceSlotUI> _resourceCollectors;
    [SerializeField] private int _itemCount;
    [SerializeField] private float _firstDelay;
    [SerializeField] private float _delayBetweenItems;
    [SerializeField] private float _delayAfterItemsSpawned;
    [SerializeField] private RandomizedFloat _jumpForce;
    [SerializeField] private RandomizedFloat _jumpDuration;
    public CollectibleResourceItem Resource => _resourceItem;
    public Action OnCollectionCompleted;
    private IEnumerator Co_SpawnResources(int amount, Action<bool> OnLastItemCollected)
    {
        yield return new WaitForSeconds(_firstDelay);
        List<CollectibleResourceItem> spawnedItems=new List<CollectibleResourceItem>();
        int remainingAmount = amount;
        int rewardCountForEachPiece = amount / _itemCount;
        CollectibleResourceItem lastItem=null;
        for (int i = 0; i < _itemCount; i++)
        {
            CollectibleResourceItem resourceItem = Instantiate(_resourceItem, _startPosition).GetComponent<CollectibleResourceItem>();
            resourceItem.Setup(_layer, _sizeMultiplier, FindResourceUIUpdater(resourceItem.Resource));
            spawnedItems.Add(resourceItem);
            Transform spawnPosition = _endPositions.PickRandom();
            resourceItem.transform.DOJump(spawnPosition.position, _jumpDuration,2, _jumpForce);
            if (remainingAmount < rewardCountForEachPiece)
            {
                resourceItem.SetAmount(remainingAmount);
            }
            else
            {
                resourceItem.SetAmount(rewardCountForEachPiece);
            }
            
            remainingAmount -= rewardCountForEachPiece;
            yield return new WaitForSeconds(_delayBetweenItems);
            if(i==_itemCount-1)
            {
                lastItem = resourceItem;
            }
        }

        yield return new WaitForSeconds(_delayAfterItemsSpawned);

        foreach (var resourceItem in spawnedItems)
        {
            yield return new WaitForSeconds(_delayBetweenItems);
            resourceItem.EnableCollection();
            resourceItem.GetCollected();
        }


        yield return new WaitUntil(() => { return lastItem.IsCollected == true; });
        OnLastItemCollected?.Invoke(true);
        OnCollectionCompleted?.Invoke();
    }

    public virtual void SpawnResourceItems(int amount,Action<bool>OnLastItemCollected)
    {
        StartCoroutine(Co_SpawnResources(amount, OnLastItemCollected));
    }

    public virtual void SpawnResourceItems()
    {

    }


    private ResourceSlotUI FindResourceUIUpdater(Resource i_Resource)
    {
        foreach (ResourceSlotUI resourceUIUpdater in _resourceCollectors)
        {
            if (i_Resource == resourceUIUpdater.Resource)
            {
                return resourceUIUpdater;
            }
        }
        //Debug.LogAssertion("Cannot Find Resource UI Updater for: " + i_Resource);
        return null;
    }
}
