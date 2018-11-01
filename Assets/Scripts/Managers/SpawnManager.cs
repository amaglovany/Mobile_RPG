using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private bool autoSpawner;

    private uint deadBodyDeleteDuration;

    [SerializeField] private Transform zombiesParentGO;
    public GameObject enemyZombie;
    public Transform[] spawnPointsTransforms;

    public void SpawnEnemies(GameObject enemyPrefab, uint count)
    {
        for (var i = 0; i < count; i++)
            if (spawnPointsTransforms[i] != null)
            {
                var enemy = Instantiate(enemyPrefab, spawnPointsTransforms[i].position,
                    spawnPointsTransforms[i].rotation);
                SetEnemyGOParent(enemy);
            }
    }

    public void SetDeadBodyDeleteDuration(uint durationInSeconds)
    {
        deadBodyDeleteDuration = durationInSeconds;
    }

    public uint GetDeadBodyDeleteDuration()
    {
        return deadBodyDeleteDuration;
    }

    public int GetCurrentEnemyQuantityOnMap(GameObject enemyPrefab)
    {
        return GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Count();
    }

    public bool CheckForEnemies(GameObject enemyPrefab)
    {
        return GetCurrentEnemyQuantityOnMap(enemyPrefab) > 1;
    }

    public void SpawnEnemies(GameObject enemyPrefab, float delay, bool message)
    {
        StartCoroutine(SpawnWithMessageIfNoEnemiesRoutine(enemyPrefab, delay, message));
    }

    private IEnumerator SpawnWithMessageIfNoEnemiesRoutine(GameObject enemyPrefab, float delay, bool message)
    {
        yield return new WaitForSeconds(delay);

        if (message)
        {
            GameplayUI.Instance.DisplayMessage(Messages.messageNoEnemies, Colors.yellowMessage, 3f, false);
            yield return new WaitForSeconds(3f);
        }

        if (autoSpawner)
        {
            AutoSpawn(enemyPrefab);
        }
        else
        {
            SpawnEnemies(enemyPrefab, (uint) spawnPointsTransforms.Length);
        }
    }

    private void SetEnemyGOParent(GameObject spawnedEnemyGO)
    {
        Transform parentGO = null;

        switch (spawnedEnemyGO.GetComponent<HealthEnemy>().enemyType)
        {
            case ContainerTypeEnum.Enum.Zombie:
                parentGO = zombiesParentGO;
                break;
        }

        if (parentGO != null)
            spawnedEnemyGO.transform.SetParent(parentGO);
    }

    private void GiveEnemyRandomWeapon(GameObject enemy)
    {
        var healthEnemy = enemy.GetComponentInChildren<HealthEnemy>();

        if (!healthEnemy.randomWeapon)
        {
            return;
        }

        // Clearing equipped weapon
        foreach (Transform child in healthEnemy.enemyWeaponParentTransform)
        {
            Destroy(child.gameObject);
        }

        // Generating random weapon data and assigning it's elements to enemy
        Weapon newWeapon = null;

        if (healthEnemy.awakeWeapon)
        {
            newWeapon = healthEnemy.awakeWeapon;
        }
        else
        {
            newWeapon = (Weapon) LootGenerator.Instance.GenerateRandomWeapon();
        }

        GameObject newWeaponGO = Instantiate(newWeapon.itemActivePrefab, healthEnemy.enemyWeaponParentTransform);

        healthEnemy.locomotion.SetIndexWeaponAnimator(
            newWeaponGO.GetComponent<WeaponData>().weaponData.animatorIDWeapon);
        healthEnemy.enemyWeaponData = newWeaponGO.GetComponent<WeaponData>();
        healthEnemy.enemyWeaponData.weaponData = newWeaponGO.GetComponent<WeaponData>().weaponData;
    }

    private void GiveAllEnemiesRandomWeapons(GameObject enemyPrefab)
    {
        foreach (var enemyGO in GameObject.FindGameObjectsWithTag(enemyPrefab.tag))
        {
            GiveEnemyRandomWeapon(enemyGO);
        }
    }

    #region Singleton

    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region AutoSpawner

    private void Start()
    {
        if (autoSpawner)
        {
            GetStartTransforms(enemyZombie);
            GiveAllEnemiesRandomWeapons(enemyZombie);
        }
    }

    private GameObject[] allStartEnemies;
    private readonly List<Vector3> autoSpawnPositions = new List<Vector3>();
    private readonly List<Quaternion> autoSpawnRotations = new List<Quaternion>();

    public void GetStartTransforms(GameObject enemyPrefab)
    {
        SetDeadBodyDeleteDuration(CheatManager.Instance.FAST_TESTING ? (uint) 0 : (uint) 5);

        allStartEnemies = GameObject.FindGameObjectsWithTag(enemyPrefab.tag);

        foreach (GameObject enemy in allStartEnemies)
        {
            autoSpawnPositions.Add(enemy.transform.position);
            autoSpawnRotations.Add(enemy.transform.rotation);
        }
    }

    public void AutoSpawn(GameObject enemyPrefab)
    {
        for (var i = 0; i < allStartEnemies.Length; i++)
        {
            var enemy = Instantiate(enemyPrefab, autoSpawnPositions[i], autoSpawnRotations[i]);
            GiveAllEnemiesRandomWeapons(enemyPrefab);
            SetEnemyGOParent(enemy);
        }
    }

    #endregion
}