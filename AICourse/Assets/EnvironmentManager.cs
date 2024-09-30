using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;
    [SerializeField] List<Transform> healthPacks= new List<Transform>();
    [SerializeField] List<EnemyBehavior> enemies= new List<EnemyBehavior>();

    public List<Transform> HealthPacks => healthPacks;
    public List<EnemyBehavior> Enemies => enemies;

    private void Awake()
    {
        Instance = this;
    }

    public void Respawn()
    {
        for (int i = 0; i < healthPacks.Count; i++)
        {
            healthPacks[i].gameObject.SetActive(true);
        }

        for (int i = 0;i < enemies.Count; i++)
        {
            enemies[i].Respawn();
        }
    }

}
