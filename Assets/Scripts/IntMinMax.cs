using UnityEngine;

public struct IntMinMax {

	public readonly int Min;
	public readonly int Max;

	public IntMinMax(int min, int max) {
		Min = min;
		Max = max;
	}

	public int GetRandomValue() {
		return Mathf.RoundToInt(Random.Range(Min - 0.5f, Max + 0.5f));
	}

}