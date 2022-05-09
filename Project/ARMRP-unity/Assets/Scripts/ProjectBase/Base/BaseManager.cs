public class BaseManager<T> where T : new()
{
    private static T instate;

    public static T Instate
    {
        get
        {
            return Getinstate();
        }
    }

    private static T Getinstate()
    {
        if (instate == null)
        {
            instate = new T();
        }
        return instate;
    }
}