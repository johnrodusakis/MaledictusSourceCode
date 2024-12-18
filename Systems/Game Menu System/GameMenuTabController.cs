using Maledictus.Events;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Maledictus.GameMenu
{
    [System.Serializable]
    public enum GameMenuTab
    {
        Character,
        Techniques,
        SkillTree,
        Bestiary,
        Map,
        Quests,
        Items,
        HuntersGuidebook
    }

    public class GameMenuTabController : MonoBehaviour
    {
        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventGameMenuTab _onGameMenuTabSelected;

        private int _selectedTab;

        private List<GameMenuTabUI> _gameMenuTabs = new();

        private void Start()
        {
            SelectTab(GameMenuTab.Map);
            ResizeTabs();
        }

        private void OnValidate()
        {
            InitializeGameMenuTabs();
        }

        private void OnEnable()
        {
            _onGameMenuTabSelected.OnRaised += SelectTab;

            InputManager.OnInventoryToggled += OpenCharacterGameMenuTab;
        }

        private void OnDisable()
        {
            _onGameMenuTabSelected.OnRaised -= SelectTab;

            InputManager.OnInventoryToggled -= OpenCharacterGameMenuTab;
        }

        private void OpenCharacterGameMenuTab()
        {
            SelectTab(GameMenuTab.Character);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
                SelectPreviousTab();

            if(Input.GetKeyDown(KeyCode.E))
                SelectNextTab();
        }

        private void InitializeGameMenuTabs()
        {
            _gameMenuTabs.Clear();

            foreach (Transform child in this.transform)
                _gameMenuTabs.Add(child.GetComponent<GameMenuTabUI>());
        }

        [Button]
        public void SelectNextTab()
        {
            DeselectAllTabs();

            if (_selectedTab == (int)GameMenuTab.HuntersGuidebook)
                _selectedTab = (int)GameMenuTab.Character;
            else
                _selectedTab++;

            _gameMenuTabs[_selectedTab].SelectTab();
        }

        public void SelectTab(GameMenuTab gameMenuTab)
        {
            DeselectAllTabs();
            _selectedTab = (int) gameMenuTab;
            _gameMenuTabs[_selectedTab].SelectTab();
        }

        [Button]
        public void SelectPreviousTab()
        {
            DeselectAllTabs();

            if (_selectedTab == (int)GameMenuTab.Character)
                _selectedTab = (int)GameMenuTab.HuntersGuidebook;
            else
                _selectedTab--;

            _gameMenuTabs[_selectedTab].SelectTab();
        }

        private void DeselectAllTabs()
        {
            foreach (var tab in _gameMenuTabs)
                tab.DeselectTab();
        }

        [Button]
        private void ResizeTabs()
        {
            foreach (var tab in _gameMenuTabs)
                tab.ResizeTab();
        }
    }
}