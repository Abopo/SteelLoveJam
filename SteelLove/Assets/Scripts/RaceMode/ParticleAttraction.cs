using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleAttraction : MonoBehaviour
{
    [SerializeField] private float _attractionSpeed;
    [SerializeField] private float _killDistance;

    private ParticleSystem _particleSystem;
    private GameObject _attractor;
    private bool _running;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_running && _attractor)
        {
            Particle[] allParticles = new Particle[_particleSystem.particleCount];
            _particleSystem.GetParticles(allParticles);

            for (int i = 0; i < allParticles.Length; ++i)
            {
                var p = allParticles[i];



                allParticles[i].position = Vector3.Lerp(allParticles[i].position, _attractor.transform.position, Time.deltaTime * _attractionSpeed);

                var dist = (allParticles[i].position - _attractor.transform.position).magnitude;
                if(dist <= _killDistance)
                {
                    allParticles[i].remainingLifetime = 0;
                }
            }

            _particleSystem.SetParticles(allParticles);
        }
    }

    public void Begin(GameObject attractor)
    {
        _running = true;
        _attractor = attractor;
    }

    public void Stop()
    {
        _running = false;
        _attractor = null;
    }
}
