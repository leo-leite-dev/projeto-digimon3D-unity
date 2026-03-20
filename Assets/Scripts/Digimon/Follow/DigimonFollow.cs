using UnityEngine;

public class DigimonFollow : Digimon
{
    [Header("External Dependencies (injeção explícita)")]
    private Transform player;
    private Transform followPoint;

    private DigimonMovement movement;
    private DigimonAttack attack;
    private DigimonAnimator digimonAnimator;
    private DigimonFollowController combat;

    [Header("Follow")]
    [SerializeField]
    private float repathDistance = 0.25f;

    private Vector3 lastFollowPosition;

    public override void Setup(DigimonData digimonData)
    {
        base.Setup(digimonData);
    }

    public void Inject(DigimonReferences references, DigimonFollowController combatController)
    {
        if (references == null)
        {
            Debug.LogError("❌ DigimonReferences não pode ser nulo", this);
            return;
        }

        if (combatController == null)
        {
            Debug.LogError("❌ CombatController NÃO foi injetado no Follow", this);
            return;
        }

        movement = references.Movement;
        attack = references.Attack;
        digimonAnimator = references.DigimonAnimator;

        combat = combatController;
    }

    public void Initialize(Transform playerRef, Transform followPointRef)
    {
        player = playerRef;
        followPoint = followPointRef;

        if (followPoint != null)
            lastFollowPosition = followPoint.position;
    }

    private void Update()
    {
        if (Data == null)
            return;

        if (player == null || followPoint == null)
            return;

        var decision = combat != null ? combat.Tick() : CombatDecision.None;

        if (decision.Action != CombatAction.None)
            ExecuteCombatDecision(decision);
        else
            FollowPlayer();

        UpdateAnimation();
    }

    private void FollowPlayer()
    {
        if (movement == null)
            return;

        Vector3 targetPosition = followPoint.position;
        float sqrDistance = (targetPosition - lastFollowPosition).sqrMagnitude;

        if (sqrDistance >= repathDistance * repathDistance)
        {
            movement.SetDestination(targetPosition);
            lastFollowPosition = targetPosition;
        }
    }

    private void ExecuteCombatDecision(CombatDecision decision)
    {
        if (movement == null)
        {
            Debug.LogError("❌ Movement NULL no Follow");
            return;
        }

        if (attack == null)
        {
            Debug.LogError("❌ Attack NULL no Follow");
            return;
        }
        switch (decision.Action)
        {
            case CombatAction.MoveToTarget:
                if (decision.Target != null)
                    movement.MoveToSkillRange(decision.Target, decision.Range);
                break;

            case CombatAction.Stop:
                movement.StopMovement();
                break;

            case CombatAction.UseSkill:
                movement.StopMovement();

                if (decision.Skill != null && decision.SkillTarget != null)
                    attack.TryUseSkill(decision.Skill, decision.SkillTarget);

                break;
        }
    }

    private void UpdateAnimation()
    {
        if (digimonAnimator == null || movement == null)
            return;

        digimonAnimator.SetSpeed(movement.Velocity.magnitude);
    }

    public void RequestSkill(DigimonSkill skill, GameObject target)
    {
        combat?.RequestSkill(skill, target);
    }
}
