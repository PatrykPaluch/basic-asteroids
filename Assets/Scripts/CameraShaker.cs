using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShaker : MonoBehaviour {

	[SerializeField]
	private Transform transformToShake;
	
	public float shakeTime = 0.5f;
	public float shakeAmplitude = 1;
	public float shakeFrequency = 10;


	private float currShakeTime = float.PositiveInfinity;
	private float currShakeFreqTime = float.PositiveInfinity;

	private float noDefaultAmplitude;
	
	private Vector2 targetOffset;
	private Vector2 previousOffset;
	private Vector2 offset;

	private bool end;
	private void LateUpdate() {

		if (currShakeTime < shakeTime) {
			currShakeTime += Time.deltaTime;


			currShakeFreqTime += Time.deltaTime;
			if (currShakeFreqTime >= 1.0f / shakeFrequency) {
				currShakeFreqTime = 0;

				previousOffset = targetOffset;
				
				float angle = Random.Range(0, 360);
				targetOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
				
				float currentAmplitude = (shakeTime - currShakeTime) * (1 / shakeTime) * noDefaultAmplitude;
				
				targetOffset *= currentAmplitude;
			}

			
			
		}
		else {
			targetOffset = Vector2.zero;
		}


		if (targetOffset == Vector2.zero) {
			offset = Vector2.Lerp(previousOffset, targetOffset, 5 * Time.deltaTime);
			if (offset.sqrMagnitude < 0.1) {
				offset = Vector2.zero;
				currShakeTime = float.PositiveInfinity;
				currShakeFreqTime = float.PositiveInfinity;
				end = true;
				
				transformToShake.localPosition = new Vector3(0, 0, transformToShake.localPosition.z);
			}
		}
		else {
			offset = Vector2.Lerp(previousOffset, targetOffset, currShakeFreqTime / (1 / shakeFrequency));
		}

		if (!end) {
			transformToShake.localPosition = new Vector3(offset.x, offset.y, transformToShake.localPosition.z);
		}

		

	}

	public void Shake(float amplitude = 0, float additionalTime = 0) {
		previousOffset = Vector2.zero;
		currShakeTime = - additionalTime;
		currShakeFreqTime = float.PositiveInfinity;
		end = false;
		noDefaultAmplitude = amplitude == 0 ? shakeAmplitude : amplitude;
	}
}
