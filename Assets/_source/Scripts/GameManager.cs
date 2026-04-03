//=== By Neoxider ===
using UnityEngine;
using Neo;
using Neo.Pages;
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
        G.OnStart.AddListener(OnStart);
        G.OnRestart.AddListener(OnRestart);
        G.OnLose.AddListener(OnLose);
        G.OnWin.AddListener(OnWin);
        G.OnEnd.AddListener(OnEnd);
        G.OnMenu.AddListener(OnMenu);
    }

    private void OnStart()
    {
        
    }

    private void OnRestart()
    {
        OnStart();
    }

    private void OnLose()
    {
        
    }

    private void OnWin()
    {
        
    }

    private void OnEnd()
    {
        
    }

    private void OnMenu()
    {
        
    }

    #endregion

    private void Start()
    {

    }

    private void Update()
    {

    }
}