using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerBaseState<Player>
{
    public void OnEnter(Player player);
    public void OnExecute(Player player);
    public void OnFixedExecute(Player player);
    public void OnExit(Player player);

}
