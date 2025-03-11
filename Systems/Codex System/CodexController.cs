using DG.Tweening;
using Maledictus.CustomUI;
using Maledictus.Enemy;
using SomniaGames.Persistence;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace Maledictus.Codex
{
    public class CodexController : MonoBehaviour
    {
        public static event System.Action<List<CodexCategoryData>> OnCodexUpdate;
        public System.Action<bool> OnNewNotification { get; set; }

        private const float X_OFFSET = 650f;

        [SerializeField] private Transform _categoryListParent;

        [Space(15f)]
        [SerializeField] private CodexCategoryUI _categoryUI;
        [SerializeField] private Transform _enemyListUI;
        [SerializeField] private CodexEnemyUI _enemyUI;
        [SerializeField] private CustomUIAnimation _bookCustomAnimation;

        [Space(15f)]
        [SerializeField] private List<CodexOverview> _codexOverview;

        private RectTransform _overviewRectTransform;

        private CustomImage _openBookCustomImage;
        private CustomImage _closedBookCustomImage;

        private void OnValidate()
        {
            InitializeOverviewData();
        }

        private void Awake()
        {
            _overviewRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
            _overviewRectTransform.anchoredPosition = new(_overviewRectTransform.anchoredPosition.x - X_OFFSET, _overviewRectTransform.anchoredPosition.y);

            _openBookCustomImage = transform.GetChild(1).GetComponent<CustomImage>();
            _closedBookCustomImage = transform.GetChild(2).GetComponent<CustomImage>();
        }

        private void Start()
        {
            StartCoroutine(InitializeCategoryList());
        }

        private void OnEnable()
        {
            PersistenceSystem.OnLoadCompleted += HandleLoadGameData;
            _enemyUI.OnNewNotification += HandleNewNotification;
        }

        private void OnDisable()
        {
            PersistenceSystem.OnLoadCompleted -= HandleLoadGameData;
            _enemyUI.OnNewNotification -= HandleNewNotification;
        }

        public void OnEnter()
        {
            _bookCustomAnimation.StopUIAnimation();
            _openBookCustomImage.GetComponent<Book>().ResetPages();

            _openBookCustomImage.EnableImage(false);
            _closedBookCustomImage.EnableImage(true);

            _closedBookCustomImage.GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
            _closedBookCustomImage.GetComponent<RectTransform>().DOScale(1f, .5f).SetEase(Ease.InOutExpo).OnComplete(() =>
            {
                //_closedBookCustomImage.GetComponent<RectTransform>().DOLocalMoveX(300f, 0.25f);
                var animUI = _closedBookCustomImage.GetComponent<CustomUIAnimation>();

                _overviewRectTransform.DOLocalMoveX(-885f, .5f).SetDelay(animUI.Duration - animUI.Speed).SetEase(Ease.OutQuad);
                animUI.PlayUIAnimation(() =>
                {
                    _openBookCustomImage.EnableImage(true);
                    _closedBookCustomImage.EnableImage(false);

                    _openBookCustomImage.GetComponent<Book>().DisplayActiveEnemy();
                });
            });
        }

        public void OnExit()
        {
            _bookCustomAnimation.StopUIAnimation();
            _openBookCustomImage.GetComponent<Book>().ResetPages();

            _overviewRectTransform.DOKill();
            _closedBookCustomImage.GetComponent<RectTransform>().DOKill();

            _overviewRectTransform.anchoredPosition = new(_overviewRectTransform.anchoredPosition.x - X_OFFSET, _overviewRectTransform.anchoredPosition.y);
        }

        private void HandleLoadGameData(GameData gameData)
        {
            Debug.Log("Load data");
            var hasNewData = false;

            if (gameData.CodexData.CategoryData.Count <= 0) return;

            for (int i = 0; i < _codexOverview.Count; i++)
            {
                var enemyList = _codexOverview[i].EnemyList;
                
                for (int j = 0; j < enemyList.Count; j++)
                {
                    var categoryData = gameData.CodexData.CategoryData[i];

                    if (categoryData.CodexEnemyData.Count <= 0) break;
                    var enemyData = categoryData.CodexEnemyData[j];

                    enemyList[j].EnemySO.Discovery.Value = (DiscoveryStage)enemyData.Discovery;
                    enemyList[j].EnemyUI.DisplayNotification(enemyData.IsNew);

                    if(!hasNewData && enemyData.IsNew)
                        hasNewData = true;
                }
            }

            OnNewNotification?.Invoke(hasNewData);
            OnCodexUpdate?.Invoke(GetCodexData());
        }

        [Button("Discover New Enemy")]
        private void DiscoverNewEnemy()
        {
            var enemies = new List<EnemySO>();

            foreach (var category in _codexOverview)
            {
                var enemyList = category.EnemyList.FindAll(m => m.IsNew == false);
                foreach (var enemy in enemyList)
                    enemies.Add(enemy.EnemySO);
            }

            enemies[UnityEngine.Random.Range(0, enemies.Count)].Discovery.Value = (DiscoveryStage)UnityEngine.Random.Range(1, 3);
        }

        private IEnumerator InitializeCategoryList()
        {
            foreach (Transform child in _categoryListParent)
                Destroy(child.gameObject);

            var objectsToDisable = new List<GameObject>();
            foreach (var category in _codexOverview)
            {
                var categoryUI = Instantiate(_categoryUI, _categoryListParent, false);
                categoryUI.name = $"[CATEGORY] {category.Name}";
                category.CategoryUI = categoryUI;

                var enemyListUI = Instantiate(_enemyListUI, _categoryListParent, false);
                enemyListUI.name = $"[ENEMY LIST] {category.Name}";

                foreach (var item in category.EnemyList)
                {
                    var enemyUI = Instantiate(_enemyUI, enemyListUI, false);
                    enemyUI.name = $"[ENEMY] {item.Name}";
                    item.EnemyUI = enemyUI;

                    enemyUI.OnNewNotification += HandleNewNotification;
                    enemyUI.InitializeEnemyUI(item.EnemySO);
                }

                categoryUI.InitializeCategoryUI(category.Name, enemyListUI.gameObject);

                objectsToDisable.Add(enemyListUI.gameObject);
            }

            yield return null;

            foreach (var item in objectsToDisable)
                item.SetActive(false);
        }

        private void HandleNewNotification(EnemySO enemySO, bool isNew)
        {
            var category = _codexOverview.Find(m => m.Category == enemySO.Category);
            var enemy = category.EnemyList.Find(m => m.EnemySO == enemySO);
            
            if(enemy != null)
                enemy.IsNew = isNew;

            category.CategoryUI.OnNewNotification?.Invoke(category.IsNew());
            OnNewNotification?.Invoke(_codexOverview.Any(m => m.IsNew()));

            OnCodexUpdate?.Invoke(GetCodexData());
        }
        
        private List<CodexCategoryData> GetCodexData()
        {
            var codexData = new List<CodexCategoryData>();

            foreach (var category in _codexOverview)
            {
                var enemyDataList = new List<CodexEnemyData>();

                foreach (var enemy in category.EnemyList)
                    enemyDataList.Add(new CodexEnemyData(enemy.Name, (int)enemy.EnemySO.Discovery.Value, enemy.IsNew));

                codexData.Add(new CodexCategoryData(category.Name, enemyDataList));
            }

            return codexData;
        }

        [ContextMenu("Initialize Overview Data")]
        private void InitializeOverviewData()
        {
            if (_codexOverview == null) return;

            foreach (var item in _codexOverview)
                item.Initialize();
        }
    }


    [System.Serializable]
    public class CodexOverview
    {
        public string Name;
        public EnemyCategory Category;
        public List<CodexEnemy> EnemyList;

        public CodexCategoryUI CategoryUI { get; set; }
        public bool IsNew() => EnemyList.Any(m => m.IsNew);

        public void Initialize()
        {
            Name = Category.ToSpacedString();

            EnemyList.Clear();
            var data = Resources.LoadAll($"Maledictus/Enemies/{Category}");
            foreach (var enemy in data)
            {
                var enemySO = (EnemySO)enemy;

                var newCodexEnemy = new CodexEnemy(enemySO);
                if (!EnemyList.Contains(newCodexEnemy) && enemySO.IsActive)
                    EnemyList.Add(newCodexEnemy);
            }
        }
    }

    [System.Serializable]
    public class CodexEnemy
    {
        public string Name;

        public bool IsNew = false;

        public EnemySO EnemySO { get; private set; }
        public CodexEnemyUI EnemyUI { get; set; }

        public CodexEnemy(EnemySO enemySO)
        {
            Name = enemySO.Name;
            EnemySO = enemySO;
        }
    }
}