public class BuildRequest
{
    public int Rotation { private set; get; }

    public Placeable Placeable { private set; get; }

    public BuildRequest(Placeable type, int rotation = 0)
    {
        Rotation = rotation;
        Placeable = type;
    }

    public enum PlaceableType
    {
        ROAD,
        BUILDING
    }
}