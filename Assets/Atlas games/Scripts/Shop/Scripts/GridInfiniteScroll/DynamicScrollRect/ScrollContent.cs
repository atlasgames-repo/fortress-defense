﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace DynamicScrollRect
{
    // TODO : handle if datum updated in runtime
    public class ScrollContent : MonoBehaviour
    {
        [SerializeField] private Vector2 _spacing = Vector2.zero;
        public Vector2 Spacing => _spacing;

        [Tooltip("it will fill content rect in main axis(horizontal or vertical) automatically. Simply ignores _fixedItemCount")]
        [SerializeField] private bool _fillContent = false;
        
        [Tooltip("if scroll is vertical it is item count in each row vice versa for horizontal")]
        [Min(1)][SerializeField] private int _fixedItemCount = 1;

        private DynamicScrollRect _dynamicScrollRect;
        public DynamicScrollRect DynamicScrollRect
        {
            get
            {
                if (_dynamicScrollRect == null)
                {
                    _dynamicScrollRect = GetComponent<DynamicScrollRect>();
                }

                return _dynamicScrollRect;
            }
        }

        private ScrollItem _referenceItem;

        private ScrollItem _ReferenceItem
        {
            get
            {
                if (_referenceItem == null)
                {
                    _referenceItem = GetComponentInChildren<ScrollItem>();

                    if (_referenceItem == null)
                    {
                        throw new Exception($"No Scroll Item found under scroll rect content." +
                                            $"You should create reference scroll item under DynamicScroll Content first.");
                    }
                }

                return _referenceItem;
            }
        }

        private List<ScrollItem> _activatedItems = new List<ScrollItem>();

        private List<ScrollItem> _deactivatedItems = new List<ScrollItem>();

        private List<ScrollItemData> _datum;
        public List<ScrollItemData> Datum => _datum;

        private float _itemWidth => _ReferenceItem.RectTransform.rect.width;
        public float ItemWidth => _itemWidth;
    
        private float _itemHeight => _ReferenceItem.RectTransform.rect.height;
        public float ItemHeight => _itemHeight;
        
        public Action<ScrollItem> OnItemActivated { get; set; }
        
        public Action<ScrollItem> OnItemDeactivated { get; set; }

        public void InitScrollContent(List<ScrollItemData> contentDatum)
        {
            _datum = contentDatum;
            
            if (DynamicScrollRect.vertical)
            {
                InitItemsVertical(contentDatum);
            }

            if (DynamicScrollRect.horizontal)
            {
                InitItemsHorizontal(contentDatum);
            }
        }

        public void ClearContent()
        {
            List<ScrollItem> activatedItems = new List<ScrollItem>(_activatedItems);

            foreach (ScrollItem item in activatedItems)
            {
                DeactivateItem(item);
            }
        }

        public bool CanAddNewItemIntoTail()
        {
            if (_activatedItems == null || _activatedItems.Count == 0)
            {
                return false;
            }
        
            return _activatedItems[_activatedItems.Count - 1].Index < _datum.Count - 1;
        }

        public bool CanAddNewItemIntoHead()
        {
            if (_activatedItems == null || _activatedItems.Count == 0)
            {
                return false;
            }
        
            return _activatedItems[0].Index - 1 >= 0;
        }

        public Vector2 GetFirstItemPos()
        {
            if (_activatedItems.Count == 0)
            {
                return Vector2.zero;
            }
            
            return _activatedItems[0].RectTransform.anchoredPosition;
        }

        public Vector2 GetLastItemPos()
        {
            if (_activatedItems.Count == 0)
            {
                return Vector2.zero;
            }
            
            return _activatedItems[_activatedItems.Count - 1].RectTransform.anchoredPosition;
        }

        public void AddIntoHead()
        {
            for (int i = 0; i < _fixedItemCount; i++)
            {
                AddItemToHead();
            }
        }

        public void AddIntoTail()
        {
            for (int i = 0; i < _fixedItemCount; i++)
            {
                AddItemToTail();
            }
        }

        public void DeleteFromHead()
        {
            if (DynamicScrollRect.vertical)
            {
                int firstRowIndex = (int) _activatedItems[0].GridIndex.y;

                DeleteRow(firstRowIndex);
            }

            if (DynamicScrollRect.horizontal)
            {
                int firstColIndex = (int) _activatedItems[0].GridIndex.x;

                DeleteColumn(firstColIndex);
            }
        }
        
        public void DeleteFromTail()
        {
            if (DynamicScrollRect.vertical)
            {
                int lastRowIndex = (int) _activatedItems[_activatedItems.Count - 1].GridIndex.y;

                DeleteRow(lastRowIndex);
            }
            
            if (DynamicScrollRect.horizontal)
            {
                int lastColIndex = (int) _activatedItems[_activatedItems.Count - 1].GridIndex.x;

                DeleteColumn(lastColIndex);
            }
        }
        
        public bool AtTheEndOfContent(ScrollItem item)
        {
            if (DynamicScrollRect.vertical)
            {
                return !CanAddNewItemIntoTail() && 
                       item.RectTransform.anchoredPosition.y == _activatedItems[_activatedItems.Count - 1].RectTransform.anchoredPosition.y;
            }

            if (DynamicScrollRect.horizontal)
            {
                return !CanAddNewItemIntoTail() &&
                       item.RectTransform.anchoredPosition.x == _activatedItems[_activatedItems.Count - 1].RectTransform.anchoredPosition.x;
            }

            return false;
        }

        private void Awake()
        {
            _ReferenceItem.gameObject.SetActive(false);
        }

        private void InitItemsVertical(List<ScrollItemData> contentDatum)
        {
            int itemCount = 0;

            Vector2Int initialGridSize = CalculateInitialGridSize();

            for (int col = 0; col < initialGridSize.y; col++)
            {
                for (int row = 0; row < initialGridSize.x; row++)
                {
                    if (itemCount == contentDatum.Count)
                    {
                        return;
                    }
                    
                    ActivateItem(itemCount);
                
                    itemCount++;
                }
            }
        }

        private void InitItemsHorizontal(List<ScrollItemData> contentDatum)
        {
            int itemCount = 0;

            Vector2Int initialGridSize = CalculateInitialGridSize();

            for (int col = 0; col < initialGridSize.y; col++)
            {
                for (int row = 0; row < initialGridSize.x; row++)
                {
                    if (itemCount == contentDatum.Count)
                    {
                        return;
                    }
                    
                    ActivateItem(itemCount);
                
                    itemCount++;
                }
            }
        }

        private Vector2Int CalculateInitialGridSize()
        {
            Vector2 contentSize = DynamicScrollRect.content.rect.size;

            if (DynamicScrollRect.vertical)
            {
                int verticalItemCount = 4 + (int) (contentSize.y / (ItemHeight + _spacing.y));
                
                if (_fillContent)
                {
                    int horizontalItemCount = (int) ((contentSize.x + _spacing.x) / (ItemWidth + _spacing.x));

                    _fixedItemCount = horizontalItemCount;
                }

                return new Vector2Int(_fixedItemCount, verticalItemCount);
            }

            if (DynamicScrollRect.horizontal)
            {
                int horizontalItemCount = 4 + (int) (contentSize.x / (ItemWidth + _spacing.x));

                if (_fillContent)
                {
                    int verticalItemCount = (int) ((contentSize.y + _spacing.y) / (ItemHeight + _spacing.y));

                    _fixedItemCount = verticalItemCount;
                }

                return new Vector2Int(horizontalItemCount, _fixedItemCount);
            }
            
            return Vector2Int.zero;
        }

        private void DeleteRow(int rowIndex)
        {
            List<ScrollItem> items = _activatedItems.FindAll(i => (int) i.GridIndex.y == rowIndex);

            foreach (ScrollItem item in items)
            {
                DeactivateItem(item);
            }
        }

        private void DeleteColumn(int colIndex)
        {
            List<ScrollItem> items = _activatedItems.FindAll(i => (int) i.GridIndex.x == colIndex);

            foreach (ScrollItem item in items)
            {
                DeactivateItem(item);
            }
        }

        private void AddItemToTail()
        {
            if (!CanAddNewItemIntoTail())
            {
                return;
            }
            
            int itemIndex = _activatedItems[_activatedItems.Count - 1].Index + 1;

            if (itemIndex == _datum.Count)
            {
                return;
            }

            ActivateItem(itemIndex);
        }

        private void AddItemToHead()
        {
            if (!CanAddNewItemIntoHead())
            {
                return;
            }
            
            int itemIndex = _activatedItems[0].Index - 1;

            if (itemIndex < 0)
            {
                return;
            }

            ActivateItem(itemIndex);
        }

        private ScrollItem ActivateItem(int itemIndex)
        {
            Vector2 gridPos = GetGridPosition(itemIndex);

            Vector2 anchoredPos = GetAnchoredPosition(gridPos);

            ScrollItem scrollItem = null;
        
            if (_deactivatedItems.Count == 0)
            {
                scrollItem = CreateNewScrollItem();
            }
            else
            {
                scrollItem = _deactivatedItems[0];

                _deactivatedItems.Remove(scrollItem);
            }
            
            scrollItem.gameObject.name = $"{gridPos.x}_{gridPos.y}";
        
            scrollItem.RectTransform.anchoredPosition = anchoredPos;
            
            scrollItem.InitItem(itemIndex, gridPos, _datum[itemIndex]);

            bool insertHead = (_activatedItems.Count == 0 ||
                               (_activatedItems.Count > 0 && _activatedItems[0].Index > itemIndex));

            if (insertHead)
            {
                _activatedItems.Insert(0, scrollItem);
            }
            else
            {
                _activatedItems.Add(scrollItem);
            }
        
            scrollItem.Activated();
        
            OnItemActivated?.Invoke(scrollItem);
            
            return scrollItem;
        }

        private void DeactivateItem(ScrollItem item)
        {
            _activatedItems.Remove(item);
        
            _deactivatedItems.Add(item);
        
            item.Deactivated();
            
            OnItemDeactivated?.Invoke(item);
        }

        private ScrollItem CreateNewScrollItem()
        {
            GameObject item = Instantiate(_ReferenceItem.gameObject, DynamicScrollRect.content);
        
            ScrollItem scrollItem = item.GetComponent<ScrollItem>();
            scrollItem.RectTransform.pivot = new Vector2(0, 1);

            return scrollItem;
        }

        private Vector2 GetGridPosition(int itemIndex)
        {
            int col = itemIndex / _fixedItemCount;
            int row = itemIndex - (col * _fixedItemCount);
            
            if (DynamicScrollRect.vertical)
            {
                return new Vector2(row, col);
            }

            if (DynamicScrollRect.horizontal)
            {
                return new Vector2(col, row);
            }

            return Vector2.zero;
        }
        
        private Vector2 GetAnchoredPosition(Vector2 gridPosition)
        {
            return new Vector2(
                (gridPosition.x * _itemWidth) + (gridPosition.x * _spacing.x),
                (-gridPosition.y * _itemHeight) - (gridPosition.y * _spacing.y));
        }
    }
}
