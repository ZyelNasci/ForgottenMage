public static class GameDelegates 
{
    public delegate void PlayerEvent(PlayerController player);
    public static PlayerEvent playerRevive;
    public static PlayerEvent playerDeath;

    public delegate void EnemiesEvent(EnemyBase _enemy);
    public static EnemiesEvent EnemyDeath;

    public static void EnemyDeathEvent(EnemyBase _enemy)
    {
        if (EnemyDeath != null)
            EnemyDeath(_enemy);
    }
}