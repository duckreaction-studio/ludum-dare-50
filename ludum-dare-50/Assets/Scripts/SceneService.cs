using DG.Tweening;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneService : MonoBehaviour
{
    [SerializeField] Camera _overlayCamera;

    List<string> _scenesToUnload;
    List<string> _scenesToLoad;

    public event EventHandler<string> SceneLoad;

    public void StartSceneTransition(string[] scenesToUnload, string[] scenesToLoad)
    {
        _scenesToUnload = new(scenesToUnload);
        _scenesToLoad = new(scenesToLoad);

        ProcessNextScene();
    }

    void OnSceneLoadedOrUnloaded(AsyncOperation op)
    {
        ProcessNextScene();
    }

    void ProcessNextScene()
    {
        if (_scenesToUnload.Count == 0 && _scenesToLoad.Count == 0)
        {
            TransitionEndAnimation();
        }
        else if (_scenesToUnload.Count == 0)
        {
            LoadNextScene();
        }
        else
        {
            UnloadNextScene();
        }
    }

    void UnloadNextScene()
    {
        var scene = _scenesToUnload.Shift();
        SceneManager.UnloadSceneAsync(scene).completed += OnSceneLoadedOrUnloaded;
    }

    void LoadNextScene()
    {
        var scene = _scenesToLoad.Shift();
        SceneLoad?.Invoke(this, scene);
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += OnSceneLoadedOrUnloaded;
    }

    [ContextMenu("Test transition start animation")]
    void TransitionStartAnimation()
    {
        _overlayCamera.gameObject.SetActive(true);
    }

    [ContextMenu("Test transition end animation")]
    void TransitionEndAnimation()
    {
        _overlayCamera.gameObject.SetActive(false);
    }
}