using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem
{
    public RenderTexture Texture;
    public bool Used;
}

public class PortalManager : Singleton<PortalManager>
{
    public PortalCamera mainCamera;
    public PortalCamera portalRenderCamera;
    public int recursionCount = 5;

    public int maxTexturePoolSize = 100;
    private List<PoolItem> pool = new List<PoolItem>();

    [HideInInspector]public List<Teleportable> teleportables = new List<Teleportable>();
    private void Awake()
    {
        Instance = this;
    }

    public PoolItem GetTexture()
    {
        foreach (var poolItem in pool)
        {
            if (!poolItem.Used)
            {
                poolItem.Used = true;
                return poolItem;
            }
        }
        
        if (pool.Count >= maxTexturePoolSize)
        {
            Debug.LogError("Pool is full!");
            throw new OverflowException();
        }
        var newPoolItem = CreateTexture();
        pool.Add(newPoolItem);
        //Debug.Log($"New RenderTexture created, pool is now {pool.Count} items big.");
        newPoolItem.Used = true;
        return newPoolItem;
    }
    public void ReleaseTexture(PoolItem item)
    {
        item.Used = false;
    }

    public void ReleaseAllTextures()
    {
        foreach (var poolItem in pool)
        {
            ReleaseTexture(poolItem);
        }
    }

    private PoolItem CreateTexture()
    {
        var newTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
        newTexture.Create();

        return new PoolItem
        {
            Texture = newTexture,
            Used = false
        };
    }
    private void DestroyTexture(PoolItem item)
    {
        item.Texture.Release();
        Destroy(item.Texture);
    }

    private void OnDestroy()
    {
        foreach (var poolItem in pool)
        {
            DestroyTexture(poolItem);
        }
    }
}
