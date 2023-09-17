using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioDetector : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;
	public float updateStep = 0.1f;
	public int sampleDataLength = 512;
    public float fallingValue = 0.1f;

	private float currentUpdateTime = 0f;

	private float clipLoudness;
	private float[] clipSampleData;

	// Use this for initialization
	void Awake () {
	
		if (!audioSource) {
			Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
		}
		clipSampleData = new float[sampleDataLength];

	}
	
	// Update is called once per frame
	void Update () {
        slider.value -= fallingValue;
        
		currentUpdateTime += Time.deltaTime;
		if (currentUpdateTime >= updateStep) {
			currentUpdateTime = 0f;
			audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
			clipLoudness = 0f;
			foreach (var sample in clipSampleData) {
				clipLoudness += Mathf.Abs(sample);
			}
			clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
            if(clipLoudness <= 0.07){ return; }
            slider.value += Mathf.Round(clipLoudness * 200) / 100f;  
            print(clipLoudness);
		}

	}
}
