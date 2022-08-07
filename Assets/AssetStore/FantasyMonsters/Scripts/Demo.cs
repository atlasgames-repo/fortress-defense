using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.FantasyMonsters.Scripts
{
    /// <summary>
    /// Demo scene that can run animations.
    /// </summary>
    public class Demo : MonoBehaviour
    {
        private List<Monster> Monster => FindObjectsOfType<Monster>().ToList();

        public void Start()
        {
            Monster.ForEach(i => i.SetState(MonsterState.Ready));
        }

        public void PlayAnimation(string clipName)
        {
            Monster.ForEach(i => i.SetState((MonsterState) Enum.Parse(typeof(MonsterState), clipName)));
        }

        public void Attack()
        {
            Monster.ForEach(i => i.Attack());
        }

        public void SetTrigger(string trigger)
        {
            Monster.ForEach(i => i.Animator.SetTrigger(trigger));
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}