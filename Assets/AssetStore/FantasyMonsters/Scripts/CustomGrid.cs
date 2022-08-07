using UnityEngine;

namespace Assets.FantasyMonsters.Scripts
{
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class CustomGrid : MonoBehaviour
    {
        public Vector2 CellSize = new Vector2(1, 1);
        public int Columns = 1;
        public bool Chess;

        public void OnValidate()
        {
            Rebuild();
        }

        protected virtual void OnTransformChildrenChanged()
        {
            Rebuild();
        }

        private void Rebuild()
        {
            var rows = Mathf.CeilToInt((float)transform.childCount / Columns);

            for (var i = 0; i < transform.childCount; i++)
            {
                var x = i % Columns;
                var y = i / Columns;

                transform.GetChild(i).localPosition = new Vector3((x - (Columns - 1) / 2f) * CellSize.x, ((rows - 1) / 2f - y) * CellSize.y);

                if (Chess && y % 2 == 1) transform.GetChild(i).localPosition += new Vector3(CellSize.x / 2, 0);
            }
        }
    }
}