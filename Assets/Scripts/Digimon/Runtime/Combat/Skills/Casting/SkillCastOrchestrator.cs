using System.Collections;
using UnityEngine;

public class SkillCastOrchestrator
{
    private readonly SkillUsageValidator usageValidator;
    private readonly SkillExecutionState executionState;
    private readonly SkillCastPipeline pipeline;

    private MonoBehaviour runner;
    private Coroutine timeoutRoutine;

    public SkillCastOrchestrator(
        SkillUsageValidator usageValidator,
        SkillExecutionState executionState,
        SkillCastPipeline pipeline
    )
    {
        this.usageValidator = usageValidator;
        this.executionState = executionState;
        this.pipeline = pipeline;
    }

    public void SetRunner(MonoBehaviour runner)
    {
        this.runner = runner;
    }

    public SkillUseCheckResult Evaluate(
        Digimon digimon,
        Transform caster,
        DigimonSkill skill,
        Transform target
    )
    {
        if (digimon == null || caster == null || skill == null || target == null)
            return SkillUseCheckResult.InvalidSkill;

        return usageValidator.Evaluate(digimon, caster, skill, target);
    }

    public bool TryStart(Digimon digimon, Transform caster, DigimonSkill skill, Transform target)
    {
        var result = Evaluate(digimon, caster, skill, target);
        if (result != SkillUseCheckResult.Success)
            return false;

        if (executionState.IsCasting)
        {
            Debug.LogWarning("⚠️ Cast anterior não finalizado → forçando finish");
            Finish();
        }

        executionState.Begin(digimon, skill, target);

        pipeline.OnSkillStarted(skill, target);

        StartTimeout(skill);

        return true;
    }

    public void Finish()
    {
        if (!executionState.IsCasting)
            return;

        pipeline.OnSkillFinished(executionState.CurrentSkill);

        executionState.Finish();

        StopTimeout();
    }

    private void StartTimeout(DigimonSkill skill)
    {
        if (runner == null)
        {
            Debug.LogWarning("⚠️ SkillCastOrchestrator sem runner → timeout desativado");
            return;
        }

        StopTimeout();

        float timeout = Mathf.Max(skill.lifeTime + 1.5f, 3f);

        timeoutRoutine = runner.StartCoroutine(TimeoutRoutine(timeout));
    }

    private void StopTimeout()
    {
        if (runner == null || timeoutRoutine == null)
            return;

        runner.StopCoroutine(timeoutRoutine);
        timeoutRoutine = null;
    }

    private IEnumerator TimeoutRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        if (executionState.IsCasting)
        {
            Debug.LogWarning("⏱️ Skill travou → forçando finish");
            Finish();
        }
    }
}
