using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STLLoader
{
    class Point3D
    {
        private float x, y, z;
        //properties
        public float X
        {
            get
            {
                return x;
            }
            set 
            {
                x = value;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public float Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }
        
        //functions
        public Point3D() 
        {
            this.x = this.y = this.z = 0;
        }
        public Point3D(float v)
        {
            this.x = this.y = this.z = v;
        }
        public Point3D(Point3D p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }

        public Point3D(float x, float y, float z) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public static Point3D operator + (Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
        
        public static Point3D operator / (Point3D p, float f)
        {
            return new Point3D(p.X / f, p.Y / f, p.Z / f);
        }

        public override string ToString()
        {
            return x + "," + y + "," + z;
        }
    }
}
