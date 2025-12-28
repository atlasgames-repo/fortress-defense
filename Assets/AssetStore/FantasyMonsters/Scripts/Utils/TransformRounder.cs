using System;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts.Utils
{
    public class TransformRounder : MonoBehaviour
    {
        public void OnValidate()
        {
            foreach (var t in GetComponentsInChildren<Transform>(true))
            {
                t.localPosition = new Vector3((float) Math.Round(t.localPosition.x, 2), (float) Math.Round(t.localPosition.y, 2), (float) Math.Round(t.localPosition.z, 2));
            }
        }
    }
}