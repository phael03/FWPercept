using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Synthesis;
using System.Media;
using System.IO;

namespace FwPercept
{
    public sealed class ModuleSpeechRecognitionKinect : IModule, IModuleVoice
    {

        private string language;
        private KindSpeechRecognitionModule kindModule;
        private List<string> comandos;

        public static readonly string Language_US_English = "en-US";
        public static readonly string Language_GB_English = "en-GB";
        public static readonly string Language_DE_German = "de-DE";
        public static readonly string Language_BR_Portuguese = "pt-BR";
        public static readonly string Language_FR_French = "fr-FR";
        public static readonly string Language_IT_Italian = "it-IT";
        public static readonly string Language_JA_Japanese = "ja-JP";
        public static readonly string Language_ES_Spanish = "es-ES";
        public static readonly string Language_RU_Russian = "ru-RU";
        public static readonly string Language_ZH_Chinese = "zh-CN";

        public enum KindSpeechRecognitionModule
        {
            VoiceCommand,
            VoiceSynthesis
        }

        public ModuleSpeechRecognitionKinect(string Language, KindSpeechRecognitionModule KindModule, List<string> Comandos)
            : base() 
        {
            language = Language;
            kindModule = KindModule;
            if (KindModule.Equals(KindSpeechRecognitionModule.VoiceCommand))
                comandos = Comandos;
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(Kinect);
        }

        public string GetNameModule()
        {
            return "SpeechRecognitionKinect";
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
                case "en-US":
                    l = "US_English";
                    break;
                case "en-GB":
                    l = "GB_English";
                    break;
                case "de-DE":
                    l = "DE_German";
                    break;
                case "pt-BR":
                    l = "BR_Portuguese";
                    break;
                case "fr-FR":
                    l = "FR_French";
                    break;
                case "it-IT":
                    l = "IT_Italian";
                    break;
                case "ja-JP":
                    l = "JA_Japanese";
                    break;
                case "es-ES":
                    l = "ES_Spanish";
                    break;
                case "ru-RU":
                    l = "RU_Russian";
                    break;
                case "zh-CN":
                    l = "ZH_Chinese";
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
            string data = ((SpeechKinectEventArgs)e).ObjSpeech.Result.Semantics.Value.ToString();
            RecognitionKinect recognition = null;

            if (kindModule.Equals(KindSpeechRecognitionModule.VoiceCommand))
            {
                for (int i = 0; i < comandos.Count; i++)
                {
                    if (comandos[i].Equals(data)) 
                    {
                        recognition = new RecognitionKinect(data, i, ((SpeechKinectEventArgs)e).ObjSpeech.Result.Confidence);
                        break;
                    }
                }
            }
            return recognition;
        }

        public void GetSpeechSynthesis(string text)
        {
            if (kindModule.Equals(KindSpeechRecognitionModule.VoiceSynthesis))
            {
                SpeechSynthesizer synth = new SpeechSynthesizer();

                PromptBuilder pb = new PromptBuilder(new System.Globalization.CultureInfo(language));
                pb.AppendText(text);

                synth.SetOutputToDefaultAudioDevice();
                synth.Speak(pb);
            }
        }

        public void SaveSpeechSynthesis(string outFile,string text)
        {
            if (outFile.ToLower().Contains(".wav"))
            {
                if (kindModule.Equals(KindSpeechRecognitionModule.VoiceSynthesis))
                {
                    SpeechSynthesizer synth = new SpeechSynthesizer();

                    PromptBuilder pb = new PromptBuilder(new System.Globalization.CultureInfo(language));
                    pb.AppendText(text);

                    synth.SetOutputToWaveFile(outFile);
                    synth.Speak(pb);
                }    
            }            
        }

        public KindSpeechRecognitionModule KindModule
        {
            get { return kindModule; }
        }

        public string Language
        {
            get { return language; }
        }
        
    }
}
