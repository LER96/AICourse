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

}
