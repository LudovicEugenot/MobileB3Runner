using System.Collections;
using UnityEngine;
using EzySlice;

public class BadApple : ObjectToSlice
{
    #region Initialization
    [Header("References")]
    [SerializeField] GameObject wormPrefab;
    [SerializeField] GameObject apple;

    [Header("Tweakable Stuff")]
    [SerializeField] [Range(0, 10)] int wormCountInApple = 2;

    //code stuff
    GameObject worm;
    #endregion
    public override void Init()
    {
        rb.simulated = true;
    }

    public override void AliveBehaviour()
    {

    }

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        SpawnWorms();
        base.OnDeath(cutImpact, cutDirection); //WIP problème de découpe des pommes, go voir model 3D pomme (/CoinGA/3D Models/ennemi/Enemies/Pomme)
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return false;
    }

    void SpawnWorms()
    {
        for (int i = 0; i < wormCountInApple; i++)
        {
            worm = Instantiate(wormPrefab, transform.position, Quaternion.identity);
        }
    }

    protected override void GetSliced(Vector2 cutImpact, Vector2 cutDirection)
    {
        cutAmount++;
        amDying = true;
        GameObject[] gos;

        gos = apple.SliceInstantiate(Vector3.Lerp(cutImpact, transform.position, 0.8f), // rapprocher la coupe du centre de l'objet de 80%
        Vector3.Cross(cutDirection, Camera.main.transform.forward), cutMat);

        if (gos != null)
        {
            foreach (GameObject gameObject in gos)
            {
                SetUpSlicedObject(gameObject, cutDirection);
            }
        }
        else
        {
            Debug.LogError(gameObject.name + " destroyed like a very bad boy.", this);
        }
        GameObjectDisappear();
    }
}
