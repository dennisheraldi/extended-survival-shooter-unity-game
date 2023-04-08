using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        // jika pemain menekan tombol spasi, skip ke 5 detik terakhir sebelum cutscene berakhir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            double timeLeft = director.duration - director.time;
            if (timeLeft > 6f) // jika sisa waktu lebih dari 5 detik, skip ke 5 detik terakhir
            {
                director.time += 2f;
            }
            else // jika sisa waktu kurang dari atau sama dengan 5 detik, skip ke akhir cutscene
            {
                // do nothing
            }
        }
    }

}
