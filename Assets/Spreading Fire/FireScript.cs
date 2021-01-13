using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    //public float burnDistance;
    public ParticleSystem smoke;
    public ParticleSystem fire;
    public ParticleSystem dissolve;

    public bool SmokeOnStart = false;
    public bool Dissolvable = true;

    public float SmokeDuration = 3;
    public float FireDuration = 5;

    public bool Smoking { get; private set; } = false;
    public bool Burning { get; private set; } = false;

    private Vector3 oldPosition;

    private float smokeStart;
    private float fireStart;

    // Start is called before the first frame update
    void Start()
    {
        //StartSmoke();
        oldPosition = transform.position;

        if (SmokeOnStart) StartSmoke();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position != oldPosition)
        {
            float d = Vector3.Magnitude(transform.position - oldPosition);
            FireManager.Instance.OnBurnableMoved(this.gameObject, d);

            oldPosition = transform.position;
        }
    }

    public void StartSmoke()
    {
        if (this.Burning) return;

        smokeStart = Time.time;
        this.StartCoroutine(Smoke(smokeStart));
    }

    public void StartFire()
    {
        fireStart = Time.time;
        this.StartCoroutine(Fire(fireStart));
    }

    public void StopBurning()
    {
        if (this.Smoking && !SmokeOnStart)
        {
            this.Smoking = false;
            this.smoke.Stop();
        }

        if (this.Burning)
        {
            this.Burning = false;
            FireManager.Instance.OnBurnStopped(this.gameObject);

            this.fire.Stop();
        }
    }

    IEnumerator Smoke(float t)
    {
        this.Smoking = true;

        this.smoke.Play();

        yield return new WaitForSeconds(SmokeDuration);

        if (this.Smoking && t == smokeStart)
        {
            fireStart = Time.time;
            StartCoroutine(Fire(fireStart));
        }
    }

    IEnumerator Fire(float t)
    {
        //Debug.Log("Starting fire");
        this.Burning = true;
        this.fire.Play();

        FireManager.Instance.OnBurnStarted(this.gameObject);

        yield return new WaitForSeconds(0.5f);

        if (this.Smoking)
        {
            this.Smoking = false;
            this.smoke.Stop();
        }

        yield return new WaitForSeconds(FireDuration);

        if (this.Burning && t == fireStart && Dissolvable)
        {
            StartCoroutine(Dissolve());
        }
    }

    IEnumerator Dissolve()
    {
        if (!Dissolvable) yield break;

        // If it is no longer burning, then do not dissolve
        // after previous bigfixes, should not be needed anymore
        if (!this.Burning)
        {
            yield break;
        }

        this.Burning = false;
        FireManager.Instance.OnBurnStopped(this.gameObject);

        this.fire.Stop();

        dissolve.Play();

        MeshRenderer mr = GetComponent<MeshRenderer>();
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();

        if (mr)
        {
            mr.enabled = false;
        }

        if (smr)
        {
            smr.enabled = false;
        }

        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = false;
        }

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
