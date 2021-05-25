using System.Collections;
using UnityEngine;

public class SingleCoin : Collectible
{
    #region Initialization
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Vector2 minSpawnEjectionForce;
    [SerializeField] Vector2 maxSpawnEjectionForce;

    private SoundManager soundManager;
    #endregion

    public override void OnStart()
    {
        soundManager.GetComponentInChildren<SoundManager>();
        if (rb == null) Debug.LogWarning("renseigne le rigidbody dans l'inspecteur !", this);

        rb.AddForce(new Vector2(
            Random.Range(minSpawnEjectionForce.x, maxSpawnEjectionForce.x)+ Manager.Instance.playerScript.moveSpeed, 
            Random.Range(minSpawnEjectionForce.y, maxSpawnEjectionForce.y)), 
            ForceMode2D.Impulse);
    }

    public override void GetCollected()
    {
        Manager.Instance.CoinAmount += ObjectsData.CoinValue; //WIP feature mort qui collecte pièces quand il est au dessus du sol
        soundManager.PlayCoin();
        Destroy(gameObject);
    }

    public override void OnUpdate()
    {
        if (transform.position.y < -.5f && rb.velocity.y<0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            if (rb.velocity.magnitude < 5f) transform.position = new Vector2(transform.position.x, -0.5f);
        }
    }
}
