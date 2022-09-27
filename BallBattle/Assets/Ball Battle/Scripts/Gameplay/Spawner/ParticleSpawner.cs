using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [Header("Scene Components")]
    public Transform particleSpawnParent;               // transform to hold all particle effects as children objects

    [Header("Particle Prefabs")]
    public ParticleSystem goalParticle;                 // particle spawned when a ball reached the target gate
    public ParticleSystem fenceParticle;                // particle spawned when a soldier reaches the opponent's fence
    public ParticleSystem regenerationParticle;         // particle spawned during a soldier's reactivation phase

    public void SpawnParticle(ParticleSystem particle, Vector3 position, Transform parent, float duration, float scale)
    {
        StartCoroutine(StartSpawn(particle, position, parent, duration, scale));
    }

    private IEnumerator StartSpawn(ParticleSystem particle, Vector3 position, Transform parent, float duration, float scale)
    {
        // spawn new particle
        ParticleSystem newParticle = Instantiate(particle, parent);
        newParticle.transform.position = position;
        newParticle.transform.rotation = Quaternion.identity;

        // adjust particle scale
        var main = newParticle.main;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;

        newParticle.transform.localScale = Vector3.one * scale;

        foreach (Transform child in newParticle.transform)
        {
            if (child.GetComponent<ParticleSystem>() != null)
            {
                main = child.GetComponent<ParticleSystem>().main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }

            child.localScale = Vector3.one * scale;
        }

        // spawn duration
        yield return new WaitForSeconds(duration);

        // destroy particle after the assigned duration
        if (newParticle != null)
        {
            Destroy(newParticle.gameObject);
        }

        yield break;
    }
}
