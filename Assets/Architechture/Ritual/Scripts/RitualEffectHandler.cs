using UnityEngine;

public class RitualEffectHandler : MonoBehaviour
{
    [Header("Effects")]
    public ParticleSystem summonSuccessParticles;
    public ParticleSystem summonFailureParticles;
    public ParticleSystem sacrificeParticles;

    public void PlaySuccessEffect(Vector3 position)
    {
        if (summonSuccessParticles != null)
        {
            Instantiate(summonSuccessParticles, position, Quaternion.identity);
        }
        Debug.Log("[RitualEffectHandler] Played Success Effect at " + position);
    }

    public void PlayFailureEffect(Vector3 position)
    {
        if (summonFailureParticles != null)
        {
            Instantiate(summonFailureParticles, position, Quaternion.identity);
        }
        Debug.Log("[RitualEffectHandler] Played Failure Effect at " + position);
    }

    public void PlaySacrificeEffect(Vector3 position)
    {
        if (sacrificeParticles != null)
        {
            Instantiate(sacrificeParticles, position, Quaternion.identity);
        }
        Debug.Log("[RitualEffectHandler] Played Sacrifice Effect at " + position);
    }
}
