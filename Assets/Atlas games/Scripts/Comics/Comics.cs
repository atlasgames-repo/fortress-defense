using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Comics", menuName = "Scriptable Objects/Comics")]
public class Comics : ScriptableObject
{

    public enum Model
    {
        InGame,
        InMenu
    }
    public enum MenuPart
    {
        None,
        Home,
        Map,
        Trophy,
        Events,
        Store,
        Leaderboard
    }

    [Serializable]
    public class Comicsinfo
    {
        public GameObject ComicPrefab;
        public Model Model;
        public int LevelNumber;
        public int Delay;
        public MenuPart MenuPart;
    }

    //public GameObject[] tutorialPrefabs;
    public Comicsinfo[] infoT;
}
