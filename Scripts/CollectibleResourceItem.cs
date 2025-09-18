using DG.Tweening;
using Essentials;
using Essentials.Managers;
using Essentials.Utilities;
using MoreMountains.Feedbacks;
using SRF;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// There can be multiple resource items for a single resource
/// For example 3D coin in the game environment and the 2D coin in the UI
/// </summary>
public class CollectibleResourceItem : CollectibleItem, IPoolObject
{

    #region Events

    public static Action<CollectibleResourceItem> OnResourceCollected;
    public void RaiseResourceCollected() { OnResourceCollected?.Invoke(this); }

    public static Action<CollectibleResourceItem> OnResourceIsReadyToBeCollected;
    public void RaiseResourceIsReadyToBeCollected() { OnResourceIsReadyToBeCollected.Invoke(this); }

    #endregion

    public Resource Resource 
    {  
        get => _resource;
        set
        {
            _resource = value;
        }
    }

    [SerializeField] private Resource _resource;
    [SerializeField] protected int _resourceCount;
    [SerializeField] private Ease _dropEase;
    [SerializeField] private bool _collectOnEnable;
    [SerializeField] private ResourceSlotUI _collector;
    [SerializeField] private bool _canBeCollected;
    [SerializeField] private ePoolType _poolType;
    [SerializeField] protected Collider _collider;
    public ExtendedAudioClip VFXOnSpawn;
    public ExtendedAudioClip VFXOnCollect;
    public GameObject GameObject => gameObject;
    public ResourceSlotUI ResourceUIUpdater
    {
        get => _collector;
        set => _collector = value;
    }
    public int ResouceCount => _resourceCount;
    
    private void Start()
    {
        Vector3 originalScale= transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, 0.25f);
        transform.DOLocalRotate(Vector3.up * 360, 0.5f,RotateMode.FastBeyond360);
    }

    public void Setup(int layer,float sizeMultiplier, ResourceSlotUI collector)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * sizeMultiplier;
        GameObject.SetLayerRecursive(Utilities.LayerIndexSafe(layer));
        transform.localPosition = Vector3.zero;
        _collector = collector;
    }

    public void SetAmount(int i_ResourceCount)
    {
        _resourceCount = i_ResourceCount;
    }

    public virtual void GotoCollector(Transform i_Position, Action Callback = null)
    {
        if (_collectOnEnable)return;

        transform.SetParent(i_Position);
        Vector3 scale = transform.localScale;
        transform.localScale=Vector3.zero;
        transform.DOScale(scale * 1.5f, 0.35f)
            .OnComplete(() =>
            {
                transform.DOScale(scale, 0.35f);
            });
        transform.DOLocalRotate(Vector3.one * 720, 0.7f, RotateMode.FastBeyond360);
        transform.DOLocalMove(i_Position.position, 0.7f)
            .OnComplete(() =>
            {
                if (Callback != null)
                {
                    Callback();
                }
                PoolManager.Instance.ReturnObjectToPool(this, _poolType);
                HapticManager.Instance.PlayHaptic(GameConfig.Instance.HapticConfig.ButtonClick);
            })
            .SetEase(Ease.Linear);
    }

    public virtual void GetCollected()
    {
        if(IsCollected) return;
        if (!_canBeCollected) return;

        if (_collector)
        {
            // Fixed Collector
            _collector.MoveResourceToCollector(this);
        }
        else
        {
            SoundManager.Instance.PlayAudioClip(VFXOnCollect);
            //Finds the active collector by releasing a collection event
            RaiseResourceCollected();
        }
        IsCollected = true;
    }

    public void DropToPosition(Transform i_TargetPosition)
    {
        transform.SetParent(i_TargetPosition);
        _collectOnEnable = false;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x += 360 * 4;
        transform.DORotate(rot, 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            rot = transform.rotation.eulerAngles;
            rot.x += 10;
            transform.DOPunchRotation(rot, 0.15f, 0, 1);
            transform.DOJump(transform.position, 0.1f, 1, 0.15f);
        });

        transform.DOJump(i_TargetPosition.position, 10, 1, 0.5f).SetEase(_dropEase).OnComplete(() =>
        {
            EnableCollection();
        }).SetUpdate(UpdateType.Fixed);
        transform.DOLocalRotate(new Vector3(360, 360, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }

    public void EnableCollection()
    {
        if (_collider)
        {
            _collider.enabled = true;
        }
        _canBeCollected = true;
    }

    public void ReturnToPool()
    {
        IsCollected = false;
        EnableCollection();
        transform.DOKill();
    }
}
