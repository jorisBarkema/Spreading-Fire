using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance { get; private set; }

    public float burnDistance = 0.6f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnBurnStarted(GameObject start)
    {
        List<GameObject> burnables = this.GetBurnablesNear(start);

        foreach (GameObject b in burnables)
        {
            FireScript fireScript = b.GetComponent<FireScript>();

            if (!fireScript.Smoking && !fireScript.Burning)
            {
                fireScript.StartSmoke();
            }
        }
    }

    public void OnBurnStopped(GameObject start)
    {
        CheckExtinguishNear(start, burnDistance);
    }

    public void OnBurnableMoved(GameObject movedObject, float d)
    {
        FireScript movedScript = movedObject.GetComponent<FireScript>();

        if (movedScript.Burning)
        {
            CheckExtinguishNear(movedObject, burnDistance + d);
            OnBurnStarted(movedObject);
        }

        if (movedScript.Smoking)
        {
            CheckExtinguishFor(movedObject);
        }

        if (!movedScript.Smoking && !movedScript.Burning)
        {
            List<GameObject> burnables = GetBurnablesNear(movedObject);

            foreach (GameObject o in burnables)
            {
                FireScript b = o.GetComponent<FireScript>();
                if (b && b.Burning)
                {
                    movedScript.StartSmoke();
                }
            }
        }
    }

    private void CheckExtinguishFor(GameObject start)
    {
        FireScript startBurnSript = start.GetComponent<FireScript>();

        bool shouldBurn = ShouldBurn(start);

        if (!shouldBurn && startBurnSript.Smoking && !startBurnSript.Burning)
        {
            startBurnSript.StopBurning();
        }
    }

    public bool ShouldBurn(GameObject o)
    {
        List<GameObject> burnables = this.GetBurnablesNear(o);
        bool shouldBurn = false;

        foreach (GameObject b1 in burnables)
        {
            if (b1.GetComponent<FireScript>().Burning)
            {
                shouldBurn = true;
            }
        }

        return shouldBurn;
    }

    private void CheckExtinguishNear(GameObject start)
    {
        CheckExtinguishNear(start, burnDistance);
    }

    private void CheckExtinguishNear(GameObject start, float distance)
    {
        List<GameObject> burnables = this.GetBurnablesNear(start, distance);

        foreach (GameObject b1 in burnables)
        {
            FireScript fireScript1 = b1.GetComponent<FireScript>();
            
            bool shouldBurn = ShouldBurn(b1);

            if (!shouldBurn && !fireScript1.Burning && fireScript1.Smoking)
            {
                fireScript1.StopBurning();
            }
        }
    }
    
    List<GameObject> GetBurnablesNear(GameObject start)
    {
        return GetBurnablesNear(start, burnDistance);
    }

    List<GameObject> GetBurnablesNear(GameObject start, float distance)
    {
        List<GameObject> burnables = new List<GameObject>();
        GameObject[] objects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject o in objects)
        {
            if (o == start) continue;

            FireScript b = o.GetComponent<FireScript>();
            if (b)
            {
                float d = DistanceBetween(start, o);

                if (d < distance)
                {
                    burnables.Add(o);
                }
            }
        }

        return burnables;
    }
    
    private List<Collider> CollidersFrom(GameObject o)
    {
        List<Collider> colliders = new List<Collider>();

        foreach (Collider c in o.GetComponents<Collider>())
        {
            colliders.Add(c);
        }

        foreach (Collider c in o.GetComponentsInChildren<Collider>())
        {
            colliders.Add(c);
        }

        return colliders;
    }

    private float DistanceBetween(GameObject a, GameObject b)
    {
        List<Tuple<Vector3, Vector3>> pairs = new List<Tuple<Vector3, Vector3>>();

        List<Vector3> pointsA = new List<Vector3>();
        List<Vector3> pointsB = new List<Vector3>();

        foreach (Collider c in CollidersFrom(a))
        {
            Vector3 closestPoint = c.ClosestPointOnBounds(b.transform.position);
            pointsA.Add(closestPoint);
        }

        foreach (Collider c in CollidersFrom(b))
        {
            Vector3 closestPoint = c.ClosestPointOnBounds(a.transform.position);
            pointsB.Add(closestPoint);
        }

        float minDistance = float.MaxValue;

        foreach (Vector3 from in pointsA)
        {
            foreach (Vector3 to in pointsB)
            {
                float d = Vector3.Distance(from, to);
                if (d < minDistance)
                {
                    minDistance = d;
                }
            }
        }

        return minDistance;
    }
}
