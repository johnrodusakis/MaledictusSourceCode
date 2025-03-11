using Obvious.Soap;
using SomniaGames.Persistence;
using System.Collections.Generic;
using UnityEngine;

namespace Maledictus.Level
{
    public class LevelSystem : MonoBehaviour
    {
        public static event System.Action<int, int> OnLevelUpdate;

        public static readonly Dictionary<int, int> ExperiencePerLevel = new()
        {
            { 1,  0    },   { 11, 2250 },   { 21, 6600  },  { 31, 14000 },  { 41, 25400 },
            { 2,  100  },   { 12, 2500 },   { 22, 7200  },  { 32, 15000 },  { 42, 26800 },
            { 3,  200  },   { 13, 2750 },   { 23, 7800  },  { 33, 16000 },  { 43, 28200 },
            { 4,  400  },   { 14, 3000 },   { 24, 8400  },  { 34, 17000 },  { 44, 29600 },
            { 5,  600  },   { 15, 3500 },   { 25, 9000  },  { 35, 18000 },  { 45, 31000 },
            { 6,  800  },   { 16, 4000 },   { 26, 9800  },  { 36, 19200 },  { 46, 32800 },
            { 7,  1000 },   { 17, 4500 },   { 27, 10600 },  { 37, 20400 },  { 47, 34600 },
            { 8,  1300 },   { 18, 5000 },   { 28, 11400 },  { 38, 21600 },  { 48, 36400 },
            { 9,  1600 },   { 19, 5500 },   { 29, 12200 },  { 39, 22800 },  { 49, 38200 },
            { 10, 2000 },   { 20, 6000 },   { 30, 13000 },  { 40, 24000 },  { 50, 40000 },
        };

        [SerializeField] private ScriptableVariable<int> _level;
        [SerializeField] private ScriptableVariable<int> _experience;
        [SerializeField] private ScriptableVariable<int> _experienceToNextLevel;

        [Space(15f)]
        [SerializeField] private ScriptableEvent<float> _onGainExperience;

        private void OnEnable()
        {
            PersistenceSystem.OnLoadCompleted += HandleLoadGameData;
            _onGainExperience.OnRaised += AddExperience;
        }

        private void OnDisable()
        {
            PersistenceSystem.OnLoadCompleted -= HandleLoadGameData;
            _onGainExperience.OnRaised -= AddExperience;
        }

        private void AddExperience(float percentage)
        {
            var expAmount = Mathf.RoundToInt(_experienceToNextLevel * percentage * 0.01f);
            IncreaseExperienceBy(expAmount);

            OnLevelUpdate?.Invoke(_level.Value, _experience.Value);
        }

        private void IncreaseExperienceBy(int amount)
        {
            if (IsMaxLevel() && _experience.Value == _experienceToNextLevel)
                return;

            _experience.Value += amount;

            while (!IsMaxLevel() && _experience >= _experienceToNextLevel)
            {
                _level.Value++;

                if (!IsMaxLevel())
                    _experience.Value -= _experienceToNextLevel;
                else
                    _experience.Value = _experienceToNextLevel;

                _experienceToNextLevel.Value = ExperiencePerLevel[IsMaxLevel() ? _level.Value : _level.Value + 1];
            }
        }

        private void HandleLoadGameData(GameData gameData)
        {
            var data = gameData.LevelData;

            _level.Value = data.Level;
            _experience.Value = data.Experience;

            _experienceToNextLevel.Value = ExperiencePerLevel[IsMaxLevel() ? data.Level : data.Level + 1];

            OnLevelUpdate?.Invoke(_level.Value, _experience.Value);
        }

        private bool IsMaxLevel() => _level.Value == ExperiencePerLevel.Count;
    }
}