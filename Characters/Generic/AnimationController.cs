using UnityEngine;

namespace Maledictus
{
    public class AnimationController : MonoBehaviour
    {

        #region Animator Parameters

        private readonly int Horizontal = Animator.StringToHash("Horizontal");
        private readonly int Vertical = Animator.StringToHash("Vertical");

        private readonly int IsIdle = Animator.StringToHash("IsIdle");
        private readonly int IsRunning = Animator.StringToHash("IsRunning");

        #endregion

        [SerializeField] private Animator _animator;

        public void HandleWalkDirectionAnimation(Vector2 direction) => SetWalkDirectionAnimation(direction.x, direction.y);

        private void SetWalkDirectionAnimation(float dirX, float dirY)
        {
            _animator.SetFloat(Horizontal, dirX);
            _animator.SetFloat(Vertical, dirY);
        }
    }
}