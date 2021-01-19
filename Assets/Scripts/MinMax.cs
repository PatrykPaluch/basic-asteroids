
using UnityEngine;

public struct MinMax {

	public readonly float Min;
	public readonly float Max;

	public MinMax(float min, float max) {
		Min = min;
		Max = max;
	}

	public float GetRandomValue() {
		return Random.Range(Min, Max);
	}

}