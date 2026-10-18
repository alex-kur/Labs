namespace Lab3_2;

public class RadiusVector3DComparer : IComparer<RadiusVector3D>
{
    public int Compare(RadiusVector3D? a, RadiusVector3D? b)
    {
        if (a == null && b == null)
            return 0;
        
        if (a == null)
            return -1;
        
        if (b == null)
            return 1;

        return a.CompareTo(b);
    }
}
