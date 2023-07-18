using UnityEngine;

namespace Assets.Scripts.Players.States
{
    public sealed  class PlayerCounterAttackState : PlayerState
    {
        private float counterAttackExitTime = 10;

        public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateTimer = _player.counterAttackDuration;
            _player.animator.SetBool(PlayerConstants.SUCCESSFUL_COUNTER_ATTACK, false);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            _player.SetZeroVelocity();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    if (hit.GetComponent<Enemy>().CanBeStunned())
                    {
                        stateTimer = counterAttackExitTime;
                        _player.animator.SetBool(PlayerConstants.SUCCESSFUL_COUNTER_ATTACK, true);
                    }
                  
                }
            }
            if (stateTimer < 0 || triggerCalled)
                _stateMachine.ChangeState(_player.PlayerIdleState);
        }
    }
}
