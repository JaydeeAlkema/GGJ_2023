using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileSpawnPoints = new List<Transform>();
    [SerializeField] private List<PlayerController> players = new List<PlayerController>();
    [SerializeField] private PlayerController targetingPlayer = null;

    [SerializeField] private BuffIconsScriptableObject icons;

    [SerializeField] private int currentLevel;


    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject buffPrefab;
    private List<GameObject> projectiles;

    private float timeBetweenBuffEvents = 10;
    private float timeBetweenTreeEvents = 3f;

    public PlayerController TargetingPlayer { get => targetingPlayer; set => targetingPlayer = value; }

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
        targetingPlayer = player;
    }

    public void ShootProjectileAtPlayer()
    {
        if (targetingPlayer == null) return;
        Vector3 spawnpos = GetRandomProjectileSpawnPoint();
        GameObject projectile = Instantiate(projectilePrefab, spawnpos, Quaternion.identity);
        projectile.GetComponent<Projectile>().Speed = currentLevel * 10;

        Vector2 direction = (targetingPlayer.transform.position - spawnpos).normalized;
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
            BuffTypes buffType = (BuffTypes)UnityEngine.Random.Range(0, Enum.GetNames(typeof(BuffTypes)).Length);

            switch (buffType)
            {
                case BuffTypes.Water:
                    buffObject.GetComponent<SpriteRenderer>().sprite = icons.speedSprite;
                    break;
                case BuffTypes.Speed:
                    buffObject.GetComponent<SpriteRenderer>().sprite = icons.speedSprite;
                    break;
                case BuffTypes.Knockback:
                    buffObject.GetComponent<SpriteRenderer>().sprite = icons.knockbackSprite;
                    break;
                case BuffTypes.Damage:
                    buffObject.GetComponent<SpriteRenderer>().sprite = icons.damageSprite;
                    break;
                default:
                    break;
            }

            buff.buffType = buffType;
            yield return new WaitForSeconds(timeBetweenBuffEvents);
        }
    }
}
