using System;
using System.Collections.Generic;
using UnityEngine;
using Maledictus.CustomSoap;

namespace Maledictus.GameMenu
{
    using Maledictus.Codex;
    using Maledictus.StateMachine;
    using SomniaGames.Persistence;

    [System.Serializable]
    public enum GameMenuTab
    {
        Character,
        Techniques,
        SkillTree,
        Map,
        Quests,
        Items,
        HuntersCodeX
    }

    public partial class GameMenuController : MonoBehaviour
    {
        [SerializeField] private GameMenuTab _initialTab;
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
            HideAllGameMenuTabs();

            _stateMachine = new StateMachine();

            var characterTab = new CharacterTab(_gameMenuBodyTabs[0].GetComponent<CharacterTabUI>(), _gameMenuHeaderTabs[0]);
            var techniquesTab = new TechniquesTab(_gameMenuBodyTabs[1], _gameMenuHeaderTabs[1]);
            var skillTreeTab = new SkillTreeTab(_gameMenuBodyTabs[2], _gameMenuHeaderTabs[2]);
            var mapTab = new MapTab(_gameMenuBodyTabs[3], _gameMenuHeaderTabs[3]);
            var questTab = new QuestTab(_gameMenuBodyTabs[4], _gameMenuHeaderTabs[4]);
            var itemsTab = new ItemsTab(_gameMenuBodyTabs[5], _gameMenuHeaderTabs[5]);
            var huntersCodexTab = new HuntersCodexTab(_gameMenuBodyTabs[6].GetComponent<CodexController>(), _gameMenuHeaderTabs[6]);

            HandleTransitions(characterTab, techniquesTab, skillTreeTab, mapTab, questTab, itemsTab, huntersCodexTab);

            _stateMachine.SetState(characterTab);

            SelectTab(_initialTab);
        }

        private void HandleTransitions(CharacterTab characterTab, TechniquesTab techniquesTab, SkillTreeTab skillTreeTab, MapTab mapTab, QuestTab questTab, ItemsTab itemsTab, HuntersCodexTab huntersCodexTab)
        {
            //void AddStateTransition(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            void AddAnyStateTransition(IState to, Func<bool> condition) => _stateMachine.AddTransition(to, condition);

            #region Transitions


            #endregion

            #region Any Transitions

            AddAnyStateTransition(characterTab,     CharacterTab());
            AddAnyStateTransition(techniquesTab,    TechniquesTab());
            AddAnyStateTransition(skillTreeTab,     SkillTreeTab());
            AddAnyStateTransition(mapTab,           MapTab());
            AddAnyStateTransition(questTab,         QuestsTab());
            AddAnyStateTransition(itemsTab,         ItemsTab());
            AddAnyStateTransition(huntersCodexTab,  HuntersCodexTab());

            #endregion

            #region Conditions

            Func<bool> CharacterTab()       => () => _selectedTab == (int)GameMenuTab.Character;
            Func<bool> TechniquesTab()      => () => _selectedTab == (int)GameMenuTab.Techniques;
            Func<bool> SkillTreeTab()       => () => _selectedTab == (int)GameMenuTab.SkillTree;
            Func<bool> MapTab()             => () => _selectedTab == (int)GameMenuTab.Map;
            Func<bool> QuestsTab()          => () => _selectedTab == (int)GameMenuTab.Quests;
            Func<bool> ItemsTab()           => () => _selectedTab == (int)GameMenuTab.Items;
            Func<bool> HuntersCodexTab()    => () => _selectedTab == (int)GameMenuTab.HuntersCodeX;

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

            if (_selectedTab == (int)GameMenuTab.HuntersCodeX)
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
                _selectedTab = (int)GameMenuTab.HuntersCodeX;
            else
                _selectedTab--;

            _gameMenuHeaderTabs[_selectedTab].SelectTab();
        }

        private void DeselectAllTabs()
        {
            foreach (var tab in _gameMenuHeaderTabs)
                tab.DeselectTab();
        }

        private void HideAllGameMenuTabs()
        {
            foreach (var go in _gameMenuBodyTabs)
            {
                var canvasGroup = go.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        internal class TechniquesTab : BaseGameMenuState
        {
            private readonly GameObject _techniquesGO;

            protected override CanvasGroup CanvasGroup => _techniquesGO.GetComponent<CanvasGroup>();

            public TechniquesTab(GameObject go, GameMenuTabUI menuTabUI)
            {
                _techniquesGO = go;
                _gameMenuTabUI = menuTabUI;
            }

            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();
            }
        }

        internal class SkillTreeTab : BaseGameMenuState
        {
            private readonly GameObject _skillTreeGO;

            protected override CanvasGroup CanvasGroup => _skillTreeGO.GetComponent<CanvasGroup>();

            public SkillTreeTab(GameObject go, GameMenuTabUI menuTabUI)
            {
                _skillTreeGO = go;
                _gameMenuTabUI = menuTabUI;
            }

            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();
            }
        }

        internal class MapTab : BaseGameMenuState
        {
            private readonly GameObject _mapGO;

            protected override CanvasGroup CanvasGroup => _mapGO.GetComponent<CanvasGroup>();

            public MapTab(GameObject go, GameMenuTabUI menuTabUI)
            {
                _mapGO = go;
                _gameMenuTabUI = menuTabUI;
            }

            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();
            }
        }

        internal class QuestTab : BaseGameMenuState
        {
            private readonly GameObject _questGO;

            protected override CanvasGroup CanvasGroup => _questGO.GetComponent<CanvasGroup>();

            public QuestTab(GameObject go, GameMenuTabUI menuTabUI)
            {
                _questGO = go;
                _gameMenuTabUI = menuTabUI;
            }

            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();
            }
        }

        internal class ItemsTab : BaseGameMenuState
        {
            private readonly GameObject _itemsGO;

            protected override CanvasGroup CanvasGroup => _itemsGO.GetComponent<CanvasGroup>();

            public ItemsTab(GameObject go, GameMenuTabUI menuTabUI)
            {
                _itemsGO = go;
                _gameMenuTabUI = menuTabUI;
            }

            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();
            }
        }

        internal class HuntersCodexTab : BaseGameMenuState
        {
            private readonly CodexController _codexController;

            protected override CanvasGroup CanvasGroup => _codexController.GetComponent<CanvasGroup>();

            public HuntersCodexTab(CodexController controller, GameMenuTabUI menuTabUI)
            {
                _codexController = controller;
                _gameMenuTabUI = menuTabUI;

                _codexController.OnNewNotification += HandleNotificationPopUp;
            }

            protected override void RegisterEvents()
            {
                _codexController.OnNewNotification += HandleNotificationPopUp;
            }

            protected override void UnregisterEvents()
            {
                _codexController.OnNewNotification -= HandleNotificationPopUp;
            }

            public override void OnEnter()
            {
                base.OnEnter();

                _codexController.OnEnter();
            }

            public override void OnExit()
            {
                base.OnExit();

                _codexController.OnExit();
            }
        }
    }
}