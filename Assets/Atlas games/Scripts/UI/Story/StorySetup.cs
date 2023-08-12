using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySetup : MonoBehaviour
{
    public Story[] stories;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public Story SceneStory()
    {
        foreach (var story in stories)
        {
            if (story.level == GlobalValue.levelPlaying)
            {
                return story;
            }
        }

        return null;
    }
}
