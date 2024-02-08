using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject _titleBarPrefab;
    
    private GameObject _titleBar;

    void Start()
    {
        GameObject _titleBar = Instantiate(_titleBarPrefab);
        
        _titleBar.name = _titleBarPrefab.name;
    }

    void Update()
    {
        
    }
}
