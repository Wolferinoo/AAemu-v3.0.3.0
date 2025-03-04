using AAEmu.Game.Models.Game.AI.v2.Behaviors;

namespace AAEmu.Game.Models.Game.AI.v2.AiCharacters
{
    /// <summary>
    /// Named as such because of game files
    /// </summary>
    public class AlmightyNpcAiCharacter : NpcAi
    {
        protected override void Build()
        {
            AddBehavior(BehaviorKind.Spawning, new SpawningBehavior());

            AddBehavior(BehaviorKind.Idle, new IdleBehavior())
                .AddTransition(TransitionEvent.OnAggroTargetChanged, BehaviorKind.AlmightyAttack)
                .AddTransition(TransitionEvent.ReturnToIdlePos, BehaviorKind.ReturnState)
                .AddTransition(TransitionEvent.OnTalk, BehaviorKind.Talk);

            AddBehavior(BehaviorKind.RunCommandSet, new RunCommandSetBehavior())
                .AddTransition(TransitionEvent.OnAggroTargetChanged, BehaviorKind.AlmightyAttack)
                .AddTransition(TransitionEvent.OnTalk, BehaviorKind.Talk);

            AddBehavior(BehaviorKind.Talk, new TalkBehavior())
                .AddTransition(TransitionEvent.OnReturnToTalkPos, BehaviorKind.ReturnState)
                .AddTransition(TransitionEvent.OnAggroTargetChanged, BehaviorKind.AlmightyAttack);

            AddBehavior(BehaviorKind.Alert, new AlertBehavior())
                .AddTransition(TransitionEvent.OnAggroTargetChanged, BehaviorKind.AlmightyAttack);

            AddBehavior(BehaviorKind.AlmightyAttack, new AlmightyAttackBehavior())
                .AddTransition(TransitionEvent.OnNoAggroTarget, BehaviorKind.ReturnState);

            AddBehavior(BehaviorKind.FollowPath, new FollowPathBehavior())
                .AddTransition(TransitionEvent.OnTalk, BehaviorKind.Talk);

            AddBehavior(BehaviorKind.FollowUnit, new FollowUnitBehavior())
                .AddTransition(TransitionEvent.OnAggroTargetChanged, BehaviorKind.AlmightyAttack)
                .AddTransition(TransitionEvent.OnTalk, BehaviorKind.Talk);

            AddBehavior(BehaviorKind.ReturnState, new ReturnStateBehavior());
            AddBehavior(BehaviorKind.Dead, new DeadBehavior());
            AddBehavior(BehaviorKind.Despawning, new DespawningBehavior());
        }

        public override void GoToCombat()
        {
            SetCurrentBehavior(BehaviorKind.AlmightyAttack);
        }
    }
}
