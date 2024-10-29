using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererRandomizer : MonoBehaviour
{
    public List<MeshRenderer> meshRenderers;
    public bool randomizeRotations;


    private void Start()
    {
        Randomize();
    }
    public void Randomize()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.gameObject.SetActive(false);
        }
        meshRenderers.GetRandomElement().gameObject.SetActive(true);
        //Randomize the rotation from the four main directions
        if (randomizeRotations)
        {
            transform.DORotate(new Vector3(0, Random.Range(0, 4) * 90, 0), 0.5f);
        }
    }
}
