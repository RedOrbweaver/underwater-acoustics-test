using Godot;
using System;

public partial class SimpleWave : MeshInstance3D
{
	bool dirty = false;
	Vector3[] points;
	Godot.Collections.Array arr;
	
	public override void _Ready()
	{

	}

	public Vector3[] Begin(int radial, int rings)
	{
		SphereMesh mesh = new SphereMesh();
		mesh.Rings = rings;
		mesh.RadialSegments = radial;
		arr = mesh.GetMeshArrays();
		points = (Vector3[])arr[(int)Mesh.ArrayType.Vertex];
		dirty = true;
		return points;
	}
	public void SetPoints(Vector3[] points, Vector3[] speeds, float[] energies)
	{
		this.points = points;
		dirty = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (dirty)
		{
			var mesh = new ArrayMesh();
			arr[(int)Mesh.ArrayType.Vertex] = points;
			mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arr);
			this.Mesh = mesh;
		}
	}
}
