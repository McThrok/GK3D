using GK3D.Graphics.Common;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.Objects
{
    public class ObjVolume : Primitive
    {

        public List<Tuple<FaceVertex, FaceVertex, FaceVertex>> faces = new List<Tuple<FaceVertex, FaceVertex, FaceVertex>>();

        public override int VertCount { get => faces.Count * 3; }
        public override int IndiceCount { get => VertCount; }
        public override int ColorDataCount { get => VertCount; }
        public override int TextureCoordsCount { get => VertCount; }
        public override int NormalCount { get => VertCount; }
        public override int[] GetIndices(int offset = 0)
        {
            return Enumerable.Range(offset, IndiceCount).ToArray();
        }

        public Vector3[] ColorData { get; set; }

        public override Vector3[] GetVerts()
        {
            List<Vector3> verts = new List<Vector3>();

            foreach (var face in faces)
            {
                verts.Add(face.Item1.Position);
                verts.Add(face.Item2.Position);
                verts.Add(face.Item3.Position);
            }

            return verts.ToArray();
        }
        public override Vector2[] GetTextureCoords()
        {
            List<Vector2> coords = new List<Vector2>();
            foreach (var face in faces)
            {
                coords.Add(face.Item1.TextureCoord);
                coords.Add(face.Item2.TextureCoord);
                coords.Add(face.Item3.TextureCoord);
            }

            return coords.ToArray();
        }
        public override Vector3[] GetNormals()
        {
            List<Vector3> normals = new List<Vector3>();

            foreach (var face in faces)
            {
                normals.Add(face.Item1.Normal);
                normals.Add(face.Item2.Normal);
                normals.Add(face.Item3.Normal);
            }

            return normals.ToArray();
        }
        public override Vector3[] GetColorData()
        {
            return ColorData ?? new Vector3[ColorDataCount];
        }

        public static ObjVolume LoadFromFile(string filename)
        {
            ObjVolume obj = new ObjVolume();
            try
            {
                using (StreamReader sr = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(sr.ReadToEnd());
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine("File not found: {0}", filename);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading file: {0}", filename);
            }

            return obj;
        }
        public static ObjVolume LoadFromString(string obj)
        {
            // Seperate lines from the file
            List<String> lines = new List<string>(obj.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<FaceInd> faceInds = new List<FaceInd>();

            // Base values
            verts.Add(new Vector3());
            texs.Add(new Vector2());
            normals.Add(new Vector3());

            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success |= float.TryParse(vertparts[1], out vec.Y);
                        success |= float.TryParse(vertparts[2], out vec.Z);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Debug.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error parsing vertex: {0}", line);
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("vt ")) // Texture coordinate
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector2 vec = new Vector2();

                    if (temp.Trim().Count((char c) => c == ' ') > 0) // Check if there's enough elements for a vertex
                    {
                        String[] texcoordparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(texcoordparts[0], out vec.X);
                        success |= float.TryParse(texcoordparts[1], out vec.Y);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Debug.WriteLine("Error parsing texture coordinate: {0}", line);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error parsing texture coordinate: {0}", line);
                    }

                    texs.Add(vec);
                }
                else if (line.StartsWith("vn ")) // Normal vector
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a normal
                    {
                        String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success |= float.TryParse(vertparts[1], out vec.Y);
                        success |= float.TryParse(vertparts[2], out vec.Z);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Debug.WriteLine("Error parsing normal: {0}", line);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error parsing normal: {0}", line);
                    }

                    normals.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    //Tuple<FaceVertexInd, FaceVertexInd, FaceVertexInd> face = new Tuple<FaceVertexInd, FaceVertexInd, FaceVertexInd>(new FaceVertexInd(), new FaceVertexInd(), new FaceVertexInd());
                    FaceInd face = new FaceInd();
                    if (temp.Trim().Count((char c) => c == ' ') >= 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        bool success = true;

                        foreach (var facepart in faceparts)
                        {
                            int v, t, n;
                            success |= int.TryParse(facepart.Split('/')[0], out v);
                            if (faceparts[0].Count((char c) => c == '/') >= 2)
                            {
                                success |= int.TryParse(facepart.Split('/')[1], out t);
                                success |= int.TryParse(facepart.Split('/')[2], out n);
                            }
                            else
                            {
                                t = texs.Count > v ? v : 0;
                                n = normals.Count > v ? v : 0;
                            }
                            face.Vertices.Add(new FaceVertexInd(v, n, t));
                        }

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Debug.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            faceInds.Add(face);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error parsing face: {0}", line);
                    }
                }
            }

            // Create the ObjVolume
            ObjVolume vol = new ObjVolume();

            foreach (var faceInd in faceInds)
            {

                if (faceInd.Vertices.Count >= 3)
                {
                    var start = new FaceVertex(verts[faceInd.Vertices[0].Vertex], normals[faceInd.Vertices[0].Normal], texs[faceInd.Vertices[0].Texcoord]);
                    var prev = new FaceVertex(verts[faceInd.Vertices[1].Vertex], normals[faceInd.Vertices[1].Normal], texs[faceInd.Vertices[1].Texcoord]);
                    foreach (var faceVertexInd in faceInd.Vertices.Skip(2))
                    {
                        var current = new FaceVertex(verts[faceVertexInd.Vertex], normals[faceVertexInd.Normal], texs[faceVertexInd.Texcoord]);
                        vol.faces.Add(new Tuple<FaceVertex, FaceVertex, FaceVertex>(start, prev, current));
                        prev = current;

                    }

                }
            }

            return vol;
        }

        public ObjVolume Clone()
        {
            ObjVolume clone = new ObjVolume();
            clone.faces =  faces.Select(x => new Tuple<FaceVertex, FaceVertex, FaceVertex>(x.Item1, x.Item2, x.Item3)).ToList();
            clone.Position = Position;
            clone.Scale = Scale;
            clone.Rotation = Rotation;
            clone.Material = Material;
            clone.IsTextured = IsTextured;
            clone.TextureID = TextureID;
            clone.Normals = Normals;
            clone.ColorData = ColorData.ToArray();

            return clone;
        }
    }
}
