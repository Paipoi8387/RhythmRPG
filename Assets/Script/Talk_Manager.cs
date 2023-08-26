using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoganeUnityLib.Example
{
    public class Talk_Manager : MonoBehaviour
    {
        TMP_Typewriter typewriter;
        public float show_speed;
        public bool show_all_content = true;
        [SerializeField] AudioSource sound_source;
        [SerializeField] AudioClip talk_clip;
        AudioClip talk_sound;

        void Awake()
        {
            typewriter = GetComponent<TMP_Typewriter>();
            talk_sound = talk_clip;
        }

        public void Show_Content(string content)
        {
            InvokeRepeating("Play_Talk_Sound", 0, 1 / show_speed);

            typewriter.Play
            (
                text: content,
                speed: show_speed,
                onComplete: () => Complete(),
                // ���r������s�ƂȂ��s�ō������ϓ����Ȃ��悤�ɂ���ɂ�true
                fixedLineHeight: true,
                // 1�s�ڂɃ��r�����鎞�ATextMeshPro��Margin�@�\���g���Ĉʒu����
                autoMarginTop: true
            );
        }

        public void Skip_Content()
        {
            typewriter.Skip();
        }

        void Complete()
        {
            show_all_content = true;
            CancelInvoke("Play_Talk_Sound");
        }


        void Play_Talk_Sound()
        {
            sound_source.PlayOneShot(talk_sound);
        }
    }
}