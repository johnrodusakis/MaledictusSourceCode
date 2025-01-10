using System;
using System.Collections.Generic;
using UnityEngine;
using Maledictus.Events;

namespace Maledictus.GameMenu
{
    using Maledictus.StateMachine;

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

    public partial class GameMenuController : MonoBehaviour
    {
        [SerializeField] private List<GameMenuTabUI> _gameMenuHeaderTabs = new();
        [SerializeField] private List<GameObject> _gameMenuBodyTabs = new();

        [Space(15f)]
        [Header("Events")]
        [SerializeField] private ScriptableEventGameMenuTab _onGameMenuTabSelected;

        private int _selectedTab;

        private StateMachine _stateMachine;
        
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

        private void Start()
        {
            _stateMachine = new StateMachine();

            var characterTab = new CharacterTab(_gameMenuBodyTabs[0]);
            var techniquesTab = new TechniquesTab(_gameMenuBodyTabs[1]);
            var skillTreeTab = new SkillTreeTab(_gameMenuBodyTabs[2]);
            var bestiaryTab = new BestiaryTab(_gameMenuBodyTabs[3]);
            var mapTab = new MapTab(_gameMenuBodyTabs[4]);
            var questTab = new QuestTab(_gameMenuBodyTabs[5]);
            var itemsTab = new ItemsTab(_gameMenuBodyTabs[6]);
            var huntersGuidbookTab = new HuntersGuidbookTab(_gameMenuBodyTabs[7]);

            HandleTransitions(characterTab, techniquesTab, skillTreeTab, bestiaryTab, mapTab, questTab, itemsTab, huntersGuidbookTab);

            _stateMachine.SetState(characterTab);
            SelectTab(GameMenuTab.Character);
        }

        private void HandleTransitions(CharacterTab characterTab, TechniquesTab techniquesTab, SkillTreeTab skillTreeTab, BestiaryTab bestiaryTab, MapTab mapTab, QuestTab questTab, ItemsTab itemsTab, HuntersGuidbookTab huntersGuidbookTab)
        {
            void AddStateTransition(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            void AddAnyStateTransition(IState to, Func<bool> condition) => _stateMachine.AddTransition(to, condition);

            #region Transitions


            #endregion

            #region Any Transitions

            AddAnyStateTransition(characterTab, CharacterTab());
            AddAnyStateTransition(techniquesTab, TechniquesTab());
            AddAnyStateTransition(skillTreeTab, SkillTreeTab());
            AddAnyStateTransition(bestiaryTab, BestiaryTab());
            AddAnyStateTransition(mapTab, MapTab());
            AddAnyStateTransition(questTab, QuestsTab());
            AddAnyStateTransition(itemsTab, ItemsTab());
            AddAnyStateTransition(huntersGuidbookTab, HuntersGuidebookTab());

            #endregion

            #region Conditions

            Func<bool> CharacterTab()           => () => _selectedTab == (int)GameMenuTab.Character;
            Func<bool> TechniquesTab()          => () => _selectedTab == (int)GameMenuTab.Techniques;
            Func<bool> SkillTreeTab()           => () => _selectedTab == (int)GameMenuTab.SkillTree;
            Func<bool> BestiaryTab()            => () => _selectedTab == (int)GameMenuTab.Bestiary;
            Func<bool> MapTab()                 => () => _selectedTab == (int)GameMenuTab.Map;
            Func<bool> QuestsTab()              => () => _selectedTab == (int)GameMenuTab.Quests;
            Func<bool> ItemsTab()               => () => _selectedTab == (int)GameMenuTab.Items;
            Func<bool> HuntersGuidebookTab()    => () => _selectedTab == (int)GameMenuTab.HuntersGuidebook;

            #endregion
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SelectPreviousTab();

            if (Input.GetKeyDown(KeyCode.E))
                SelectNextTab();

            _stateMachine.Tick();
        }

        private void OpenCharacterGameMenuTab() => SelectTab(GameMenuTab.Character);

        private void SelectNextTab()
        {
            DeselectAllTabs();

            if (_selectedTab == (int)GameMenuTab.HuntersGuidebook)
                _selectedTab = (int)GameMenuTab.Character;
            else
                _selectedTab++;

            _gameMenuHeaderTabs[_selectedTab].SelectTab();
        }

        private void SelectTab(GameMenuTab gameMenuTab)
        {
            DeselectAllTabs();
            _selectedTab = (int)gameMenuTab;
            _gameMenuHeaderTabs[_selectedTab].SelectTab();
        }

        private void SelectPreviousTab()
        {
            DeselectAllTabs();

            if (_selectedTab == (int)GameMenuTab.Character)
                _selectedTab = (int)GameMenuTab.HuntersGuidebook;
            else
                _selectedTab--;

            _gameMenuHeaderTabs[_selectedTab].SelectTab();
        }

        private void DeselectAllTabs()
        {
            foreach (var tab in _gameMenuHeaderTabs)
                tab.DeselectTab();
        }

        internal class TechniquesTab : BaseState
        {
            private GameObject _techniquesGO;

            public TechniquesTab(GameObject go)
            {
                _techniquesGO = go;
            }

            public override void OnEnter()
            {
                _techniquesGO.SetActive(true);
            }

            public override void OnExit()
            {
                _techniquesGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }

        internal class SkillTreeTab : BaseState
        {
            private GameObject _skillTreeGO;

            public SkillTreeTab(GameObject go)
            {
                _skillTreeGO = go;
            }

            public override void OnEnter()
            {
                _skillTreeGO.SetActive(true);
            }

            public override void OnExit()
            {
                _skillTreeGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }

        internal class BestiaryTab : BaseState
        {
            private GameObject _bestiaryGO;

            public BestiaryTab(GameObject go)
            {
                _bestiaryGO = go;
            }

            public override void OnEnter()
            {
                _bestiaryGO.SetActive(true);
            }

            public override void OnExit()
            {
                _bestiaryGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }

        internal class MapTab : BaseState
        {
            private GameObject _mapGO;

            public MapTab(GameObject go)
            {
                _mapGO = go;
            }

            public override void OnEnter()
            {
                _mapGO.SetActive(true);
            }

            public override void OnExit()
            {
                _mapGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }

        internal class QuestTab : BaseState
        {
            private GameObject _questGO;

            public QuestTab(GameObject go)
            {
                _questGO = go;
            }

            public override void OnEnter()
            {
                _questGO.SetActive(true);
            }

            public override void OnExit()
            {
                _questGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }

        internal class ItemsTab : BaseState
        {
            private GameObject _itemsGO;

            public ItemsTab(GameObject go)
            {
                _itemsGO = go;
            }

            public override void OnEnter()
            {
                _itemsGO.SetActive(true);
            }

            public override void OnExit()
            {
                _itemsGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }

        internal class HuntersGuidbookTab : BaseState
        {
            private GameObject _huntersGuidbookGO;

            public HuntersGuidbookTab(GameObject go)
            {
                _huntersGuidbookGO = go;
            }

            public override void OnEnter()
            {
                _huntersGuidbookGO.SetActive(true);
            }

            public override void OnExit()
            {
                _huntersGuidbookGO.SetActive(false);
            }

            public override void Tick()
            {

            }
        }
    }
}