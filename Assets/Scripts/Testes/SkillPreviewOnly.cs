using UnityEngine;

public class SkillPreviewOnly : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform firePoint;

    [Header("Skill Preview")]
    [SerializeField]
    private string animationTrigger = "Teste";

    [SerializeField]
    private GameObject effectPrefab;

    private GameObject currentEffect;

    void Reset()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (firePoint == null)
        {
            FirePoint fp = GetComponentInChildren<FirePoint>(true);
            if (fp != null)
                firePoint = fp.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            PlayPreview();
    }

    public void PlayPreview()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator não encontrado.");
            return;
        }

        animator.SetTrigger(animationTrigger);
        Debug.Log($"[Preview] Trigger disparado: {animationTrigger}");
    }

    public void AnimationEvent_SpawnPreviewEffect()
    {
        if (effectPrefab == null || firePoint == null)
        {
            Debug.LogWarning("EffectPrefab ou FirePoint não definido.");
            return;
        }

        if (currentEffect != null)
            Destroy(currentEffect);

        currentEffect = Instantiate(effectPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("[Preview] efeito spawnado por Animation Event");
    }

    public void AnimationEvent_StartEffectMoveForward()
    {
        if (currentEffect == null)
            return;

        SkillEffectPreview preview = currentEffect.GetComponent<SkillEffectPreview>();
        if (preview == null)
            return;

        preview.StartMoveForward();

        Debug.Log("[Preview] efeito começou a mover para frente");
    }

    public void AnimationEvent_DestroyPreviewEffect()
    {
        if (currentEffect == null)
            return;

        currentEffect.SetActive(false);
        currentEffect = null;

        Debug.Log("[Preview] efeito desativado por Animation Event");
    }
}
