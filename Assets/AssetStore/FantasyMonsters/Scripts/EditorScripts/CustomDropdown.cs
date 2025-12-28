using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.FantasyMonsters.Scripts.EditorScripts
{
    public class CustomDropdown : Dropdown
    {
        public new void Start()
        {
            base.Start();

            var onPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            var onPointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };

            onPointerUp.callback.AddListener(eventData => StartCoroutine(OnDropdownExpanded()));
            onPointerExit.callback.AddListener(eventData => { if (GetComponentInChildren<ScrollRect>() != null) onValueChanged.Invoke(value); });
            gameObject.AddComponent<EventTrigger>().triggers = new List<EventTrigger.Entry> { onPointerUp, onPointerExit };
        }

        private IEnumerator OnDropdownExpanded()
        {
            yield return null;

            var toggles = GetComponentsInChildren<Toggle>().ToList();

            for (var i = 0; i < toggles.Count; i++)
            {
                var index = i;
                var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                var pointerClick = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                var scroll = new EventTrigger.Entry { eventID = EventTriggerType.Scroll };
                var scrollRect = GetComponentInChildren<ScrollRect>();

                pointerEnter.callback.AddListener(eventData => onValueChanged.Invoke(index));
                pointerClick.callback.AddListener(eventData => toggles.ForEach(j => Destroy(j.GetComponent<EventTrigger>())));
                scroll.callback.AddListener(eventData => scrollRect.OnScroll(eventData as PointerEventData));
                toggles[i].gameObject.AddComponent<EventTrigger>().triggers = new List<EventTrigger.Entry> { pointerEnter, pointerClick, scroll };
            }

            if (options.Count > 1)
            {
                GetComponentInChildren<ScrollRect>().verticalScrollbar.value = 1 - (float) value / (options.Count - 1);
            }
        }
    }
}