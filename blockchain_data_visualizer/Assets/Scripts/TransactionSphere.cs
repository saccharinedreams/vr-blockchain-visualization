using UnityEngine;

public class TransactionSphere
{

    public void createSphere(Color sphereColor, float sphereSize, Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
        sphere.GetComponent<Renderer>().material.color = sphereColor;
        SphereMovement sphereMovement = sphere.AddComponent<SphereMovement>();
    }
    
}
