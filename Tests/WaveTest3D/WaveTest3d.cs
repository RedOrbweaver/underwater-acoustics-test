using Godot;
using System;

public partial class WaveTest3d : Node3D
{
	Node3D _source;
	Pool _pool;
	SimpleWave _wave;
	float tot_energy = 0;
	float[] energies;
	Vector3[] speeds;
	Vector3[] vertices;
	// Called when the node enters the scene tree for the first time.
	float Tick(float delta)
	{
		Assert(energies.Length == speeds.Length);
		Assert(vertices.Length == speeds.Length);
		float total_energy = 0;
		for (int i = 0; i < vertices.Length; i++)
		{
			float energy = energies[i];
			if (energy == 0)
				continue;
			Vector3 dif = speeds[i];
			Vector3 npos = vertices[i] + dif;
			bool bounced = false;
			for (int ii = 0; ii < 3; ii++)
			{
				if (npos[ii] >= _pool.Scale[ii] / 2 || npos[ii] <= -_pool.Scale[ii] / 2)
				{
					dif[ii] = -dif[ii];
					bounced = true;
				}
			}
			speeds[i] = dif;
			vertices[i] = vertices[i] + dif;
			if (bounced)
				energy /= 2;
			energy -= delta * 0.01f;
			if (energy < 0.001)
				energy = 0;
			energies[i] = energy;
			total_energy += energy;
		}
		return total_energy;
	}
	public override void _Ready()
	{
		base._Ready();

		_pool = this.GetNodeSafe<Pool>("Pool");
		_source = this.GetNodeSafe<Node3D>("Source");
		_wave = this.GetNodeSafe<SimpleWave>("Source/SimpleWave");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);

		if (tot_energy == 0)
		{
			vertices = _wave.Begin(8, 8);
			speeds = vertices.ForEach((v) =>
			{
				return v * 0.01f;
			});
			energies = vertices.ForEach((v) =>
			{
				return 1.0f;
			});
			tot_energy = energies.Sum();
		}
		else
		{
			tot_energy = Tick((float)delta);
			_wave.SetPoints(vertices, speeds, energies);
		}
		
	}
}
