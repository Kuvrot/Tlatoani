using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    Damageable buildingToAttack;

    private void Start()
    {
        FindBuildingToAttack();
    }
    void FindBuildingToAttack()
    {
        buildingToAttack = BuildingManager.instance.GetRandomBuilding().attackable;
        AttackTarget(buildingToAttack);
    }

}
