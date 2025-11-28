using System;
using UnityEngine;

[CreateAssetMenu(fileName = "tutorials", menuName = "Scriptable Objects/Tutorials")]
public class tutorials : ScriptableObject
{
    public enum Model
    {
        InGame,
        InMenu,
        Endless
    }
    public enum MenuPart
    {
        None,
        Home,
        Map,
        Trophy,
        Events,
        Store,
        Leaderboard,
    }

    [Serializable]
    public class Tutorialinfo
    {
        public GameObject TutorialPrefab;
        public Model Model;
        public int LevelNumber;
        public int Delay;
        public MenuPart MenuPart;
    }

    //public GameObject[] tutorialPrefabs;
    public Tutorialinfo[] infoT;
}
