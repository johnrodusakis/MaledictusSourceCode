using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Maledictus.CustomUI
{
    [RequireComponent(typeof(CustomImage))]
    public class CustomUIAnimation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _loop = false;
        [SerializeField] private float _speed = 0.2f;

        [Space(10f)]
        [Header("Animation")]
        [SerializeField] private Sprite[] _spriteArray;

        public float Duration => _spriteArray.Length * _speed;
        public float Speed => _speed;

        private int _indexSprite;
        private Coroutine _coroutineAnim;
        private bool _isDone;

        private CustomImage _customImage;

        private void Awake()
        {
            _customImage = GetComponent<CustomImage>();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            ResetAnimation();
        }

        [Button] public void PlayAnimation() => PlayUIAnimation(null);

        public void PlayUIAnimation(System.Action OnComplete = null)
        {
            _isDone = false;
            ResetAnimation();

            StartCoroutine(PlayAnimationUI(OnComplete));
        }

        public void StopUIAnimation()
        {
            _isDone = true;
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            _indexSprite = 0;
            _customImage.SetImage(_spriteArray[_indexSprite]);

            if (_coroutineAnim != null)
            {
                StopCoroutine(_coroutineAnim);
                _coroutineAnim = null;
            }
        }

        private IEnumerator PlayAnimationUI(System.Action OnComplete)
        {
            yield return new WaitForSeconds(_speed);


            if(_indexSprite >= _spriteArray.Length)
            {
                _indexSprite = 0;

                if (!_loop)
                {
                    StopUIAnimation();
                    OnComplete?.Invoke();
                    yield break;
                }
            }

            _customImage.SetImage(_spriteArray[_indexSprite]);
            _indexSprite++;

            if (_isDone == false)
                _coroutineAnim = StartCoroutine(PlayAnimationUI(OnComplete));
        }
    }
}