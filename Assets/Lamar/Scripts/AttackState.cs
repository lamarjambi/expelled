using UnityEngine;
using StarterAssets;

namespace Expelled.Combat
{
    public class AttackState : MonoBehaviour
    {
        public AudioSource PlayerHitAudio;

        private Animator _animator;
        private StarterAssetsInputs _input;

        private float _timePassed;
        private bool _inAttack;
        private bool _attackQueued;

        private void Awake()
        {
            // :desc: cache animator and input references
            _animator = GetComponent<Animator>();
            _input = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            // :desc: handle attack input and chain queued attacks once the current clip finishes
            if (!_inAttack)
            {
                if (_input.attack)
                {
                    EnterAttack();
                    _input.attack = false;
                }
                return;
            }

            if (_input.attack)
            {
                _attackQueued = true;
                _input.attack = false;
            }

            _timePassed += Time.deltaTime;

            var clipInfoArray = _animator.GetCurrentAnimatorClipInfo(1);
            if (clipInfoArray.Length == 0) return;

            float clipLength = clipInfoArray[0].clip.length;
            float clipSpeed = _animator.GetCurrentAnimatorStateInfo(1).speed;
            float duration = clipLength / clipSpeed;

            if (_timePassed >= duration)
            {
                if (_attackQueued)
                {
                    EnterAttack();
                }
                else
                {
                    ExitAttack();
                }
            }
        }

        private void EnterAttack()
        {
            // :desc: start an attack swing, reset timer, freeze movement, play audio
            _inAttack = true;
            _attackQueued = false;
            _timePassed = 0f;
            //_animator.applyRootMotion = true;
            _animator.SetTrigger("Attack");
            _animator.SetFloat("Speed", 0f);
            if (PlayerHitAudio != null) PlayerHitAudio.Play();
        }

        private void ExitAttack()
        {
            // :desc: clear attack state so movement can resume
            _inAttack = false;
            //_animator.applyRootMotion = false;
        }

        public bool IsAttacking => _inAttack;
    }
}
