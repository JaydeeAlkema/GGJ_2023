using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileSpawnPoints = new List<Transform>();
    [SerializeField] private List<PlayerController> players = new List<PlayerController>();
    [SerializeField] private PlayerController currentSidePlayer = null;

    [SerializeField] private int currentLevel;


    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject buffPrefab;
    private List<GameObject> projectiles;

    private float timeBetweenBuffEvents = 5;
    private float timeBetweenTreeEvents = 1f;


    public void Awake()
    {
        StartCoroutine(treeEventTimer());
        StartCoroutine(buffSpawnEventTimer());
    }
    public void AddPlayerTransform(Transform transform)
    {
        players.Add(transform.GetComponent<PlayerController>());
    }

    public void ChangeSide(PlayerController player)
    {
        currentSidePlayer = player;
    }

    public void ShootProjectileAtPlayer()
    {
        if (currentSidePlayer == null) return;
        Vector3 spawnpos = GetRandomProjectileSpawnPoint();
        GameObject projectile = Instantiate(projectilePrefab, spawnpos, Quaternion.identity);
        projectile.GetComponent<Projectile>().Speed = currentLevel * 10;

        Vector2 direction = (currentSidePlayer.transform.position - spawnpos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }


    public Vector3 GetRandomProjectileSpawnPoint()
    {
        int r = UnityEngine.Random.Range(0, projectileSpawnPoints.Count);
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

    public IEnumerator buffSpawnEventTimer()
    {
        while (true)
        {
            Vector3 spawnPos = GetRandomProjectileSpawnPoint();
            GameObject buffObject = Instantiate(buffPrefab, spawnPos, Quaternion.identity);
            BuffCollectable buff = buffObject.GetComponent<BuffCollectable>();
            buff.buffType = (BuffTypes)UnityEngine.Random.Range(0, Enum.GetNames(typeof(BuffTypes)).Length);
            yield return new WaitForSeconds(timeBetweenBuffEvents);
        }
    }
}
