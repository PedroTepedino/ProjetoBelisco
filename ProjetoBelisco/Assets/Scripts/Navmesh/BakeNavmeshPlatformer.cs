using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

[ExecuteAlways]
public class BakeNavmeshPlatformer : MonoBehaviour
{
    private List<Collider2D> _colliders;
    private List<Vector2> _centers;
    private List<Mesh> _meshes;

    private List<List<Tuple<Vector2, Vector2>>> _worldLines;

    [SerializeField] private float _permitedAngle = 45f;
    [SerializeField] private float _agentHeight = 1.0f;
    [SerializeField] private float _agentWidth = 1.0f;

    [Button]
    public void Bake()
    {
        _colliders = new List<Collider2D>(FindObjectsOfType<Collider2D>());

        for (var index = 0; index < _colliders.Count; index++)
        {
            Collider2D col = _colliders[index];
            if (!col.gameObject.isStatic || !col.gameObject.activeInHierarchy || col.usedByComposite)
            {
                _colliders.Remove(col);
            }
        }

        _meshes = new List<Mesh>();

        foreach (var col in _colliders)
        {
            Mesh mesh = col.CreateMesh(false, useBodyRotation: false);
            _meshes.Add(mesh);
        }

        _centers = new List<Vector2>();
        
        foreach (var mesh in _meshes)
        {
            Vector2 center = Vector2.zero;

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                center += (Vector2)mesh.vertices[i];
            }

            center /= mesh.vertices.Length;
            _centers.Add(center);
        }
        
        _worldLines = new List<List<Tuple<Vector2, Vector2>>>();

        foreach (var mesh in _meshes)
        {
            List<Vector3Int> triangles = new List<Vector3Int>();
            for (int i = 0; i < mesh.triangles.Length / 3; i++)
            {
                Vector3Int aux = new Vector3Int(mesh.triangles[i * 3], mesh.triangles[(i * 3) + 1],
                    mesh.triangles[(i * 3) + 2]);
                triangles.Add(aux);
            }
            
            Vector2Int[] lines = CheckMachingLineTriangles(triangles);
            lines = OrderLinePoints(lines);


            List<Vector2Int> walkableLines = new List<Vector2Int>();

            foreach (var line in lines)
            {
                if (IsWalkableLine(line, mesh))
                {
                    walkableLines.Add(line);
                }
            }
            
            _worldLines.Add(CalculateWorldLines(walkableLines.ToArray(), mesh));
        }

        CalculateConnectedLines();
        
        OffsetHeightWorldLines();
        
        //WidthOffsetLines();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (_centers != null)
        {
            foreach (var center in _centers)
            {
                Gizmos.DrawWireSphere(center, 0.1f);
            }

            foreach (var mesh in _meshes)
            {
                Gizmos.color = Color.grey;

                System.Collections.Generic.List<Vector3Int> triangles = new System.Collections.Generic.List<Vector3Int>();
                for (int i = 0; i < mesh.triangles.Length/3; i++)
                {
                    Vector3Int aux = new Vector3Int(mesh.triangles[i * 3], mesh.triangles[(i* 3) + 1], mesh.triangles[(i*3) + 2]);
                    triangles.Add(aux);
                    Drawtriangle(aux, mesh);
                }
                
                Gizmos.color = Color.blue;

                Vector2Int[] lines = CheckMachingLineTriangles(triangles);
                lines = OrderLinePoints(lines);

                foreach (var line in lines)
                {
                    PlotNormal(line, GetNormal(line, mesh) * 0.2f, mesh);
                }

                System.Collections.Generic.List<Vector2Int> walkableLines = new System.Collections.Generic.List<Vector2Int>();

                foreach (var line in lines)
                {
                    if (IsWalkableLine(line, mesh))
                    {
                        walkableLines.Add(line);
                        this.DrawLine(line, mesh);       
                    }
                }
            }
        }

