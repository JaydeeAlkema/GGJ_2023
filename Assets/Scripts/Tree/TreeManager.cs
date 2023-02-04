using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileSpawnPoints = new List<Transform>();
    [SerializeField] private List<PlayerController> playerTransforms = new List<PlayerController>();
    [SerializeField] private PlayerController currentSidePlayer = null;

    [SerializeField] private int currentLevel;


    [SerializeField] private GameObject projectilePrefab;
    private List<GameObject> projectiles;
    private float timeBetweenTreeEvents = 1f;



    public void Awake()
    {
        if (playerTransforms.Count == 0)
        {
            Debug.Log("Tree found no players");
        }
        else
        {
            Debug.Log("Tree found " + playerTransforms.Count + " players");
        }
    }

    public void ChangeSide(PlayerController player)
    {
        currentSidePlayer = player;
    }

    public void ShootProjectileAtPlayer()
    {
        if (currentSidePlayer == null) return; 

        GameObject projectile = Instantiate(projectilePrefab, GetRandomProjectileSpawnPoint(), Quaternion.identity);
        projectile.GetComponent<Projectile>().Speed = currentLevel * 10;


        Vector2 direction = (currentSidePlayer.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    public Vector3 GetRandomProjectileSpawnPoint()
    {
        int r = Random.Range(0, projectileSpawnPoints.Count);
        return projectileSpawnPoints[r].position;
    }

    public IEnumerator treeEventTimer()
    {
        while (true)
        {
            ShootProjectileAtPlayer();
            yield return new WaitForSeconds(timeBetweenTreeEvents);
        }
    }
}
