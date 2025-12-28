using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts
{
    /// <summary>
    /// The main script to control monsters.
    /// </summary>
    public class WarMachine : Monster
    {
        public List<SpriteRenderer> Wheels;
        public List<Sprite> WheelSprites;

        public new void Awake()
        {
            base.Awake();

            var stateHandler = Animator.GetBehaviours<StateHandler>().SingleOrDefault(i => i.Name == "Death");

            if (stateHandler)
            {
                stateHandler.StateExit.AddListener(() => SetWheels(0));
            }
        }

        /// <summary>
        /// Called from animation.
        /// </summary>
        public void SetWheels(int index)
        {
            if (index != 1 && Animator.GetInteger("State") == (int) MonsterState.Death) return;

            if (index < WheelSprites.Count)
            {
                Wheels.ForEach(i => i.sprite = WheelSprites[index]);
            }
        }
    }
}