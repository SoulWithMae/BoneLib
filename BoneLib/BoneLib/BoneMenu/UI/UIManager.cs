﻿using UnityEngine;
using UnityEngine.UI;

using BoneLib.BoneMenu.Elements;

namespace BoneLib.BoneMenu.UI
{
    /// <summary>
    /// The UI manager's purpose is to tie the internal menu together with the UI.
    /// It can show/hide pages, go back to pages, and add/remove elements.
    /// </summary>
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class UIManager : MonoBehaviour
    {
        public UIManager(System.IntPtr ptr) : base(ptr) { }

        public static UIManager Instance { get; private set; }

        public UIPool PagePool { get; private set; }
        public UIPool CategoryPool { get; private set; }
        public UIPool FunctionPool { get; private set; }
        public UIPool ValuePool { get; private set; }
        public UIPool TogglePool { get; private set; }

        public UIPage MainPage;

        private GameObject pagePool;
        private GameObject categoryPool;
        private GameObject functionPool;
        private GameObject valuePool;
        private GameObject togglePool;

        private void Awake()
        {
            Instance = this;

            gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();

            SetupPools();

            MainPage = PagePool.Spawn(DataManager.UI.PanelView.transform, false).GetComponent<UIPage>();
            DataManager.UI.Init();
        }

        private void Update()
        {
            MainPage.transform.position = DataManager.UI.PanelView.transform.position;
            MainPage.transform.rotation = DataManager.UI.PanelView.transform.rotation;
        }

        private void OnEnable()
        {
            MenuManager.OnCategorySelected += OnCategoryUpdated;
            MenuCategory.OnElementCreated += OnElementAdded;
        }

        private void OnDisable()
        {
            MenuManager.OnCategorySelected -= OnCategoryUpdated;
            MenuCategory.OnElementCreated -= OnElementAdded;
        }

        public void OnCategoryUpdated(MenuCategory category)
        {
            if (category == null)
            {
                return;
            }

            MainPage.AssignElement(category);
            MainPage.Draw();
            MainPage.gameObject.SetActive(true);
        }

        public void OnElementAdded(MenuCategory category, MenuElement element)
        {
            OnCategoryUpdated(category);
        }

        public void OnElementRemoved(MenuCategory category, MenuElement element)
        {
            OnCategoryUpdated(category);
        }

        private void SetupPools()
        {
            pagePool = new GameObject("Page Pool");
            categoryPool = new GameObject("Category Pool");
            functionPool = new GameObject("Function Pool");
            valuePool = new GameObject("Value Pool");
            togglePool = new GameObject("Toggle Pool");

            pagePool.transform.SetParent(gameObject.transform);
            categoryPool.transform.SetParent(gameObject.transform);
            functionPool.transform.SetParent(gameObject.transform);
            valuePool.transform.SetParent(gameObject.transform);
            togglePool.transform.SetParent(gameObject.transform);

            PagePool = pagePool?.AddComponent<UIPool>();
            CategoryPool = categoryPool?.AddComponent<UIPool>();
            FunctionPool = functionPool?.AddComponent<UIPool>();
            ValuePool = valuePool?.AddComponent<UIPool>();
            TogglePool = togglePool?.AddComponent<UIPool>();

            PagePool.SetCount(2);
            CategoryPool.SetCount(6);
            FunctionPool.SetCount(6);
            ValuePool.SetCount(6);
            TogglePool.SetCount(6);

            PagePool.SetPrefab(DataManager.UI.PagePrefab);
            CategoryPool.SetPrefab(DataManager.UI.CategoryPrefab);
            FunctionPool.SetPrefab(DataManager.UI.FunctionPrefab);
            ValuePool.SetPrefab(DataManager.UI.ValuePrefab);
            TogglePool.SetPrefab(DataManager.UI.TogglePrefab);

            PagePool.Populate(PagePool.Count);
            CategoryPool.Populate(CategoryPool.Count);
            FunctionPool.Populate(FunctionPool.Count);
            ValuePool.Populate(ValuePool.Count);
            TogglePool.Populate(ValuePool.Count);
        }
    }
}
