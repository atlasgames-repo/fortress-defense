namespace Assets.FantasyMonsters.Scripts
{
    /// <summary>
    /// Monster state. The same parameter controls animation transitions.
    /// </summary>
    public enum MonsterState
    {
        Idle 	= 0,
        Ready 	= 1,
        Walk 	= 2,
        Run 	= 3,
        Death   = 9,
        Custom  = 4
    }
}