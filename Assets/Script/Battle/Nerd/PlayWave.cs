using UnityEngine;
using System;
using TMPro;

public class PlayWave : MonoBehaviour
{
	const double PI = System.Math.PI;

	//C～C2までしかないから、その音階だけで曲作る必要がある
	public enum PlayState
	{
		None,
		C,
		CS,
		D,
		DS,
		E,
		F,
		FS,
		G,
		GS,
		A,
		AS,
		B,
		C2,
		Space
	}

	public static double[] freq_PlayState = new double[] 
	{ 
		0,
		261.6255653005985, 
		277.18263097687196, 
		293.66476791740746, 
		311.1269837220808, 
		329.62755691286986,
		349.2282314330038,
		369.99442271163434,
		391.99543598174927,
		415.3046975799451,
		440.0,
		466.1637615180899,
		493.8833012561241,
		523.2511306011974
	};


	public static double Get_Freq(string mScale)
	{
		double freq = 0;
		int count = 0;
		foreach (string playstate in Enum.GetNames(typeof(PlayState)))
		{
			if (playstate == mScale) freq = freq_PlayState[count];
			 count++;
		}

		return freq;
	}

	public double gain = 1.5;
	private double increment;
	private double sampling_frequency = 48000;
	private double time;
	private PlayState playState = PlayState.None;
	private bool nowPlaying = false;
	private float fadeScale;

	void SineWave(float[] data, int channels, double frequency)
	{
		increment = frequency * 2 * PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			time = time + increment;
			data[i] = (float)(gain * Math.Sin(time) * fadeScale);

			if (nowPlaying)
			{
				fadeScale *= 1.5f;
				if (fadeScale > 1.0f) fadeScale = 1.0f;
			}
			else
			{
				fadeScale -= .025f;
				if (fadeScale < 0.001f)
				{
					fadeScale = 0.0f;
					playState = PlayState.None;
				}
			}
			if (channels == 2)
				data[i + 1] = data[i];
			if (time > 2 * Math.PI)
				time = 0;
		}
	}

	public static float freq = 250;
	void OnAudioFilterRead(float[] data, int channels)
	{
		double scale = 1;
		/*
		switch (playState)
		{
			case PlayState.C:
				SineWave(data, channels, 261.6255653005985 * scale);
				break;
			case PlayState.CS:
				SineWave(data, channels, 277.18263097687196 * scale);
				break;
			case PlayState.D:
				SineWave(data, channels, 293.66476791740746 * scale);
				break;
			case PlayState.DS:
				SineWave(data, channels, 311.1269837220808 * scale);
				break;
			case PlayState.E:
				SineWave(data, channels, 329.62755691286986 * scale);
				break;
			case PlayState.F:
				SineWave(data, channels, 349.2282314330038 * scale);
				break;
			case PlayState.FS:
				SineWave(data, channels, 369.99442271163434 * scale);
				break;
			case PlayState.G:
				SineWave(data, channels, 391.99543598174927 * scale);
				break;
			case PlayState.GS:
				SineWave(data, channels, 415.3046975799451 * scale);
				break;
			case PlayState.A:
				SineWave(data, channels, 440.0 * scale);
				break;
			case PlayState.AS:
				SineWave(data, channels, 466.1637615180899 * scale);
				break;
			case PlayState.B:
				SineWave(data, channels, 493.8833012561241 * scale);
				break;
			case PlayState.C2:
				SineWave(data, channels, 523.2511306011974 * scale);
				break;
		}
		*/

		if(playState == PlayState.Space) SineWave(data, channels, freq * scale);
	}

	void Play(PlayState playState_)
	{
		if (playState == PlayState.None)
		{
			fadeScale = 0.1f;
			time = 0.0f;
		}
		playState = playState_;
		nowPlaying = true;
	}

	

	void Update()
	{
		nowPlaying = false;

		if (Input.GetKey(KeyCode.Space) || is_touched)
		{
			Play(PlayState.Space);
		}
        else
        {
			nottouch_score += (int)(Time.deltaTime * 100);
			nottouch_score_text.text = nottouch_score.ToString();
		}
		Score_Manager.Overwrite_Score(touchsuccess_score + nottouch_score);

		/*
		if (Input.GetKey("z"))
		{
			Play(PlayState.C);
		}
		if (Input.GetKey("s"))
		{
			Play(PlayState.CS);
		}
		if (Input.GetKey("x"))
		{
			Play(PlayState.D);
		}
		if (Input.GetKey("d"))
		{
			Play(PlayState.DS);
		}
		if (Input.GetKey("c"))
		{
			Play(PlayState.E);
		}
		if (Input.GetKey("f"))
		{
			Play(PlayState.F);
		}
		if (Input.GetKey("v"))
		{
			Play(PlayState.FS);
		}
		if (Input.GetKey("b"))
		{
			Play(PlayState.G);
		}
		if (Input.GetKey("h"))
		{
			Play(PlayState.GS);
		}
		if (Input.GetKey("n"))
		{
			Play(PlayState.A);
		}
		if (Input.GetKey("j"))
		{
			Play(PlayState.AS);
		}
		if (Input.GetKey("m"))
		{
			Play(PlayState.B);
		}
		if (Input.GetKey(","))
		{
			Play(PlayState.C2);
		}
		*/
	}



	//デバッグ用
	bool is_touched = false;
	[SerializeField] GameObject cursor;
	[SerializeField] TextMeshProUGUI touchsuccess_score_text;
	[SerializeField] TextMeshProUGUI nottouch_score_text;
	int touchsuccess_score = 0;
	int nottouch_score = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
		if (!Input.GetKey(KeyCode.Space)) return;


		if (collision.transform.localPosition.y - 25f <= cursor.transform.localPosition.y && cursor.transform.localPosition.y <= collision.transform.localPosition.y + 25f)
		{
			Sub_Beat_Action.Show_Judge_Text(type: 1, new Vector2(-700f, 500f));
			touchsuccess_score += (int)(Time.deltaTime * 200);
			touchsuccess_score_text.text = touchsuccess_score.ToString();
		}
		else if (collision.transform.localPosition.y - 50f <= cursor.transform.localPosition.y && cursor.transform.localPosition.y <= collision.transform.localPosition.y + 50f)
		{
			Sub_Beat_Action.Show_Judge_Text(type: 2, new Vector2(-700f, 500f));
			touchsuccess_score += (int)(Time.deltaTime * 100);
			touchsuccess_score_text.text = touchsuccess_score.ToString();
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{
		is_touched = true;
		freq = collision.transform.localPosition.y;
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		is_touched = false;
    }
}