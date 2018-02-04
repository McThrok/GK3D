using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GK3D.Graphics.Common
{
    public class CollectionItem<T> where T:GameObject
    {
        public T Object { get; set; }
        public Matrix4 GlobalModelMatrix { get; set; }
    }
}
