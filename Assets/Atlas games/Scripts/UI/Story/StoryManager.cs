using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    private Canvas _canvas;
    private Story _story;
    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        _story = FindObjectOfType<StorySetup>().SceneStory();
        GameObject newStory = Instantiate(_story.gameObject, _canvas.transform.position, Quaternion.identity,
            _canvas.transform);
        newStory.transform.SetSiblingIndex(_canvas.transform.childCount -1);
    }
}
