using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace FwPercept
{
    public sealed class ModuleVoiceRecognitionRealSense : IModule, IModuleVoice
    {

        private LanguageEnum language;
        private KindVoiceRecognitionModule kindModule;
        private List<string> comandos;
        
        public enum LanguageEnum 
        {
            Language_US_English = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_US_ENGLISH,
            Language_BR_Portuguese = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_BR_PORTUGUESE,
            Language_CN_Chinese = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_CN_CHINESE,
            Language_DE_German = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_DE_GERMAN,
            Language_FR_French = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_FR_FRENCH,
            Language_GB_English = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_GB_ENGLISH,
            Language_IT_Italian = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_IT_ITALIAN,
            Language_JP_Japanese = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_JP_JAPANESE,
            Language_ES_Spanish = PXCMVoiceRecognition.ProfileInfo.Language.LANGUAGE_US_SPANISH
        }

        public enum KindVoiceRecognitionModule 
        {
            VoiceCommand,
            VoiceDictation,
            VoiceSynthesis
        }

        public ModuleVoiceRecognitionRealSense(LanguageEnum Language, KindVoiceRecognitionModule KindModule, List<string> Comandos)
            : base() 
        {
            language = Language;
            kindModule = KindModule;
            if (KindModule.Equals(KindVoiceRecognitionModule.VoiceCommand))
                comandos = Comandos;
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(RealSense);
        }

        public string GetNameModule()
        {
            return "VoiceRegonitionRealSense";
        }

        public int GetIdModule()
        {
            return 4;
        }

        public string GetDescriptionLanguage()
        {
            string l = "";
            switch (language)
            {
                case LanguageEnum.Language_US_English:
                    l = "US_English";
                    break;
                case LanguageEnum.Language_BR_Portuguese:
                    l = "BR_Portuguese";
                    break;
                case LanguageEnum.Language_CN_Chinese:
                    l = "CN_Chinese";
                    break;
                case LanguageEnum.Language_DE_German:
                    l = "DE_German";
                    break;
                case LanguageEnum.Language_FR_French:
                    l = "FR_French";
                    break;
                case LanguageEnum.Language_GB_English:
                    l = "GB_English";
                    break;
                case LanguageEnum.Language_IT_Italian:
                    l = "IT_Italian";
                    break;
                case LanguageEnum.Language_JP_Japanese:
                    l = "JP_Japanese";
                    break;
                case LanguageEnum.Language_ES_Spanish:
                    l = "ES_Spanish";
                    break;
            }
            return l;
        }

        public List<string> GetComandos()
        {
            return comandos;
        }

        public IRecognition GetCommandFromFrame(EventArgs e) 
        {
            PXCMVoiceRecognition.Recognition data = ((VoiceRecognitionSenseEventArgs)e).ObjVoiceRecognition;
            RecognitionRealSense recognition = null;

            if (kindModule.Equals(KindVoiceRecognitionModule.VoiceCommand))
            {
                if (data.label >= 0)
                {
                    recognition = new RecognitionRealSense(comandos[data.label], data.label, data.confidence);
                }
            }
            return recognition;            
        }

        public List<RecognitionRealSense> GetNearCommandsFromFrame(VoiceRecognitionSenseEventArgs e)
        {
            PXCMVoiceRecognition.Recognition data = e.ObjVoiceRecognition;
            List<RecognitionRealSense> lista = new List<RecognitionRealSense>();
            RecognitionRealSense recognition = null;

            if (kindModule.Equals(KindVoiceRecognitionModule.VoiceCommand))
            {
                for (int i = 0; i < 4; i++)
                {
                    int label = data.nBest[i].label;
                    if (data.label >= 0)
                    {
                        recognition = new RecognitionRealSense(comandos[data.nBest[i].label], data.nBest[i].label, data.nBest[i].confidence);
                        lista.Add(recognition);
                        recognition = null;
                    }
                }                
            }
            return lista;
        }

        public string GetDictation(VoiceRecognitionSenseEventArgs e) 
        {
            PXCMVoiceRecognition.Recognition data = e.ObjVoiceRecognition;
            string dictation = null;

            if (kindModule.Equals(KindVoiceRecognitionModule.VoiceDictation))
            {
                if (data.label < 0)
                {
                    dictation = data.dictation;
                }                
            }
            return dictation;
        }

        public byte[] GetDataFromVoiceSynthesis(object sensor, string sentence, PXCMAudio.AudioFormat audioFormat)
        {
            PXCMSession.ImplDesc desc = new PXCMSession.ImplDesc();
            desc.cuids[0] = PXCMVoiceSynthesis.CUID;
            Boolean stop = false;
            byte[] bytes = null;

            if (KindModule.Equals(ModuleVoiceRecognitionRealSense.KindVoiceRecognitionModule.VoiceSynthesis))
            {
                for (uint i = 0; ; i++)
                {
                    if (stop)
                        break;

                    PXCMSession.ImplDesc desc1;
                    if (((PXCMSession)sensor).QueryImpl(ref desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    PXCMVoiceSynthesis vrec;
                    if (((PXCMSession)sensor).CreateImpl<PXCMVoiceSynthesis>(ref desc1, PXCMVoiceSynthesis.CUID, out vrec) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    for (uint j = 0; ; j++)
                    {
                        PXCMVoiceSynthesis.ProfileInfo pinfo;
                        if (vrec.QueryProfile(j, out pinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                        if (language.Equals(pinfo.language))
                        {
                            int tid = 0;
                            vrec.QueueSentence(sentence, out tid);
                            for (; ; )
                            {
                                PXCMAudio sample;
                                PXCMScheduler.SyncPoint sp;

                                if (vrec.ProcessAudioAsync(tid, out sample, out sp) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                                pxcmStatus sts = sp.Synchronize();

                                PXCMAudio.AudioData adata;

                                pxcmStatus status = sample.AcquireAccess(PXCMAudio.Access.ACCESS_READ, audioFormat, out adata);
                                if (status == pxcmStatus.PXCM_STATUS_NO_ERROR)
                                {
                                    bytes = adata.ToByteArray();
                                }

                                sample.Dispose();
                                sp.Dispose();
                                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                            }
                            stop = true;
                            break;
                        }
                    }
                }
            }
            return bytes;
        }

        public SoundPlayer GetSoundFromVoiceSynthesis(object sensor, string sentence, PXCMAudio.AudioFormat audioFormat)
        {            
            PXCMSession.ImplDesc desc = new PXCMSession.ImplDesc();
            desc.cuids[0] = PXCMVoiceSynthesis.CUID;
            Boolean stop = false;
            byte[] bytes = null;
            SoundPlayer player = null;

            if (KindModule.Equals(ModuleVoiceRecognitionRealSense.KindVoiceRecognitionModule.VoiceSynthesis))
            {
                for (uint i = 0; ; i++)
                {
                    if (stop)
                        break;

                    PXCMSession.ImplDesc desc1;
                    if (((PXCMSession)sensor).QueryImpl(ref desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    PXCMVoiceSynthesis vrec;
                    if (((PXCMSession)sensor).CreateImpl<PXCMVoiceSynthesis>(ref desc1, PXCMVoiceSynthesis.CUID, out vrec) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    for (uint j = 0; ; j++)
                    {
                        PXCMVoiceSynthesis.ProfileInfo pinfo;
                        if (vrec.QueryProfile(j, out pinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                        if (language.Equals(pinfo.language))
                        {
                            int tid = 0;
                            vrec.QueueSentence(sentence, out tid);
                            for (; ; )
                            {
                                PXCMAudio sample;
                                PXCMScheduler.SyncPoint sp;

                                if (vrec.ProcessAudioAsync(tid, out sample, out sp) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                                pxcmStatus sts = sp.Synchronize();

                                PXCMAudio.AudioData adata;

                                pxcmStatus status = sample.AcquireAccess(PXCMAudio.Access.ACCESS_READ, audioFormat, out adata);
                                if (status == pxcmStatus.PXCM_STATUS_NO_ERROR)
                                {
                                    bytes = adata.ToByteArray();
                                    MemoryStream ms = new MemoryStream(bytes);
                                    player = new SoundPlayer(ms);
                                }

                                sample.Dispose();
                                sp.Dispose();
                                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                            }
                            stop = true;
                            break;
                        }
                    }
                }
            }
            return player;
        }

        public KindVoiceRecognitionModule KindModule
        {
            get { return kindModule; }
        }

        public LanguageEnum Language
        {
            get { return language; }
        }
        
    }
}
