using Essentials;
using Essentials.Managers;
using UnityEngine;

public class ResourceItemSpawner : MonoBehaviour
{
    [SerializeField] private bool _canGenerate;
    [SerializeField] private ResourceItemPile _resourceItemPile;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ePoolType poolType;

    //Must Consider boosters and multipliers
    public void SpawnItem(int amount)
    {
        if (!_canGenerate) return;
        CollectibleResourceItem item = PoolManager.Instance.GetPooledObject(poolType).GameObject.GetComponent<CollectibleResourceItem>();
        item.GameObject.SetActive(true);
        item.transform.position = _spawnPoint.position;
        item.transform.rotation = Quaternion.identity;
        item.SetAmount((int)(amount * DoubleCoinsBoost.Instance.Value));
        item.DropToPosition(_resourceItemPile.ItemDropPoint.transform);
        SoundManager.Instance.PlayAudioClip(item.VFXOnSpawn);

    }
    public void DetachFromParent()
    {
        if (!transform.parent)
        {
            return;
        }
        transform.SetParent(LevelManager.Instance.levelArea);
    }
}
