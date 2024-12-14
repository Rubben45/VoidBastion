using UnityEngine;

public class AutoDestructParticles : MonoBehaviour
{
    private ParticleSystem currentParticleSystem;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!currentParticleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
