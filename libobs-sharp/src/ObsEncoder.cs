using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OBS
{
    public class ObsEncoder : IObsContextData, IDisposable
    {
        internal IntPtr instance;    //pointer to unmanaged object

        public unsafe ObsEncoder(ObsEncoderType type, string id, string name)
        {
            if (type == ObsEncoderType.Video)
            {
                instance = libobs.obs_video_encoder_create(id, name, IntPtr.Zero, IntPtr.Zero);

                if (instance == null)
                    throw new ApplicationException("obs_video_encoder_create failed");
            }
            else if (type == ObsEncoderType.Audio)
            {
                instance = libobs.obs_audio_encoder_create(id, name, IntPtr.Zero, UIntPtr.Zero, IntPtr.Zero);

                if (instance == null)
                    throw new ApplicationException("obs_audio_encoder_create failed");
            }
        }

        public unsafe ObsEncoder(ObsEncoderType type, string id, string name, ObsData settings)
        {
            if (type == ObsEncoderType.Video)
            {
                instance = libobs.obs_video_encoder_create(id, name, settings.GetPointer(), IntPtr.Zero);

                if (instance == null)
                    throw new ApplicationException("obs_video_encoder_create failed");
            }
            else if (type == ObsEncoderType.Audio)
            {
                instance = libobs.obs_audio_encoder_create(id, name, settings.GetPointer(), UIntPtr.Zero, IntPtr.Zero);

                if (instance == null)
                    throw new ApplicationException("obs_audio_encoder_create failed");
            }
        }

        public unsafe ObsEncoder(IntPtr instance)
        {
            this.instance = instance;
            libobs.obs_output_addref(instance);
        }


        public unsafe void SetVideo(IntPtr video)
        {
            libobs.obs_encoder_set_video(instance, video);
        }

        public unsafe void SetAudio(IntPtr audio)
        {
            libobs.obs_encoder_set_audio(instance, audio);
        }

		public unsafe void Update(ObsData settings)
		{
			libobs.obs_encoder_update(instance, settings.GetPointer());
		}

		public unsafe IntPtr GetPointer()
        {
            return instance;
        }

        public unsafe void Dispose()
        {
            if (instance == IntPtr.Zero)
                return;

            libobs.obs_encoder_release(instance);

            instance = IntPtr.Zero;
        }
    }

    public enum ObsEncoderType : int
    {
        Video,
        Audio,
    };
    public enum MFEncoderRateControl : int
    {
	    H264RateControlCBR,
	    H264RateControlConstrainedVBR,
	    H264RateControlVBR,
	    H264RateControlCQP
    };

    public enum MFEncoderProfile : int
    {
	    H264ProfileBaseline,
	    H264ProfileMain,
	    H264ProfileHigh
    };
}
