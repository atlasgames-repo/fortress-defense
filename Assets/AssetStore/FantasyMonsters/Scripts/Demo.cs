using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyMonsters.Scripts
{
    /// <summary>
    /// Demo scene that can run animations.
    /// </summary>
    public class Demo : MonoBehaviour
    {
        public UnityEngine.Object MonstersFolder;
        public List<Monster> Monsters = new List<Monster>();
        public GameObject SelectedMonster;
        public Dropdown MonstersDropdown;

        private static List<Monster> ActiveMonsters => FindObjectsOfType<Monster>().ToList();

        #if UNITY_EDITOR

        public void OnValidate()
        {
            if (Application.isPlaying || MonstersFolder == null) return;

            Monsters = Directory.GetFiles(UnityEditor.AssetDatabase.GetAssetPath(MonstersFolder), "*.prefab", SearchOption.AllDirectories).Select(UnityEditor.AssetDatabase.LoadAssetAtPath<Monster>).Where(i => i != null).OrderBy(i => i.name).ToList();
            MonstersDropdown.options = Monsters.Select(i => new Dropdown.OptionData(Regex.Replace(i.name, "([a-z])([A-Z])", "$1 $2"))).ToList();
        }

        #endif

        public void Start()
        {
            ActiveMonsters.ForEach(i => i.SetState(MonsterState.Ready));
        }

        public void LoadMonster(int index)
        {
            Destroy(SelectedMonster);
            SelectedMonster = Instantiate(Monsters[index]).gameObject;
            SelectedMonster.name = Monsters[index].name;
            SelectedMonster.transform.position = new Vector3(0, -1.5f);
        }

        public void PlayAnimation(string clipName)
        {
            ActiveMonsters.ForEach(i => i.SetState((MonsterState) Enum.Parse(typeof(MonsterState), clipName)));
        }

        public void Attack()
        {
            ActiveMonsters.ForEach(i => i.Attack());
        }

        public void SetTrigger(string trigger)
        {
            ActiveMonsters.ForEach(i => i.Animator.SetTrigger(trigger));
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}