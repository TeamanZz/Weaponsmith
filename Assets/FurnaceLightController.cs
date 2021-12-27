using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceLightController : MonoBehaviour
{
    [SerializeField] AnimationCurve lightIntensity;

    private float currentTime, totalTime;
    private Light furnaceLight;
    public GameObject particles;
    public Transform particlesParent;

    private void Start()
    {
        furnaceLight = GetComponent<Light>();

        totalTime = lightIntensity.keys[lightIntensity.keys.Length - 1].time;
        StartCoroutine(SpawnParticles());
    }

    private IEnumerator SpawnParticles()
    {
        yield return new WaitForSeconds(Random.Range(7, 20));
        Instantiate(particles, particlesParent);
        yield return SpawnParticles();
    }

    private void Update()
    {
        furnaceLight.intensity = lightIntensity.Evaluate(currentTime);

        currentTime += Time.deltaTime;

        if (currentTime >= totalTime)
            currentTime = 0;
    }
}