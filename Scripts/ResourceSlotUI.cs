using DG.Tweening;
using Essentials;
using Essentials.Managers;
using TMPro;
using UnityEngine;

public class ResourceSlotUI : MonoBehaviour
{
    [SerializeField] private Resource m_Resource;
    [SerializeField] private TextMeshProUGUI m_ReseourceCountText;
    [SerializeField] private Transform m_Icon;
    [SerializeField] private Transform m_IconVisual;
    [SerializeField] private bool m_ListenCollectionEvents;

    public Resource Resource=>m_Resource;
    private Tween m_Tween;
    private ResourceManager ResourceManager=>ResourceManager.Instance;
    private void OnEnable()
    {
        ResourceManager.OnResourceCountChanged += HandleResourceCountChanged;
        LevelManager.OnLevelLoaded += HandleLevelLoad;
        if(m_ListenCollectionEvents)
        {
            CollectibleResourceItem.OnResourceCollected += HandleResourceItemCollection;
        }
        Refresh();
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceCountChanged -= HandleResourceCountChanged;
        LevelManager.OnLevelLoaded -= HandleLevelLoad;
        if (m_ListenCollectionEvents)
        {
            CollectibleResourceItem.OnResourceCollected -= HandleResourceItemCollection;
        }
    }
    private void HandleResourceItemCollection(CollectibleResourceItem item)
    {
        if (item.Resource != Resource) return;
        item.ResourceUIUpdater = this;
        item.GetCollected();
    }
    private void Refresh()
    {
        m_ReseourceCountText.text = ResourceManager.GetResourceCount(m_Resource).ToString();
    }
    private void HandleLevelLoad()
    {
        Refresh();
    }

    private void HandleResourceCountChanged(Resource i_Resource)
    {
        if(i_Resource==m_Resource)
        {
            m_ReseourceCountText.text = ResourceManager.GetResourceCount(m_Resource).ToString();
        }
    }

    public void MoveResourceToCollector(CollectibleResourceItem _resourceItem)
    {
        _resourceItem.GotoCollector(m_Icon, () =>
        {
            m_IconVisual.DOComplete();
            m_IconVisual.DOPunchScale(m_Icon.transform.localScale * 1.1f, 0.2f);

            ResourceManager.UpdateResourceCount(_resourceItem.Resource, _resourceItem.ResouceCount);
            Refresh();

        });
    }
}
