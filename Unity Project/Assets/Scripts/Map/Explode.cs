using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public void ExplodeIn(float time)
    {
        Invoke("ExplodeObject", time);
    }

    private void ExplodeObject()
    {
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            gameObject.AddComponent<Rigidbody>();
        }

        if (gameObject.GetComponent<BoxCollider>())
            gameObject.GetComponent<BoxCollider>().enabled = false;

        gameObject.GetComponent<Rigidbody>().AddForce(
            Random.Range(0.0f, 3.0f) *
            new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.5f, 1.0f)).normalized,
            ForceMode.Impulse);

        gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-1.0f, 1.0f), ForceMode.Impulse);
    }
}