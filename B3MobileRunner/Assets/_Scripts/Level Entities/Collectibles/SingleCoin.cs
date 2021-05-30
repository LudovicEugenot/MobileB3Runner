using System.Collections;
using UnityEngine;

public class SingleCoin : Collectible
{
    #region Initialization
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Vector2 minSpawnEjectionForce;
    [SerializeField] Vector2 maxSpawnEjectionForce;
    #endregion

    public override void OnStart()
    {
        if (rb == null) Debug.LogWarning("renseigne le rigidbody dans l'inspecteur !", this);

        rb.AddForce(new Vector2(
            Random.Range(minSpawnEjectionForce.x, maxSpawnEjectionForce.x)+ Manager.Instance.playerScript.moveSpeed, 
            Random.Range(minSpawnEjectionForce.y, maxSpawnEjectionForce.y)), 
            ForceMode2D.Impulse);
    }

    public override void GetCollected()
    {
        Manager.Instance.CoinAmount += ObjectsData.CoinValue; //WIP feature mort qui collecte pièces quand il est au dessus du sol
        Manager.Instance.sound.PlayCoin();
        Destroy(gameObject);
    }

    public override void OnUpdate()
    {
        if (transform.position.y < Manager.Instance.neutralYOffset -.5f && rb.velocity.y<0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            if (rb.velocity.magnitude < 5f) transform.position = new Vector2(transform.position.x, Manager.Instance.neutralYOffset - 0.5f);
        }
    }

    public override bool CollectionConditions()
    {
        if (Vector2.Distance(Manager.Instance.playerTrsf.position, transform.position) < 1f)
            return true;
        return rb.velocity.x < .5f;
    }
}
