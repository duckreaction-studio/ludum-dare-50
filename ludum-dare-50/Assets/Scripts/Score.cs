public struct Score
{
    public enum Type
    {
        Perfect,
        Good,
        NotBad,
        Fail
    }

    public Type type;
    public bool perfectHit;
    public float enemyDistanceFromSquare;
}