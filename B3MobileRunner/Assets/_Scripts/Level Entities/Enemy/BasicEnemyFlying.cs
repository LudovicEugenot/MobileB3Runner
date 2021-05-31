using UnityEngine;

public class BasicEnemyFlying : ObjectToSlice
{
    #region Initiatlization
    [SerializeField] [Range(0f, 25f)] float speed = .5f;

    float playerSpeed;

    [SerializeField] bool isSkull = false;
    [SerializeField] SkinnedMeshRenderer smr;
    [SerializeField] Material blueLevelSkin;
    [SerializeField] Material redLevelSkin;
    #endregion

    public override void AliveBehaviour()
    {
        playerSpeed = Manager.Instance.playerScript.moveSpeed;
        rb.simulated = true;
        transform.position += TargetDirection().normalized
            * speed
            / playerSpeed
            * Time.deltaTime;
    }

    public override void Init()
    {
        if (isSkull)
        {
            smr.material = LevelLoader.LevelIsInRedLevels(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) ? redLevelSkin : blueLevelSkin;
        }
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return true;
    }

    //Renvoie un offset à droite de Dante qui s'adapte à sa vitesse
    Vector3 TargetDirection()
    {
        //player direction with offset from his speed
        Vector3 xOffset = Vector3.right * playerSpeed *
            //offset more reduced with fewer distance
            (Vector3.Distance(transform.position, Manager.Instance.playerTrsf.position) * .5f + transform.position.y * .5f);
        return Manager.Instance.playerTrsf.position + xOffset * .2f - transform.position;
    }
}
