using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using STLLoader;

class STLObject
{
    private List<Point3D> lpoints;  //list of vertex in 3D
    private List<Point3D> lnormals; //list of normal values (not normalized)
    private Point3D pCenter;        //the center of the object
    private float fScaleFactor;     //it stores the scale factor to fit into a unit bounding box

    /// <summary>
    /// Simple function who returns the numbers of elements in the list of vertex
    /// </summary>
    public int getNPoints() {return lpoints.Count;}

    /// <summary>
    ///  This the loader function
    /// </summary>
    /// <param name="strFilename">the name of the file</param>
    /// <returns>true if everything is ok, otherwise false</returns>
    public bool ReadSTLObject(string strFilename) 
    {
        try
        {
            using (StreamReader sr = new StreamReader(strFilename))
            {
                char[] delimiterChars = {' ', ',', ':', '\t'}; //the : is useless! but its fun
                String line = sr.ReadLine();
                string[] words = line.Split(delimiterChars);
                if (words.Length < 1) return false;
                Point3D pMin = new Point3D(float.MaxValue);
                Point3D pMax = new Point3D(float.MinValue);
                if (words[0].CompareTo("solid") != 0) throw new Exception("solid"); ;  //its value should be zero
                while((line = sr.ReadLine()) != null)
                {
                    line = line.ToLower().Trim();   //more clean value
                    if (line.CompareTo("endsolid") == 0) break;
                    words = line.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                    if (words[0].CompareTo("facet") != 0 && words[1].CompareTo("normal") != 0) throw new Exception("normal value");
                    Point3D normal = new Point3D(float.Parse(words[2]), float.Parse(words[3]), float.Parse(words[4]));  //this value should be a Vector3D
                    lnormals.Add(normal);
                    line = sr.ReadLine().ToLower().Trim();
                    if (line.CompareTo("outer loop") != 0) throw new Exception("outer loop");
                    Point3D p;
                    for (int k = 0; k < 3; k++) 
                    {
                        line = sr.ReadLine().ToLower().Trim();
                        words = line.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        if (words[0].ToLower().CompareTo("vertex") != 0) throw new Exception("vertex on iteration on" + line);
                        p = new Point3D(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]));
                        if (p.X < pMin.X) pMin.X = p.X; if (p.X > pMax.X) pMax.X = p.X;
                        if (p.Y < pMin.Y) pMin.Y = p.Y; if (p.Y > pMax.Y) pMax.Y = p.Y;
                        if (p.Z < pMin.Z) pMin.Z = p.Z; if (p.Z > pMax.Z) pMax.Z = p.Z;
                        lpoints.Add(p);
                    }
                    if (sr.ReadLine().ToLower().Trim().CompareTo("endloop") != 0) throw new Exception("end loop");
                    if (sr.ReadLine().ToLower().Trim().CompareTo("endfacet") != 0) throw new Exception("end facet");
                }
                //now, calculate the others values
                pCenter = new Point3D(pMin + pMax);
                pCenter /= 2.0f;
                fScaleFactor = 1.0f / Math.Max(Math.Max(pMax.Z - pMin.Z, pMax.Y - pMin.Y), pMax.X - pMin.X);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The following error has ocurred:");
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }

    public void printInfo()
    {
        Console.WriteLine("It has " + getNPoints() + " points");
        Console.WriteLine("The center is (" + pCenter.ToString() + ")");
        Console.WriteLine("The scale factor is " + fScaleFactor);
    }

    // constructor
    public STLObject() 
    {
        lpoints = new List<Point3D>();
        lnormals = new List<Point3D>();
    }

}

namespace STLLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            STLObject some = new STLObject();
            if (some.ReadSTLObject("../../10 huecos.stl"))
            {
                some.printInfo();
            }
        }
    }
}
