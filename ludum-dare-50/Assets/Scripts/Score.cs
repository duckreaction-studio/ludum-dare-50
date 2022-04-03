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

    public int StarCount
    {
        get
        {
            return type switch
            {
                Type.Perfect => 3,
                Type.Good => 2,
                Type.NotBad => 1,
                _ => 0
            };
        }
    }
}