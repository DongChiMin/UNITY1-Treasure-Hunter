using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerBaseState<Player>
{
    public void OnInit(Player player);
    public void OnEnter();
    public void OnExecute();
    public void OnFixedExecute();
    public void OnExit();

}
