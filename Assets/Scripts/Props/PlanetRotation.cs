using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetRotation : MonoBehaviour
{
    public List<Material> materials;
    public List<float> rotationBounds;
    public float rotationReduction;
    private Renderer _renderer;
    private List<float> speeds = new List<float>();

    public List<float> sizeBounds;

    private float rotX;
    private float rotY;
    private float rotZ;

    private float velocityMin;
    private float velocityMax;

    private Vector3 velocity;

    private Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var ranMove = Random.Range(velocityMin, velocityMax);
        var ranSize = Random.Range(sizeBounds[0], sizeBounds[1]);
        transform.localScale = new Vector3(ranSize, ranSize, ranSize);
        _renderer = GetComponent<Renderer>();
        var ranVel = Random.Range(0, materials.Count);
        _renderer.material = materials[ranVel];
        for (int i = 0; i < 3; i++)
        {
            speeds.Add(Random.Range(rotationBounds[0], rotationBounds[1] - i*rotationReduction));
        }

        var ranAxis = Random.Range(0, materials.Count);
        if (ranAxis == 0)
        {
            rotX = speeds[0];
            rotY = speeds[1];
            rotZ = speeds[2];
            velocity = new Vector3(ranMove, 0, 0);
        }
        else if (ranAxis == 1)
        {
            rotX = speeds[2];
            rotY = speeds[0];
            rotZ = speeds[1];
            velocity = new Vector3(0, ranMove, 0);
        }
        else
        {
            rotX = speeds[1];
            rotY = speeds[2];
            rotZ = speeds[0];
            velocity = new Vector3(ranMove/2, ranMove/2, 0);
        }
    }

    private void Update()
    {
        transform.Rotate(rotX, rotY, rotZ);
        transform.position += velocity;
    }
}
