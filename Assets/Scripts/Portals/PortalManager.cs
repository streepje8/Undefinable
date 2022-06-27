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

    private void LateUpdate()
    {
        ReleaseAllTextures(); //Make all textures availible again at the end of each frame
    }

    /// <summary>
    /// Retrieves a texture from the texture pool
    /// </summary>
    /// <returns>an unused texture</returns>
    /// <exception cref="OverflowException"></exception>
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
        newPoolItem.Used = true;
        return newPoolItem;
    }
    
    /// <summary>
    /// Makes a texture availble again in the pool
    /// </summary>
    /// <param name="item">the texture to release</param>
    public void ReleaseTexture(PoolItem item)
    {
        item.Used = false;
    }

    /// <summary>
    /// Makes all textures availble in the pool again
    /// </summary>
    public void ReleaseAllTextures()
    {
        foreach (var poolItem in pool)
        {
            ReleaseTexture(poolItem);
        }
    }

    /// <summary>
    /// Creates a new rendertexture for the pool at screen resolution, this can be turned down to save performance if we need to
    /// </summary>
    /// <returns>The created texture</returns>
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
    
    /// <summary>
    /// Deletes a created texture from VRAM (and also from existance)
    /// </summary>
    /// <param name="item">The texture to delete</param>
    private void DestroyTexture(PoolItem item)
    {
        item.Texture.Release(); //Remove it form the VRAM
        Destroy(item.Texture); //Remove the pointer from the RAM
    }

    /// <summary>
    /// When the scene ends or the game ends all textures get cleaned up.
    /// </summary>
    private void OnDestroy()
    {
        foreach (var poolItem in pool)
        {
            DestroyTexture(poolItem);
        }
    }
}
