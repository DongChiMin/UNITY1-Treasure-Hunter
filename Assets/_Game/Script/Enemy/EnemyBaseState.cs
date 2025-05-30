using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyBaseState<Enemy>
{
    public void OnInit(Enemy enemy);
    public void OnEnter();
    public void OnExecute();
    public void OnFixedExecute();
    public void OnExit();
}
