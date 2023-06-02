using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashPad : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;

    //This is the speed of the object that hit the crash pad
    public float impactSpeed = 1f;

    public float impactSpeedMultiplier = 0.3f;
    public float TimeFromImpactSpeed = 1.5f;

    //True if the collider of the mesh will update
    public bool UpdateCollider = false;

    //Put it to true if you want the craash pad to shake
    public bool Shake = false;

    private float timer = 0f;

    public float CushionForce = 5;


    private void OnTriggerEnter(Collider other)
    {
        impactSpeed = Mathf.Abs(1f + ((other.gameObject.GetComponent<Rigidbody>().velocity.y) / 4));
        Debug.Log("Hit!");
        Shake = true;


        //Cancels Vertical Velocity of object
        other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(other.gameObject.GetComponent<Rigidbody>().velocity.x, other.gameObject.GetComponent<Rigidbody>().velocity.y / CushionForce, other.gameObject.GetComponent<Rigidbody>().velocity.z);
    }

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void Update()
    {
        if (UpdateCollider)
        {
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        TimeFromImpactSpeed = impactSpeedMultiplier * 3f * impactSpeed; //The float in here is the multiplier for the time

        if (Shake)
        {
            HitObject();

            timer += Time.deltaTime;

            if (timer >= TimeFromImpactSpeed)
                Shake = false;
        }
        else
            ResetVertices();
    }

    //Stop object shaking
    void ResetVertices()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y < 0.01)
                vertices[i] += Vector3.up * (Time.deltaTime * TimeFromImpactSpeed) * 0.1f;
            if (vertices[i].y > 0.01)
                vertices[i] += Vector3.down * (Time.deltaTime * TimeFromImpactSpeed) * 0.1f;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    //Shakes the crash pad
    void HitObject()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[Random.Range(0, vertices.Length)] += Vector3.down * Time.deltaTime * impactSpeedMultiplier * impactSpeed;
        }
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[Random.Range(0, vertices.Length)] += Vector3.up * Time.deltaTime * impactSpeedMultiplier * impactSpeed;
        }

        // assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
