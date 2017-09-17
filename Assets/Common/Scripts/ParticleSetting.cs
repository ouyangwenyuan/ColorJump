using UnityEngine;
using UnityEngine.UI;

public class ParticleSetting : MonoBehaviour
{
    public ParticleSystem particle;
    public string sortingLayer;
    public int sortingOrder;

    private void Start()
    {
        particle.GetComponent<Renderer>().sortingLayerName = sortingLayer;
        particle.GetComponent<Renderer>().sortingOrder = sortingOrder;
    }
}
