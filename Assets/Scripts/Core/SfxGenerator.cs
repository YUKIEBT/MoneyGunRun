using UnityEngine;

// 責務: 音素材がない場合に、プログラムで「ピュン」という音を作る
public class SfxGenerator : MonoBehaviour
{
    public static SfxGenerator Instance;

    // 生成した音データを保持する変数
    public AudioClip ShootClip { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // 音を作る（長さ0.1秒、周波数44100Hz）
        ShootClip = CreatePewSound(0.1f, 44100);
    }

    private AudioClip CreatePewSound(float length, int frequency)
    {
        int sampleCount = (int)(frequency * length);
        float[] samples = new float[sampleCount];

        // 波形を作る（高い音から低い音へ急激に下がる＝ピュン音）
        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleCount; // 0.0 〜 1.0 の進行度

            // 周波数を変化させる（1500Hz -> 0Hz）
            float waveFreq = 1500f * (1f - t);

            // サイン波の計算
            samples[i] = Mathf.Sin(2 * Mathf.PI * waveFreq * (float)i / frequency);

            // 音量を減衰させる（最後は無音に）
            samples[i] *= (1f - t);
        }

        AudioClip clip = AudioClip.Create("ProceduralPew", sampleCount, 1, frequency, false);
        clip.SetData(samples, 0);
        return clip;
    }
}