        if (_worldLines != null)
        {
            Gizmos.color = Color.green;
            
            foreach (var lines in _worldLines)
            {
                foreach (var line in lines)
                {
                    this.DrawLine(line);
                }
            }
            
            Gizmos.color = Color.red;
            
            CheckLineVertexesForCollisions();
        }
    }

    private void Drawtriangle(Vector3 triangle, Mesh mesh)
    {
        Gizmos.DrawLine(mesh.vertices[(int)triangle.x],mesh.vertices[(int)triangle.y]);
        Gizmos.DrawLine(mesh.vertices[(int)triangle.y],mesh.vertices[(int)triangle.z]);
        Gizmos.DrawLine(mesh.vertices[(int)triangle.z],mesh.vertices[(int)triangle.x]);
    }

    private Vector2Int[] CheckMachingLineTriangles(System.Collections.Generic.List<Vector3Int> triangles)
    {
        int count = 0;
        System.Collections.Generic.List<Vector2Int> Lines = new System.Collections.Generic.List<Vector2Int>();
        bool aux = false;
        foreach (var tri1 in triangles)
        {
            for (int i = 0; i < 3; i++)
            {
                aux = false;
                foreach (var tri2 in triangles)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((tri1[i] == tri2[j == 2 ? 0 : j + 1] &&
                            tri1[i == 2 ? 0 : i + 1] == tri2[j]))
                        {
                            aux = true;
                        }

                        count++;
                    }
                }
                if (!aux)
                {
                    Lines.Add(new Vector2Int(tri1[i], tri1[i == 2 ? 0 : i + 1]));
                }
            }
        }
        return Lines.ToArray();
    }

    private Vector2Int[] OrderLinePoints( Vector2Int[] lines)
    {
        void Swap(int line1, int line2)
        {
            Vector2Int aux = lines[line1];
            lines[line1] = lines[line2];
            lines[line2] = aux;
        }
        
        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (lines[i].y == lines[i + 1].x) continue;
            
            for (int j = i; j < lines.Length; j++)
            {
                if (lines[i].y == lines[j].x)
                {
                    Swap(i + 1, j);
                }
            }
        }

        return lines;
    }

    private void PlotNormal(Vector2Int line, Vector2 normal, Mesh mesh)
    {
        Vector2 aux = (mesh.vertices[line.x] + mesh.vertices[line.y]) / 2f;
        Gizmos.DrawLine(aux, aux + normal);
    }

    private Vector2 GetNormal(Vector2Int line, Mesh mesh)
    {
        Vector2 aux = mesh.vertices[line.y] - mesh.vertices[line.x];
        return Vector2.Perpendicular(aux.normalized);
    }

    private bool IsWalkableLine(Vector2Int line, Mesh mesh)
    {
        return Vector2.Angle(GetNormal(line, mesh), Vector2.up) <= _permitedAngle;
    }

    private void DrawLine(Vector2Int line , Mesh mesh)
    {
         DrawLine(Tuple.Create((Vector2)mesh.vertices[line.x], (Vector2)mesh.vertices[line.y]));
    }

    private void DrawLine(Tuple<Vector2, Vector2> line)
    {
        Gizmos.DrawLine(line.Item1, line.Item2);
    }

    private List<Tuple<Vector2, Vector2>> CalculateWorldLines(Vector2Int[] walkableLines, Mesh mesh)
    {
       List<Tuple<Vector2, Vector2>> lines = new List<Tuple<Vector2, Vector2>>();

        foreach (var line in walkableLines)
        {
            lines.Add(Tuple.Create((Vector2)mesh.vertices[line.x], (Vector2)mesh.vertices[line.y]));
        }

        return lines;
    }

    private void CalculateConnectedLines()
    {
        List<Tuple<Vector2, Vector2>>[] worldLines = new List<Tuple<Vector2, Vector2>>[_worldLines.Count];
        _worldLines.CopyTo(worldLines);

        List<List<Tuple<Vector2, Vector2>>> newWorldLines = new List<List<Tuple<Vector2, Vector2>>>();
        
        foreach (var lines in worldLines)
        {
            List<List<Tuple<Vector2, Vector2>>> connectedLines = new List<List<Tuple<Vector2, Vector2>>>();
            
            foreach (var line in lines)
            {
                bool lineAlreadyExists = false;
                
                foreach (var connectedLine in connectedLines)
                {
                    foreach (var cLine in connectedLine.Where(cLine => line.Item1 == cLine.Item2 || line.Item2 == cLine.Item1))
                    {
                        lineAlreadyExists = true;
                    }

                    if (lineAlreadyExists)
                    {
                        connectedLine.Add(line);
                        break;
                    }
                }

                if (!lineAlreadyExists)
                {
                    var aux = new List<Tuple<Vector2, Vector2>>();
                    aux.Add(line);
                    connectedLines.Add(aux);
                }
            }

            newWorldLines.AddRange(connectedLines);
        }

        _worldLines = newWorldLines;
    }

    private void OffsetHeightWorldLines()
    {
        foreach (var lines in _worldLines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                lines[i] = Tuple.Create(lines[i].Item1 + (Vector2.up * (_agentHeight / 2f)), 
                    lines[i].Item2 + (Vector2.up * (_agentHeight / 2f)));
            }
        }
    }

    private void WidthOffsetLines()
    {
        foreach (var lines in _worldLines)
        {
            lines[0] = Tuple.Create(lines[0].Item1  - ((lines[0].Item1 - lines[0].Item2).normalized * (_agentWidth / 2f)) , 
                lines[0].Item2);
            lines[lines.Count - 1] = Tuple.Create(lines[lines.Count - 1].Item1, 
                lines[lines.Count - 1].Item2 -((lines[lines.Count - 1].Item2 - lines[lines.Count - 1].Item1).normalized 
                                               * (_agentWidth / 2f)));
        }
    }


    private void CheckLineVertexesForCollisions()
    {
        var aux = new List<Tuple<Vector2, Vector2>>();
        foreach (var lines in _worldLines)
        {
            foreach (var line in lines)
            {
                float angle = Vector2.SignedAngle(Vector2.up, line.Item2 - line.Item1);
                Vector2 orthogonal = Vector2.Perpendicular(line.Item2 - line.Item1).normalized;
                
                if (IsCollinding(line.Item1, orthogonal, angle) ||
                        IsCollinding(line.Item2, orthogonal, angle))
                {
                    aux.Add(line);
                }
            }
        }
        
        foreach (var line in aux)
        {
            this.DrawLine(line);
        }
        
        Gizmos.color = Color.magenta;

        foreach (var line in aux)
        {
            float angle = Vector2.SignedAngle(Vector2.up, line.Item2 - line.Item1);
            Vector2 orthogonal = Vector2.Perpendicular(line.Item2 - line.Item1).normalized;
            
            Gizmos.DrawLine(line.Item1, line.Item1 + orthogonal);
            Gizmos.DrawLine(line.Item2, line.Item2 + orthogonal);

            var collision = GetCollision(line.Item1, orthogonal, angle);
            
            if (collision.Length > 0)
            {
                foreach (var hit in collision)
                {
                    Gizmos.DrawWireSphere(hit.point, 0.1f);
                }
            }
            
            collision = GetCollision(line.Item2, orthogonal, angle);
            
            if (collision.Length > 0)
            {
                foreach (var hit in collision)
                {
                    Gizmos.DrawWireSphere(hit.point, 0.1f);
                }
            }
        }
    }

    private bool IsCollinding(Vector2 point, Vector2 up, float angle)
    {
        return Physics2D.OverlapBox(point + (up * 0.3f), new Vector2(_agentWidth, _agentHeight), angle) != null;
    }

    private RaycastHit2D[] GetCollision(Vector2 point, Vector2 up, float angle)
    {
        ExtDebug.DrawBoxCastBox(point + (up *0.3f), new Vector3(_agentWidth/2f, _agentHeight/2f, 0.0f), Quaternion.Euler(new Vector3(0, 0, angle)),up, 0f, Color.green);
        return Physics2D.BoxCastAll(point + (up * 0.3f), new Vector2(_agentWidth, _agentHeight), angle, up, 0f);
    }
}