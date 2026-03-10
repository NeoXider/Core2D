//=== By Neoxider ===
using UnityEngine;
using Neo;
using Neo.Tools;

/// <summary>
/// Singleton implementation of GameManager 
/// Use Singleton.I to access the instance
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Singleton Implementation
    
    // Custom setup when the Singleton is first created
    protected override void OnInstanceCreated()
    {
    }

    // Initialization logic here (Awake)
    protected override void Init()
    {
        base.Init();
        
    }
    
    #endregion

    private void Start()
    {

    }

    private void Update()
    {

    }
}