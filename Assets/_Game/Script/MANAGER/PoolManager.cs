using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public ObjectPooling poolJump;
    public ObjectPooling poolGhost;
    public ObjectPooling poolGround;
    public ObjectPooling poolRun;
    public ObjectPooling poolAttack1;
    public ObjectPooling poolAttack2;
    public ObjectPooling poolAttack3;
    public ObjectPooling poolAirAttack1;
    public ObjectPooling poolAirAttack2;
    public ObjectPooling poolSwordThrowPool;
    public ObjectPooling poolSwordEmbeddedParticlePool;
    public ObjectPooling poolSwordItem;
    public ObjectPooling poolCannonBall;
    public ObjectPooling poolCannonFireEffect;
}
