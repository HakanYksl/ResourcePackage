using System;
using System.Collections.Generic;
using UnityEngine;

namespace Essentials.Managers
{
    /// <summary>
    /// Everything is a resource including buildings, coins, skills, items etc
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        [SerializeField] private List<Resource> m_Resources;
        [SerializeField] private Resource m_Coin;
        public Resource Coin => m_Coin;
        public int CoinCount
        {
            get => GetResourceCount(Coin);
            set => UpdateResourceCount(Coin, value);
        }

        #region Events
        public delegate void ResourceCountChangedEvent(Resource i_Resource);
        public static event ResourceCountChangedEvent OnResourceCountChanged;
        private void RaiseResourceCountChanged(Resource i_Resource) { OnResourceCountChanged?.Invoke(i_Resource); }
        #endregion

        private PlayerDataManager m_PlayerData=>PlayerDataManager.Instance;

        public void SetResourceCount(Resource i_Resource,int i_Amount)
        {
            m_PlayerData.SetResourceCount(i_Resource, i_Amount);
        }
        public void SetResourceCount(string i_Resource, int i_Amount)
        {
            m_PlayerData.SetResourceCount(i_Resource, i_Amount);
        }

        public int GetResourceCount(Resource i_Resource)
        {
            return m_PlayerData.GetResourceCount(i_Resource);
        }

        public void UpdateResourceCount(Resource i_Resource, int i_Amount)
        {
            m_PlayerData.UpdateResourceCount(i_Resource, i_Amount);
            RaiseResourceCountChanged(i_Resource);
        }
    }
}
